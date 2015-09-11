using UnityEngine;
using System.Collections;

public class CharacterAnimationScript : MonoBehaviour 
{
	public AnimationScript leftArmScript;
	public AnimationScript rightArmScript;
	public AnimationScript feetScript;

	void Update() 
	{ 

	}

	void Start () 
	{
		
	}

	public void Running()
	{
		this.feetScript.currentFps = 8;
		this.feetScript.currentStartIndex = 0;
		this.feetScript.currentEndIndex = 8;
		this.feetScript.repeat = true;
		this.feetScript.play = true;
		this.feetScript.inverted = false;
		this.feetScript.resetAfter = false;
	}

	public void StandingStill()
	{
		this.feetScript.play = false;
		this.feetScript.Reset ();
	}

	public void SetRunningAnimationSpeed(float speed)
	{
		this.feetScript.animationSpeed = speed;
	}

	public void AttackRightArm()
	{
		this.rightArmScript.currentFps = 32;
		this.rightArmScript.currentStartIndex = 0;
		this.rightArmScript.currentEndIndex = 16;
		this.rightArmScript.repeat = false;
		this.rightArmScript.play = true;
		this.rightArmScript.inverted = false;
		this.rightArmScript.resetAfter = true;
	}

	public void AttackLeftArm()
	{
		this.leftArmScript.currentFps = 32;
		this.leftArmScript.currentStartIndex = 0;
		this.leftArmScript.currentEndIndex = 8;
		this.leftArmScript.repeat = false;
		this.leftArmScript.play = true;
		this.leftArmScript.inverted = false;
		this.leftArmScript.resetAfter = false;
	}

	public void HoldBlock()
	{
		this.leftArmScript.currentFps = 32;
		this.leftArmScript.currentStartIndex = 0;
		this.leftArmScript.currentEndIndex = 8;
		this.leftArmScript.repeat = false;
		this.leftArmScript.play = true;
		this.leftArmScript.inverted = false;
		this.leftArmScript.resetAfter = false;
	}

	public void ReleaseBlock()
	{
		this.leftArmScript.currentFps = 32;
		this.leftArmScript.currentStartIndex = 0;
		this.leftArmScript.currentEndIndex = 8;
		this.leftArmScript.repeat = false;
		this.leftArmScript.play = true;
		this.leftArmScript.inverted = true;
		this.leftArmScript.resetAfter = false;
	}

	public void SetBlockLevel(float blockLevel)
	{
		this.leftArmScript.currentFps = 32;
		this.leftArmScript.currentStartIndex = 0;
		this.leftArmScript.currentEndIndex = 8;
		this.leftArmScript.repeat = false;
		this.leftArmScript.play = true;
		this.leftArmScript.inverted = false;
		this.leftArmScript.resetAfter = false;
		this.leftArmScript.currentIndex = 8 * blockLevel;
	}

	/*
	public void HoldSwing()
	{
		this.topScript.currentFps = 16;
		this.topScript.currentStartIndex = 0;
		this.topScript.currentEndIndex = 8;
		this.topScript.repeat = false;
		this.topScript.play = true;
		this.topScript.inverted = false;
		this.topScript.resetAfter = false;
	}

	public void StopSwing()
	{
		this.topScript.currentFps = 16;
		this.topScript.currentStartIndex = 0;
		this.topScript.currentEndIndex = 8;
		this.topScript.repeat = false;
		this.topScript.play = true;
		this.topScript.inverted = true;
		this.topScript.resetAfter = false;
	}

	public void ReleaseSwing()
	{
		this.topScript.currentFps = 16;
		this.topScript.currentStartIndex = 8;
		this.topScript.currentEndIndex = 16;
		this.topScript.repeat = false;
		this.topScript.play = true;
		this.topScript.inverted = false;
		this.topScript.resetAfter = true;
	}
	*/
}



