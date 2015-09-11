using UnityEngine;
using System.Collections;

public class GUIScript : MonoBehaviour {
	private int currentTopMenuSelection = -1;
	public WorldScript world;
	
	public GUIExpandableScript sizeX;
	public GUIExpandableScript sizeY;
	public GUIExpandableScript generateButton;

	public GUIExpandableScript expandUp;
	public GUIExpandableScript expandDown;
	public GUIExpandableScript expandLeft;
	public GUIExpandableScript expandRight;
	public GUIExpandableScript expandButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TopMenuPressed(int id)
	{
		GUIExpandableScript.HideAllExpandables ();
		if (id != this.currentTopMenuSelection) {
			this.currentTopMenuSelection = id;
			
			if(id == 1)
				this.world.LoadMap ();
			if(id == 2)
				this.world.SaveMap ();
			if(id == 3)
				this.ShowGenerateMapElements();
			if(id == 4)
				this.ShowExpandMapElements();

		} else {
			this.currentTopMenuSelection = -1;
		}
	}

	public void SubmitPressed()
	{
		GUIExpandableScript.HideAllExpandables ();
		this.currentTopMenuSelection = -1;
	}

	private void ShowGenerateMapElements()
	{
		this.sizeX.gameObject.SetActive (true);
		this.sizeY.gameObject.SetActive (true);
		this.generateButton.gameObject.SetActive (true);
	}

	private void ShowExpandMapElements()
	{
		this.expandUp.gameObject.SetActive (true);
		this.expandDown.gameObject.SetActive (true);
		this.expandLeft.gameObject.SetActive (true);
		this.expandRight.gameObject.SetActive (true);
		this.expandButton.gameObject.SetActive (true);
	}
}
