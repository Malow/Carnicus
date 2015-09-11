using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GUIScript : MonoBehaviour {

	public PlayerScript player;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject g1 = GameObject.FindWithTag ("DmgTaken");
		g1.GetComponent<Text> ().text = "Damage Taken: " + player.damageTaken.ToString ();

		GameObject g3 = GameObject.FindWithTag ("DmgDone");
		g3.GetComponent<Text> ().text = "Damage Done: " + player.damageDone.ToString ();
	}
}
