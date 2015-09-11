package com.malow.carnicusserver;

import java.util.Scanner;

public class CarnicusServer 
{	
	public static void main(String [] args)
	{		
		System.out.println("Carnicus Server started");
		System.out.println("Type 'Exit' to close");
		
		int port = 7000;
		
		LoginServer loginServer = new LoginServer();
		GameServer gameServer = new GameServer();
		loginServer.SetGameServer(gameServer);
		loginServer.Start();
		gameServer.Start();
		
		ConnectionListener cl = new ConnectionListener(port, loginServer);
		cl.Start();
		
		LoadFromDatabase();
		
		String input = "";
		Scanner in = new Scanner(System.in);
		while(!input.equals("Exit"))
		{
			System.out.print("> ");
			input = in.next();
			
			if(input.equals("help"))
				PrintHelp();
			
			if(input.equals("reload:db"))
				LoadFromDatabase();
		}
				
		in.close();
		cl.Close();
		loginServer.Close();
		gameServer.Close();
		cl.WaitUntillDone();
		loginServer.WaitUntillDone();
		gameServer.WaitUntillDone();
		cl = null;
		loginServer = null;
		gameServer = null;
	}
	
	public static void LoadFromDatabase()
	{
		try {
			
		} catch (Exception e) {
			System.out.println("Error reading Abilities from database.");
			e.printStackTrace();
		}
	}
	
	public static void PrintHelp()
	{
		System.out.println("Commands:");
		System.out.println("help - Displays this help.");
		System.out.println("reload db - Reloads static information from database.");
	}
}
