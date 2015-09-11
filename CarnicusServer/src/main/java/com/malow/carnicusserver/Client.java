package com.malow.carnicusserver;

import java.util.ArrayList;

import com.malow.carnicusserver.models.updates.KeypressUpdate;
import com.malow.carnicusserver.models.updates.WorldUpdate;
import com.malow.carnicusserver.world.World;
import com.malow.malowlib.NetworkChannel;
import com.malow.malowlib.Process;
import com.malow.malowlib.Vector3;

public class Client 
{
    public final float BASE_SPEED = 2.0f;
    public final float BLOCK_TIME = 0.25f;
    
	private NetworkChannel nc;
	public boolean authenticated = false;
	public int accountID = -1;
    public boolean up = false;
    public boolean down = false;
    public boolean left = false;
    public boolean right = false;
    public boolean shift = false;
    public float currentSpeed = BASE_SPEED;
    public int lastProcessedInput = 0;
    
    public Vector3 pos = new Vector3();
    public Vector3 dir = new Vector3();
    public Vector3 forward = new Vector3();
    
	public float lastAttackTime = 0.0f;
	public float lastAttack2Time = 0.0f;
	public boolean isBlocking = false;
	public float blockLevel = 0.0f;
    	
	public Client(NetworkChannel nc)
	{
		this.nc = nc;
	}
	
	public void SendData(String msg)
	{
		this.nc.SendData(msg);
	}
	
	public void Close()
	{
		this.nc.Close();
	}
	
	public void WaitUntillDone()
	{
		this.nc.WaitUntillDone();
	}
	
	public void SetNotifier(Process notifier)
	{
		this.nc.SetNotifier(notifier);
	}
	
	public void Start()
	{
		this.nc.Start();
	}
	
	public long GetClientID()
	{
		return this.nc.GetChannelID();
	}	
	
	public boolean IsAlive()
	{
		return this.nc.GetState() == Process.RUNNING;
	}
	
	public void SavePositionToDatabase()
	{
		try {
			SQLConnector.updateCharacterPosition(this.accountID, this.pos.x, this.pos.y, this.pos.z);
		} catch (Exception e) {
			System.out.println("CRITICAL ERROR: Failed to save character position to database.");
			e.printStackTrace();
		}
	}
	
	public void ApplyInput(KeypressUpdate ku)
	{
		this.forward = new Vector3(ku.forwardX, ku.forwardY, ku.forwardZ);
		this.lastProcessedInput = ku.inputSequenceNumber;
		
		this.dir = new Vector3();
		if (ku.up) {
			this.dir.y += 1.0f;
		}
		if (ku.down) {
			this.dir.y -= 1.0f;
		}
		if (ku.left) {
			this.dir.x -= 1.0f;
		}
		if (ku.right) {
			this.dir.x += 1.0f;
		}
		
		this.dir.normalize();

		if (ku.shift) {
			this.currentSpeed = this.BASE_SPEED * 1.5f;
		}	
		else {
			this.currentSpeed = this.BASE_SPEED;
		}

		float directionSpeed = (this.dir.dot(this.forward) * 0.25f) + 0.75f;
		
		this.dir = this.dir.mul(this.currentSpeed * directionSpeed);
		
		this.pos = this.pos.add(this.dir.mul(ku.diff));
	}

	public void Update(float diff) 
	{
		if(this.isBlocking)
		{
			this.blockLevel +=  diff / this.BLOCK_TIME;
			if(this.blockLevel > 1.0f)
				this.blockLevel = 1.0f;
		}
		else
		{
			this.blockLevel -=  diff / this.BLOCK_TIME;
			if(this.blockLevel < 0.0f)
				this.blockLevel = 0.0f;
		}
	}

	public void SendWorldUpdateData(ArrayList<String> strings) {
		int i = 0;
		int tot = strings.size();
		for(String s : strings)
		{
			this.SendData(new WorldUpdate(s, World.TILE_Z, i, tot).toNetworkString());
			i++;
		}
	}
	
	public boolean IsInsideHitbox(Vector3 impactPoint)
	{
		this.forward.normalize();
		Vector3 right = new Vector3(this.forward.y, -this.forward.x, 0);
		right.normalize();
		
		Vector3 c1 = this.pos.add(this.forward.mul(0.075f)).add(right.mul(0.25f));
		Vector3 c2 = this.pos.add(this.forward.mul(0.075f)).sub(right.mul(0.25f));
		Vector3 c3 = this.pos.sub(this.forward.mul(0.075f)).add(right.mul(0.25f));
		Vector3 c4 = this.pos.sub(this.forward.mul(0.075f)).sub(right.mul(0.25f));
		
		if(this.PointInTriangle(impactPoint, c1, c2, c3) || this.PointInTriangle(impactPoint, c2, c3, c4))
		{
			return true;
		}
		return false;
	}
	
	
	
	private float sign(Vector3 p1, Vector3 p2, Vector3 p3)
	{
	  return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
	}

	private boolean PointInTriangle(Vector3 pt, Vector3 v1, Vector3 v2, Vector3 v3)
	{
	  boolean b1, b2, b3;

	  b1 = sign(pt, v1, v2) < 0.0f;
	  b2 = sign(pt, v2, v3) < 0.0f;
	  b3 = sign(pt, v3, v1) < 0.0f;

	  return ((b1 == b2) && (b2 == b3));
	}
}
