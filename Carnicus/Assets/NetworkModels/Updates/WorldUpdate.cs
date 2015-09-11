using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class WorldUpdate : ModelInterface
{
	public string type = "WorldUpdate";
	public string dataString;
	public float tileZ;
	public int sequenceNumber;
	public int totalSequencePackets;
	
	public WorldUpdate (string dataString, float tileZ, int sequenceNumber, int totalSequencePackets)
	{
		this.dataString = dataString;
		this.tileZ = tileZ;
		this.sequenceNumber = sequenceNumber;
		this.totalSequencePackets = totalSequencePackets;
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static WorldUpdate ToModel(string networkString)
	{
		WorldUpdate req = JsonConvert.DeserializeObject<WorldUpdate>(networkString);
		if (req != null && req.type == "WorldUpdate")
			return req;
		return null;
	}
}