using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {
	private float MAX_SPEED_ANIMATION = 3.0f;
	private float BASE_SPEED = 2.0f;

	public CharacterAnimationScript animationScript;

	public long GUID = 0;

	private float speed = 0.0f;	
	public int inputSequenceNumber = 0;
	public List<KeypressUpdate> pendingInputs = new List<KeypressUpdate> ();

	public float damageTaken = 0;
	public float damageDone = 0;

	public Vector3 forward = new Vector3(0, -1, 0);

	void Start () {
		this.animationScript.StandingStill();
	}

	void Update () {		
	}

	public void ProcessServerMessage(Entity e)
	{
		this.speed = e.speed;
		this.transform.position = new Vector3 (e.posx, e.posy, e.posz);
		this.forward = new Vector3 (e.forwardx, e.forwardy, e.forwardz);

		int i = 0;
		while (i < this.pendingInputs.Count) {
			KeypressUpdate input = this.pendingInputs[i];
			if(input.inputSequenceNumber <= e.lastProcessedInput)
			{
				this.pendingInputs.RemoveAt(i);
			}
			else
			{
				this.ApplyInput(input, false);
				i++;
			}
		}
	}

	public void ApplyInput(KeypressUpdate ku, bool clientSide)
	{
		this.forward = new Vector3 (ku.forwardX, ku.forwardY, ku.forwardZ);
		Vector3 myDir = new Vector3 (0, 0, 0);
		if (ku.up) {
			myDir.y += 1;
		}
		if (ku.down) {
			myDir.y -= 1;
		}
		if (ku.left) {
			myDir.x -= 1;
		}
		if (ku.right) {
			myDir.x += 1;
		}

		myDir.Normalize ();

		if (ku.shift) {
			this.speed = this.BASE_SPEED * 1.5f;
		}	
		else {
			this.speed = this.BASE_SPEED;
		}

		float directionSpeed = (Vector3.Dot (myDir, this.forward) * 0.25f) + 0.75f;

		myDir *= this.speed * directionSpeed;

		this.transform.position += new Vector3 (myDir.x, myDir.y, 0) * ku.diff;


		float AngleRad = Mathf.Atan2 (this.forward.x, -this.forward.y);
		float AngleDeg = (180 / Mathf.PI) * AngleRad;
		this.transform.rotation = Quaternion.Euler (0, 0, AngleDeg);

		if (myDir.magnitude > 0.0f) {
			this.animationScript.SetRunningAnimationSpeed (myDir.magnitude / this.MAX_SPEED_ANIMATION);
			this.animationScript.Running ();
		} else {	
			this.animationScript.StandingStill ();
		}

		if (clientSide) {
			if(ku.attack)
			{
				this.animationScript.AttackRightArm();
			}

			if(ku.attack2)
			{
				this.animationScript.HoldBlock();
			}
			else
			{
				this.animationScript.ReleaseBlock();
			}

			if(ku.attack3)
			{
				// Arrow animation
			}
		}

		Camera.main.transform.position = this.transform.position + new Vector3(0, 0, -10);
	}

	public void ProcessAction(ActionUpdate au)
	{
		if (au.actionType == Action.MELEE_ATTACK_HIT) {
			if (au.victimGUID == this.GUID) {
				this.damageTaken += au.damage;
			}
			if (au.creatorGUID == this.GUID && au.victimGUID != -1) {
				this.damageDone += au.damage;
			}
		}
		if (au.actionType == Action.PROJECTILE_END) {
			if (au.victimGUID == this.GUID) {
				this.damageTaken += au.damage;
			}
			if (au.creatorGUID == this.GUID && au.victimGUID != -1) {
				this.damageDone += au.damage;
			}
		}
	}
}

