using UnityEngine;
using System.Collections;

public class GUITopMenuScript : MonoBehaviour {
	public GUIScript gui;
	public int topMenuId;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onClick() {
		this.gui.TopMenuPressed (this.topMenuId);
	}
}
