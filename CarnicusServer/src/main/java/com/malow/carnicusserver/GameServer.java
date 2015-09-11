package com.malow.carnicusserver;

import java.util.ArrayList;
import java.util.List;

import com.malow.carnicusserver.models.ConvertStringToModel;
import com.malow.carnicusserver.models.ModelInterface;
import com.malow.carnicusserver.models.requests.CharacterInfoRequest;
import com.malow.carnicusserver.models.responses.CharacterInfoResponse;
import com.malow.carnicusserver.models.responses.NoCharacterFoundResponse;
import com.malow.carnicusserver.models.responses.SomethingWentHorriblyWrongResponse;
import com.malow.carnicusserver.models.responses.UnexpectedRequestResponse;
import com.malow.carnicusserver.models.updates.KeypressUpdate;
import com.malow.malowlib.NetworkPacket;
import com.malow.malowlib.Process;
import com.malow.malowlib.ProcessEvent;

public class GameServer extends Process
{
	private List<Client> ccs = new ArrayList<Client>();
	private WorldServer world;
	
	public GameServer()
	{
		this.world = new WorldServer();
		this.world.Start();
	}

	public void CloseSpecific()
	{
		this.world.Close();
		this.world.WaitUntillDone();
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
		this.ccs.add(cc);
		cc.SetNotifier(this);
		System.out.println("Client " + cc.GetClientID() + " added to GameServer.");
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
		System.out.println("CRITICAL ERROR, CANNOT FIND CLIENT AT GameServer getClientWithId: " + id);
		System.out.println("this.ccs.size: " + this.ccs.size());
		return null;
	}

	@Override
	public void Life() 
	{
		while(this.stayAlive)
		{
			ProcessEvent ev = this.WaitEvent();
			if(ev instanceof NetworkPacket)
			{
				String msg = ((NetworkPacket) ev).GetMessage();
				Client sender = this.getClientWithId(((NetworkPacket) ev).GetSenderID());
				ModelInterface request = ConvertStringToModel.toModel(msg);
				if(request instanceof CharacterInfoRequest)
				{
					ModelInterface response = null;
					try 
					{
						response = SQLConnector.getCharacterInfo(sender.accountID, sender.GetClientID());
						sender.pos.x = ((CharacterInfoResponse)response).posx;
						sender.pos.y = ((CharacterInfoResponse)response).posy;
						sender.pos.z = ((CharacterInfoResponse)response).posz;
						this.ccs.remove(sender);
						this.world.addClient(sender);
					} 
					catch (Exception e) 
					{
						if(e instanceof SQLConnector.NoCharacterFoundException)
						{
							response = new NoCharacterFoundResponse();
							System.out.println("Client " + (sender.GetClientID() + 1) + " No character found for account");
						}
						else
						{
							response = new SomethingWentHorriblyWrongResponse("Unexpected CharacterInfoRequest error.");
							System.out.println("Client " + (sender.GetClientID() + 1) + " Unexpected CharacterInfoRequest error.");
							e.printStackTrace();
						}
					}
					sender.SendData(response.toNetworkString());
				}
				else if(request instanceof KeypressUpdate)
				{
					sender.up = ((KeypressUpdate) request).up;
					sender.down = ((KeypressUpdate) request).down;
					sender.left = ((KeypressUpdate) request).left;
					sender.right = ((KeypressUpdate) request).right;
					sender.shift = ((KeypressUpdate) request).shift;
				}
				else
				{
					System.out.println("Unexpected Request to GameServer from client " + (sender.GetClientID() + 1) + ": " + request);
					System.out.println("Sent to Client " + (sender.GetClientID() + 1) + ": " + new UnexpectedRequestResponse(msg).toNetworkString());
					sender.SendData(new UnexpectedRequestResponse(msg).toNetworkString());
				}
			}
		}
	}
}
