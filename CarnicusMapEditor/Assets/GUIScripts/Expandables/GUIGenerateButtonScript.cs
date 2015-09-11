using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIGenerateButtonScript : MonoBehaviour {
	public GUIExpandableScript x;
	public GUIExpandableScript y;
	public WorldScript world;
	public GUIScript gui;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onClick() {
		int x = int.Parse (this.x.GetComponent<InputField> ().text);
		int y = int.Parse (this.y.GetComponent<InputField> ().text);
		this.gui.SubmitPressed ();
		this.world.GenerateMap (x, y);
	}
}
