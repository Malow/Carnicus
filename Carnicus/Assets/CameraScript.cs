using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

	void Start () {
		this.GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
	}

	void Update () {
		//this.transform.position = this.player.transform.position + new Vector3(0, 0, -10);
	}
}
