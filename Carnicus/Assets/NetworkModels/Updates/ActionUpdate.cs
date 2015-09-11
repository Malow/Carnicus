using UnityEngine;
using Newtonsoft.Json;


public class ActionUpdate : ModelInterface
{
	public string type = "ActionUpdate";
	public int actionType;
	public long creatorGUID;
	public long victimGUID;
	public long actionGUID;

	public float posx;
	public float posy;
	public float posz;

	public float dirx;
	public float diry;
	public float dirz;
	
	public float damage;
	public float blockReduction;
	
	public ActionUpdate (int actionType, 
	                     long creatorGUID,
	                     long victimGUID,
	                     long actionGUID,
	                     float posx,
	                     float posy,
	                     float posz,
	                     float dirx,
	                     float diry,
	                     float dirz,
	                     float damage,
	                     float blockReduction)
	{
		this.actionType = actionType;
		this.creatorGUID = creatorGUID;
		this.victimGUID = victimGUID;
		this.actionGUID = actionGUID;
		this.posx = posx;
		this.posy = posy;
		this.posz = posz;
		this.dirx = dirx;
		this.diry = diry;
		this.dirz = dirz;
		this.damage = damage;
		this.blockReduction = blockReduction;
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static ActionUpdate ToModel(string networkString)
	{
		ActionUpdate req = JsonConvert.DeserializeObject<ActionUpdate>(networkString);
		if (req != null && req.type == "ActionUpdate")
			return req;
		return null;
	}
}