using UnityEngine;
using System.Collections;

public class OtherPlayersScript : MonoBehaviour {
	private float MAX_SPEED_ANIMATION = 5.0f;
	private float INTERPOLATION_THRESHOLD = 0.01f;

	public CharacterAnimationScript animationScript;
	public Vector3 targetPos = new Vector3(0, 0, 0);
	public Vector3 dir = new Vector3(0, 0, 0);
	public Vector3 forward = new Vector3(0, 0, 0);
	public Vector3 targetForward = new Vector3(0, 0, 0);
	public Vector3 forwardDir = new Vector3(0, 0, 0);
	public float speed;
	public float blockLevel;

	public long GUID;

	void Start () {
		this.animationScript.StandingStill();
	}

	void Update () {
		this.dir = this.targetPos - this.transform.position;
		this.forwardDir = this.targetForward - this.forward;

		if (dir.magnitude < INTERPOLATION_THRESHOLD) {
			this.transform.position = this.targetPos;
			this.animationScript.StandingStill();
		} else {
			this.transform.position += this.dir * (Time.deltaTime / (1.0f / Constants.SERVER_FPS));
			this.animationScript.Running ();
		}

		this.forward += this.forwardDir * (Time.deltaTime / (1.0f / Constants.SERVER_FPS));
		float AngleRad = Mathf.Atan2 (this.forward.x, -this.forward.y);
		float AngleDeg = (180 / Mathf.PI) * AngleRad;
		this.transform.rotation = Quaternion.Euler (0, 0, AngleDeg);
	}

	public void SetNewTargetPos(Vector3 target, float speed, Vector3 forward, float blockLevel)
	{
		this.speed = speed;
		if (blockLevel > this.blockLevel || blockLevel == 1.0f)
			this.animationScript.HoldBlock ();
		else
			this.animationScript.ReleaseBlock ();
		this.blockLevel = blockLevel;
		this.animationScript.SetRunningAnimationSpeed (this.speed / this.MAX_SPEED_ANIMATION);
		this.targetPos = target;
		this.targetForward = forward;
	}
	
	public void ProcessAction(ActionUpdate au)
	{
		if (au.actionType == Action.MELEE_ATTACK_START) {
			if (au.creatorGUID == this.GUID) {
				this.animationScript.AttackRightArm();
			}
		}
	}
}
