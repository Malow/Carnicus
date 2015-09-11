package com.malow.carnicusserver;

import com.malow.malowlib.NetworkChannel;
import com.malow.malowlib.NetworkServer;

public class ConnectionListener extends NetworkServer
{
	private LoginServer server = null;
	
	public ConnectionListener(int port, LoginServer server)
	{
		super(port);
		this.server = server;
	}
	
	public void ClientConnected(NetworkChannel cc)
	{
		this.server.clientConnected(new Client(cc));
	}
}