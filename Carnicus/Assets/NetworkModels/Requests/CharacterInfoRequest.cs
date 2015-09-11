using UnityEngine;
using Newtonsoft.Json;

public class CharacterInfoRequest : ModelInterface
{
	public string type = "CharacterInfoRequest";
	
	public CharacterInfoRequest()
	{
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static CharacterInfoRequest ToModel(string networkString)
	{
		CharacterInfoRequest req = JsonConvert.DeserializeObject<CharacterInfoRequest>(networkString);
		if (req != null && req.type == "CharacterInfoRequest")
			return req;
		return null;
	}
}