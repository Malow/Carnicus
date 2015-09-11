
using System;
using Newtonsoft.Json;

public class LoginSuccessfulResponse : ModelInterface
{
	public string type = "LoginSuccessfulResponse";
	public LoginSuccessfulResponse ()
	{
	}

	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static LoginSuccessfulResponse ToModel(string networkString)
	{
		LoginSuccessfulResponse req = JsonConvert.DeserializeObject<LoginSuccessfulResponse>(networkString);
		if (req != null && req.type == "LoginSuccessfulResponse")
			return req;
		return null;
	}
}


