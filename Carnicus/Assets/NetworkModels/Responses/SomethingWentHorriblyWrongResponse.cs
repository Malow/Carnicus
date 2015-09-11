
using System;
using Newtonsoft.Json;

public class SomethingWentHorriblyWrongResponse : ModelInterface
{
	public string type = "SomethingWentHorriblyWrongResponse";
	public string errorMsg;
	
	public SomethingWentHorriblyWrongResponse (string errorMsg)
	{
		this.errorMsg = errorMsg;
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static SomethingWentHorriblyWrongResponse ToModel(string networkString)
	{
		SomethingWentHorriblyWrongResponse req = JsonConvert.DeserializeObject<SomethingWentHorriblyWrongResponse>(networkString);
		if (req != null && req.type == "SomethingWentHorriblyWrongResponse")
			return req;
		return null;
	}
}


