
using System;
using Newtonsoft.Json;

public class LoginFailedResponse : ModelInterface
{
	public string type = "LoginFailedResponse";
	public string errorMsg;

	public LoginFailedResponse (string errorMsg)
	{
		this.errorMsg = errorMsg;
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static LoginFailedResponse ToModel(string networkString)
	{
		LoginFailedResponse req = JsonConvert.DeserializeObject<LoginFailedResponse>(networkString);
		if (req != null && req.type == "LoginFailedResponse")
			return req;
		return null;
	}
}


