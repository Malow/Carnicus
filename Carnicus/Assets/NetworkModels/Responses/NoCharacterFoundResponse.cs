using System;
using Newtonsoft.Json;

public class NoCharacterFoundResponse : ModelInterface
{
	public string type = "NoCharacterFoundResponse";
	public NoCharacterFoundResponse ()
	{
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static NoCharacterFoundResponse ToModel(string networkString)
	{
		NoCharacterFoundResponse req = JsonConvert.DeserializeObject<NoCharacterFoundResponse>(networkString);
		if (req != null && req.type == "NoCharacterFoundResponse")
			return req;
		return null;
	}
}
