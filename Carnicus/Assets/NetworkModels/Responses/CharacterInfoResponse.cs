
using System;
using Newtonsoft.Json;

public class CharacterInfoResponse : ModelInterface
{
	public string type = "CharacterInfoResponse";
	public string name;
	public float posx;
	public float posy;
	public float posz;
	public int GUID;
	
	public CharacterInfoResponse (string name, float posx, float posy, float posz, int GUID)
	{
		this.name = name;
		this.posx = posx;
		this.posy = posy;
		this.posz = posz;
		this.GUID = GUID;
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static CharacterInfoResponse ToModel(string networkString)
	{
		CharacterInfoResponse req = JsonConvert.DeserializeObject<CharacterInfoResponse>(networkString);
		if (req != null && req.type == "CharacterInfoResponse")
			return req;
		return null;
	}
}


