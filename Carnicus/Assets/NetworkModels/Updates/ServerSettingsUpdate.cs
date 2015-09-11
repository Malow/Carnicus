using UnityEngine;
using Newtonsoft.Json;


public class ServerSettingsUpdate : ModelInterface
{
	public string type = "ServerSettingsUpdate";
	public float serverFps;
	
	public ServerSettingsUpdate (float serverFps)
	{
		this.serverFps = serverFps;
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static ServerSettingsUpdate ToModel(string networkString)
	{
		ServerSettingsUpdate req = JsonConvert.DeserializeObject<ServerSettingsUpdate>(networkString);
		if (req != null && req.type == "ServerSettingsUpdate")
			return req;
		return null;
	}
}