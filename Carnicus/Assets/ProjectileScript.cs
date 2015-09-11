using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	public Vector3 orig;
	public Vector3 direction = new Vector3(0, 0, 0);
	public float speed = 0.0f;

	public long GUID;
	public float timeFlown = 0.0f;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if (this.direction.magnitude != 0.0f && this.speed != 0.0f) {
			this.transform.position += this.direction.normalized * Time.deltaTime * this.speed;
			this.timeFlown += Time.deltaTime;
		}
	}
}
