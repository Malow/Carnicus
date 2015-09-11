
using UnityEngine;
using Newtonsoft.Json;

public class KeypressUpdate : ModelInterface
{
	public string type = "KeypressUpdate";
	public bool up;
	public bool down;
	public bool left;
	public bool right;
	public bool shift;
	public bool attack;
	public bool attack2;
	public bool attack3;
	public float diff;
	public int inputSequenceNumber;
	public float forwardX;
	public float forwardY;
	public float forwardZ;

	public bool noInput;
	
	public KeypressUpdate(bool up, bool down, bool left, bool right, bool shift, bool attack, bool attack2, bool attack3, float diff, int inputSequenceNumber, 
	                      float forwardX, float forwardY, float forwardZ, bool noInput)
	{
		this.up = up;
		this.down = down;
		this.left = left;
		this.right = right;
		this.shift = shift;
		this.attack = attack;
		this.attack2 = attack2;
		this.attack3 = attack3;
		this.diff = diff;
		this.inputSequenceNumber = inputSequenceNumber;
		this.forwardX = forwardX;
		this.forwardY = forwardY;
		this.forwardZ = forwardZ;
		this.noInput = noInput;
	}
	
	public override string ToNetworkString()
	{
		return JsonConvert.SerializeObject(this);
	}
	
	public static KeypressUpdate ToModel(string networkString)
	{
		KeypressUpdate req = JsonConvert.DeserializeObject<KeypressUpdate>(networkString);
		if (req != null && req.type == "KeypressUpdate")
			return req;
		return null;
	}
}
