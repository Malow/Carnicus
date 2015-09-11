using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {
	public int colCount =  4;
	public int rowCount =  2;

	public int currentFps = 8;
	public int currentStartIndex = 0;
	public int currentEndIndex = 8;
	public bool repeat = true;
	public float animationSpeed = 1.0f;
	public bool play = false;
	public bool inverted = false;
	public bool resetAfter = false;

	public float currentIndex = 0.0f;

	void Start () {
		this.UpdateScript();
	}

	void Update () {
		if (!this.play)
			return;
		
		this.UpdateScript ();
	}

	private void UpdateScript()
	{
		float frameIncrease = this.currentFps * this.animationSpeed;
		float aniDiff = Time.deltaTime * frameIncrease;
		if (this.inverted) {
			if (this.currentIndex - aniDiff < this.currentStartIndex) {
				if(this.repeat)
					this.currentIndex += (this.currentEndIndex - this.currentStartIndex);
				else
				{
					this.play = false;
					if(this.resetAfter)
						this.Reset();
					return;
				}
			}
			this.currentIndex -= aniDiff;

		} else {
			if (this.currentIndex + aniDiff > this.currentEndIndex) {
				if(this.repeat)
					this.currentIndex -= (this.currentEndIndex - this.currentStartIndex);
				else
				{
					this.play = false;
					if(this.resetAfter)
						this.Reset();
					return;
				}
			}
			this.currentIndex += aniDiff;
		}

		this.Render ((int)this.currentIndex);
	}

	private void Render(int ind)
	{
		// Size of every cell
		float sizeX = 1.0f / this.colCount;
		float sizeY = 1.0f / this.rowCount;
		Vector2 size =  new Vector2(sizeX, sizeY);
		
		// split into horizontal and vertical index
		var uIndex = ind % this.colCount;
		var vIndex = ind / this.colCount;
		
		// build offset
		// v coordinate is the bottom of the image in opengl so we need to invert.
		float offsetX = uIndex * size.x;
		float offsetY = (1.0f - size.y) - vIndex * size.y;
		Vector2 offset = new Vector2(offsetX, offsetY);
		
		GetComponent<Renderer>().material.SetTextureOffset ("_MainTex", offset);
		GetComponent<Renderer>().material.SetTextureScale  ("_MainTex", size);
	}

	public void Reset()
	{
		this.currentIndex = 0;
		this.Render ((int)this.currentIndex);
	}
}
