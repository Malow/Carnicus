using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIExpandButtonScript : MonoBehaviour {
	public GUIExpandableScript startX;
	public GUIExpandableScript startY;
	public GUIExpandableScript endX;
	public GUIExpandableScript endY;
	public WorldScript world;
	public GUIScript gui;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onClick() {
		int sx = 0;
		int sy = 0;
		int ex = 0;
		int ey = 0;

		try
		{
			sx = int.Parse(this.startX.GetComponent<InputField> ().text);
		} catch
		{
		}
		try
		{
			sy = int.Parse(this.startY.GetComponent<InputField> ().text);
		}catch
		{
		}
		try
		{
			ex = int.Parse(this.endX.GetComponent<InputField> ().text);
		}catch
		{
		}
		try
		{
			ey = int.Parse(this.endY.GetComponent<InputField> ().text);
		}catch
		{
		}

		this.gui.SubmitPressed ();
		this.world.ExpandMap (sx, sy, ex, ey);
	}
}
