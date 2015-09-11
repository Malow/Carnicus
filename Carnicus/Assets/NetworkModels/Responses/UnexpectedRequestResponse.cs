using System;
using Newtonsoft.Json;
using UnityEngine;

public class UnexpectedRequestResponse : ModelInterface
{
	public string type = "UnexpectedRequestResponse";
	public string request;

	public UnexpectedRequestResponse (string request)
	{
		this.request = request;
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static UnexpectedRequestResponse ToModel(string networkString)
	{
		UnexpectedRequestResponse req = JsonConvert.DeserializeObject<UnexpectedRequestResponse>(networkString);
		if (req != null && req.type == "UnexpectedRequestResponse")
			return req;
		return null;
	}
}


