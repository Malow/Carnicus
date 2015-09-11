using UnityEngine;
using Newtonsoft.Json;

public class LoginRequest : ModelInterface
{
	public string type = "LoginRequest";
	public string username;
	public string password;

	public LoginRequest(string username, string password)
	{
		this.username = username;
		this.password = password;
	}

	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}

	public static LoginRequest ToModel(string networkString)
	{
		LoginRequest req = JsonConvert.DeserializeObject<LoginRequest>(networkString);
		if (req != null && req.type == "LoginRequest")
			return req;
		return null;
	}
}
