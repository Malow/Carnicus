package com.malow.carnicusserver;

import java.util.ArrayList;
import java.util.List;

import com.malow.carnicusserver.models.ConvertStringToModel;
import com.malow.carnicusserver.models.Entity;
import com.malow.carnicusserver.models.ModelInterface;
import com.malow.carnicusserver.models.responses.UnexpectedRequestResponse;
import com.malow.carnicusserver.models.updates.ActionUpdate;
import com.malow.carnicusserver.models.updates.KeypressUpdate;
import com.malow.carnicusserver.models.updates.PositionUpdate;
import com.malow.carnicusserver.models.updates.ServerSettingsUpdate;
import com.malow.carnicusserver.world.World;
import com.malow.malowlib.NetworkPacket;
import com.malow.malowlib.Process;
import com.malow.malowlib.ProcessEvent;
import com.malow.malowlib.Vector3;

public class WorldServer extends Process
{
	public final float SERVER_FPS = 10.0f;
	private List<Client> ccs = new ArrayList<Client>();
	private List<Projectile> projectiles = new ArrayList<Projectile>();
	private long lastTime = System.nanoTime();
	private float accuDiff = 0.0f;
	private World world;
	
	public WorldServer()
	{
		this.world = new World();
		this.world.Load();
	}
	
	public void CloseSpecific()
	{
		for(int i = 0; i < this.ccs.size(); i++)
		{
			this.ccs.get(i).Close();
		}
		for(int i = 0; i < this.ccs.size(); i++)
		{
			this.ccs.get(i).WaitUntillDone();
		}
		this.ccs = null;
	}
	
	public void addClient(Client cc)
	{
		cc.SetNotifier(this);
		this.ccs.add(cc);
		cc.SendData(new ServerSettingsUpdate(this.SERVER_FPS).toNetworkString());
		cc.SendWorldUpdateData(this.world.GetWorldUpdateStrings());
		System.out.println("Client " + cc.GetClientID() + " added to WorldServer.");
	}
	
	public Client getClientWithId(long id)
	{
		for(Client c: this.ccs)
		{
			if(c.GetClientID() == id)
			{
				return c;
			}
		}
		System.out.println("CRITICAL ERROR, CANNOT FIND CLIENT AT WorldServer getClientWithId: " + id);
		System.out.println("this.ccs.size: " + this.ccs.size());
		return null;
	}
		
	private void SendWorldState(PositionUpdate pu)
	{
		String s = pu.toNetworkString();
		for(Client cc : this.ccs)
		{
			cc.SendData(s);
		}
	}
	
	private boolean ValidateKeypressUpdate(KeypressUpdate request) {
		//if(request.diff)
		// Validate diff somehow, maybe add up diff of each client every 10 second window, and make sure the client diff isnt higher than server diff
		return true;
	}
	
