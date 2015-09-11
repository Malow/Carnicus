package com.malow.carnicusserver;

import java.util.ArrayList;
import java.util.List;

import com.malow.carnicusserver.models.ConvertStringToModel;
import com.malow.carnicusserver.models.ModelInterface;
import com.malow.carnicusserver.models.requests.LoginRequest;
import com.malow.carnicusserver.models.responses.LoginFailedResponse;
import com.malow.carnicusserver.models.responses.LoginSuccessfulResponse;
import com.malow.carnicusserver.models.responses.UnexpectedRequestResponse;
import com.malow.malowlib.NetworkPacket;
import com.malow.malowlib.Process;
import com.malow.malowlib.ProcessEvent;

public class LoginServer extends Process
{
	private List<Client> ccs = new ArrayList<Client>();
	
	private GameServer gameServer;

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
	
	public void clientConnected(Client cc)
	{
		this.ccs.add(cc);
		cc.SetNotifier(this);
		cc.Start();
		System.out.println("Client " + cc.GetClientID() + " connected.");
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
		System.out.println("CRITICAL ERROR, CANNOT FIND CLIENT AT LoginServer getClientWithId: " + id);
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
				if(request instanceof LoginRequest)
				{
					ModelInterface response = null;
					try 
					{
						int accountID = SQLConnector.authenticateAccount(((LoginRequest)request).username, ((LoginRequest)request).password);
						response = new LoginSuccessfulResponse();
						sender.authenticated = true;
						sender.accountID = accountID;
						this.ccs.remove(sender);
						this.gameServer.addClient(sender);
					} 
					catch (Exception e) 
					{
						if(e instanceof SQLConnector.WrongPasswordException)
						{
							response = new LoginFailedResponse("Wrong username/password.");
							System.out.println("Client " + (sender.GetClientID() + 1) + " failed login due to wrong password");
						}
						else
						{
							response = new LoginFailedResponse("Unexpected login error.");
							System.out.println("Client " + (sender.GetClientID() + 1) + " Unexpected login error.");
							e.printStackTrace();
						}
					}
					sender.SendData(response.toNetworkString());
				}
				else
				{
					System.out.println("Unexpected Request to LoginServer from client " + (sender.GetClientID() + 1) + ": " + request);
					System.out.println("Sent to Client " + (sender.GetClientID() + 1) + ": " + new UnexpectedRequestResponse(msg).toNetworkString());
					sender.SendData(new UnexpectedRequestResponse(msg).toNetworkString());
				}
			}
		}
	}
	
	public void SetGameServer(GameServer gameServer)
	{
		this.gameServer = gameServer;
	}
}
