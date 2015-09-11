
using System;

public class NetworkPacket : ProcessEvent
{
	private String message;
	private long id;
	
	public NetworkPacket(String message, long SenderID)
	{
		this.message = message;
		this.id = SenderID;
	}
	
	public String GetMessage()
	{
		return this.message;
	}
	
	public long GetSenderID()
	{
		return this.id;
	}
}