	@Override
	public void Life() 
	{
		while(this.stayAlive)
		{
			float diff = this.GetDiff();
			
			ProcessEvent ev = this.PeekEvent();
			while(ev != null)
			{
				if(ev instanceof NetworkPacket)
				{
					String msg = ((NetworkPacket) ev).GetMessage();
					Client sender = this.getClientWithId(((NetworkPacket) ev).GetSenderID());
					ModelInterface request = ConvertStringToModel.toModel(msg);
					if(request instanceof KeypressUpdate)
					{
						if(this.ValidateKeypressUpdate((KeypressUpdate) request))
						{
							sender.ApplyInput((KeypressUpdate) request);
							this.CalculateAttacks((KeypressUpdate) request, sender, diff);
						}
					}
					else
					{
						System.out.println("Unexpected Request to WorldServer from client " + (sender.GetClientID() + 1) + ": " + request);
						System.out.println("Sent to Client " + (sender.GetClientID() + 1) + ": " + new UnexpectedRequestResponse(msg).toNetworkString());
						sender.SendData(new UnexpectedRequestResponse(msg).toNetworkString());
					}
				}
				ev = this.PeekEvent();
			}
			
			// Update projectiles
			List<Projectile> deadProjectiles = new ArrayList<Projectile>();
			for(Projectile proj : this.projectiles)
			{
				proj.flownTime += diff;
				if(proj.flownTime > proj.maxTime)
				{
					deadProjectiles.add(proj);	
					this.BroadcastProjectileHit(proj, -1, -1, -1);
				}
				else
				{
					proj.pos.x += proj.dir.x * diff * proj.speed;
					proj.pos.y += proj.dir.y * diff * proj.speed;
					
					for(Client cc : this.ccs)
					{
						if(cc.GetClientID() != proj.attackerGUI)
						{
							Vector3 dist = proj.pos.sub(cc.pos);						
							float distance = dist.length();
							if(distance < 1.0f)
							{
								if(cc.IsInsideHitbox(proj.pos))
								{
									deadProjectiles.add(proj);		
									float damage = 10.0f;
									float blockReduction = 0.0f;
									
									if(proj.type == Projectile.MELEE_ATTACK)
									{
										damage = 10.0f;
										// BlockCoef depending on attack
										//cc.blockLevel -= x // Add pushing back block on successfull blocks
									}	
									else if(proj.type == Projectile.ARROW)
									{
										damage = 15.0f;
									}
									
									Vector3 projectileForward = proj.dir;
									projectileForward.normalize();
									
									Vector3 defenderForward = cc.forward;
									defenderForward.normalize();
									Vector3 defenderLeft = new Vector3(-defenderForward.y, defenderForward.x, 0);
									defenderLeft.normalize();
									
									float blockLevel = cc.blockLevel;
									float percentForward = blockLevel;
									float percentLeft = 1.0f - blockLevel;
									
									Vector3 blockDirection = defenderForward.mul(percentForward).add(defenderLeft.mul(percentLeft));
									blockDirection.normalize();
									
									float blockDot = projectileForward.dot(blockDirection);

									System.out.println("blockDot: " + blockDot);
									if(blockDot < 0.0f)
									{
										float directionPercent = Math.abs(blockDot);
										blockReduction = directionPercent * ((blockLevel / 2) + 0.5f);
										System.out.println("directionPercent: " + directionPercent);
									}
									
									damage -= damage * blockReduction;

									System.out.println("Damage: " + damage);
									System.out.println("blockReduction: " + blockReduction);
									System.out.println("");
									System.out.println("");
									this.BroadcastProjectileHit(proj, cc.GetClientID(), damage, blockReduction);
								}
							}
						}
					}
				}
			}
			for(Projectile proj : deadProjectiles)
			{
				this.projectiles.remove(proj);
			}
			
			if(this.accuDiff + diff > 1.0f / this.SERVER_FPS)
			{
				diff += this.accuDiff;
				this.accuDiff = 0.0f;
				
				ArrayList<Entity> entities = new ArrayList<Entity>();
				List<Client> deadCCs = new ArrayList<Client>();
				
				for(Client cc : this.ccs)
				{
					if(!cc.IsAlive())
					{
						cc.SavePositionToDatabase();
						deadCCs.add(cc);
					}
					else
					{
						cc.Update(diff);
						Entity e = new Entity(cc.GetClientID(), cc.pos.x, cc.pos.y, cc.pos.z, cc.currentSpeed, cc.forward.x, cc.forward.y, cc.forward.z, cc.lastProcessedInput, cc.blockLevel);
						entities.add(e);
					}
				}
				for(Client cc : deadCCs)
				{
					this.ccs.remove(cc);
				}
				
				PositionUpdate pu = new PositionUpdate(entities);
				this.SendWorldState(pu);
			}
			else
				this.accuDiff += diff;
			try {
				Thread.sleep(1);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
	}

	private void BroadcastAction(ActionUpdate au)
	{
		String s = au.toNetworkString();
		for(Client cc : this.ccs)
		{
			cc.SendData(s);
		}
	}
	
	private void CalculateAttacks(KeypressUpdate ku, Client c, float diff) {
		if(ku.attack)
		{
			if ((System.nanoTime() - c.lastAttackTime) / 1000000000.0f > 0.5f)
			{
				c.lastAttackTime = System.nanoTime();
				
				Vector3 attackerPos = c.pos;
				Vector3 attackerForward = c.forward;
				attackerForward.normalize();
				Vector3 attackerRight = new Vector3(attackerForward.y, -attackerForward.x, 0);
				attackerRight.normalize();
				Vector3 impactPointStart = attackerPos.add(attackerForward.mul(0.5f)).add((attackerRight.mul(0.3f)));
				Projectile proj = new Projectile(Projectile.MELEE_ATTACK, c.GetClientID(), impactPointStart.x, impactPointStart.y, impactPointStart.z, 2.0f, 0.25f, attackerForward.x, attackerForward.y, attackerForward.z);
				this.BroadcastAction(new ActionUpdate(Action.MELEE_ATTACK_START, c.GetClientID(), -1, -1, -1, -1, -1, c.forward.x, c.forward.y, c.forward.z, -1, -1));
				this.projectiles.add(proj);
			}
		}
		if(ku.attack2)
		{
			c.isBlocking = true;
		}
		else
		{
			c.isBlocking = false;
		}
		if(ku.attack3)
		{
			if ((System.nanoTime() - c.lastAttack2Time) / 1000000000.0f > 1.0f)
			{
				c.lastAttack2Time = System.nanoTime();
				Projectile proj = new Projectile(Projectile.ARROW, c.GetClientID(), c.pos.x, c.pos.y, c.pos.z, 10.0f, 3.0f, c.forward.x, c.forward.y, c.forward.z);
				this.BroadcastAction(new ActionUpdate(Action.PROJECTILE_START, c.GetClientID(), -1, proj.GUID, c.pos.x + c.forward.x, 
						c.pos.y + c.forward.y, c.pos.z + c.forward.z, c.forward.x, c.forward.y, c.forward.z, -1, -1));
				this.projectiles.add(proj);
			}
		}
	}
	
	private void BroadcastProjectileHit(Projectile proj, long victimGUID, float damage, float blockReduction)
	{
		if(proj.type == Projectile.MELEE_ATTACK)
		{
			this.BroadcastAction(new ActionUpdate(Action.MELEE_ATTACK_HIT, proj.attackerGUI, victimGUID, proj.GUID, proj.pos.x, proj.pos.y, proj.pos.z, -1, -1, -1, damage, blockReduction));	
		}	
		else if(proj.type == Projectile.ARROW)
		{
			this.BroadcastAction(new ActionUpdate(Action.PROJECTILE_END, proj.attackerGUI, victimGUID, proj.GUID, proj.pos.x, proj.pos.y, proj.pos.z, -1, -1, -1, damage, blockReduction));	
		}
	}

	private float GetDiff()
	{
		long currentTime = System.nanoTime();
		float diff = (currentTime - this.lastTime) / 1000000000.0f;
		this.lastTime = currentTime;
		return diff;
	}
	
}
