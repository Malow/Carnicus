using UnityEngine;
using System;
using System.Collections.Generic;

public class CameraScript : MonoBehaviour {
	private float speed = 5.0f;
	public WorldScript world;

	private int currentTile = Tile.TILE_GRASS;

	void Start () {
		this.GetComponent<Camera>().transparencySortMode = TransparencySortMode.Orthographic;
	}

	void Update () {
		float curSpeed = this.speed;
		if (Input.GetKey (KeyCode.LeftShift))
			curSpeed *= 2;

		if (Input.GetKey ("w"))
			this.transform.position += new Vector3(0, 1, 0) * Time.deltaTime * curSpeed;
		if (Input.GetKey ("s"))
			this.transform.position += new Vector3(0, -1, 0) * Time.deltaTime * curSpeed;
		if (Input.GetKey ("a"))
			this.transform.position += new Vector3(-1, 0, 0) * Time.deltaTime * curSpeed;
		if (Input.GetKey ("d"))
			this.transform.position += new Vector3(1, 0, 0) * Time.deltaTime * curSpeed;

		float zoom = Input.GetAxis ("Mouse ScrollWheel");
		this.transform.position += new Vector3 (0, 0, 1) * zoom;

		if (Input.GetKey ("1"))
			this.currentTile = Tile.TILE_CHECKER;
		if (Input.GetKey ("2"))
			this.currentTile = Tile.TILE_GRASS;
		if (Input.GetKey ("3"))
			this.currentTile = Tile.TILE_BRICK;

		if (Input.GetKey (KeyCode.Mouse0))
		{
			Camera camera = this.GetComponent<Camera>();
			Ray ray = camera.ScreenPointToRay(Input.mousePosition);
			this.world.PaintTiles(ray, this.currentTile);
		}
	}
}
