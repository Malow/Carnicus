using UnityEngine;
using System;
using System.Collections.Generic;

public class WorldScript : MonoBehaviour {
	private List<List<Tile>> tiles = new List<List<Tile>>();
	private string mapName = "world.txt";
	private float tileZ = 0.1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void PaintTiles(Ray ray, int currentTile)
	{
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit))
		{
			GameObject objectHit = hit.transform.gameObject;
			foreach (List<Tile> row in this.tiles) {
				foreach(Tile tile in row)
				{
					if (objectHit == tile.obj)
					{
						Vector3 pos = tile.obj.transform.position;
						Destroy(tile.obj);
						tile.type = currentTile;
						
						GameObject templateTile = null;
						if(tile.type == Tile.TILE_CHECKER)
							templateTile = GameObject.FindGameObjectWithTag ("CheckerFloor");
						if(tile.type == Tile.TILE_GRASS)
							templateTile = GameObject.FindGameObjectWithTag ("GrassFloor");
						if(tile.type == Tile.TILE_BRICK)
							templateTile = GameObject.FindGameObjectWithTag ("BrickFloor");
						GameObject o = (GameObject)GameObject.Instantiate(templateTile, pos, new Quaternion (0, 0, 0, 0));
						tile.obj = o;
					}
				}
			}
		}
	}

	/* TODO: Expand Right -1 and it doesn't work and fucks up subsequent expands hard, gogo debug */ 
	public void ExpandMap(int startX, int startY, int endX, int endY)
	{
		List<List<Tile>> newTiles = new List<List<Tile>> ();
		int curX = this.tiles [0].Count;
		int curY = this.tiles.Count;
		
		for (int y = 0; y < startY + curY + endY; y++)
		{
			if(y < startY)
			{
				List<Tile> row = new List<Tile>();
				for (int x = 0; x < startX + curX + endX; x++)
				{
					GameObject templateTile = GameObject.FindGameObjectWithTag ("CheckerFloor");
					GameObject o = (GameObject)GameObject.Instantiate(templateTile, new Vector3 (x, y, this.tileZ), new Quaternion (0, 0, 0, 0));
					
					Tile tile = new Tile(Tile.TILE_CHECKER, o);
					row.Add(tile);
				}
				newTiles.Add(row);
			}
			else if(y < startY + curY)
			{
				List<Tile> row = new List<Tile>();
				for (int x = 0; x < startX + curX + endX; x++)
				{
					if(x < startX)
					{
						GameObject templateTile = GameObject.FindGameObjectWithTag ("CheckerFloor");
						GameObject o = (GameObject)GameObject.Instantiate(templateTile, new Vector3 (x, y, this.tileZ), new Quaternion (0, 0, 0, 0));
						
						Tile tile = new Tile(Tile.TILE_CHECKER, o);
						row.Add(tile);
					}
					else if(x < startX + curX)
					{
						Tile t = this.tiles[y - startY][x - startX];
						t.obj.transform.position = new Vector3(x, y, this.tileZ);
						row.Add(t);
					}
					else
					{
						GameObject templateTile = GameObject.FindGameObjectWithTag ("CheckerFloor");
						GameObject o = (GameObject)GameObject.Instantiate(templateTile, new Vector3 (x, y, this.tileZ), new Quaternion (0, 0, 0, 0));
						
						Tile tile = new Tile(Tile.TILE_CHECKER, o);
						row.Add(tile);
					}
				}
				newTiles.Add(row);
			}
			else
			{
				List<Tile> row = new List<Tile>();
				for (int x = 0; x < startX + curX + endX; x++)
				{
					GameObject templateTile = GameObject.FindGameObjectWithTag ("CheckerFloor");
					GameObject o = (GameObject)GameObject.Instantiate(templateTile, new Vector3 (x, y, this.tileZ), new Quaternion (0, 0, 0, 0));
					
					Tile tile = new Tile(Tile.TILE_CHECKER, o);
					row.Add(tile);
				}
				newTiles.Add(row);
			}
		}
		
		this.tiles = newTiles;
	}
	
	public void GenerateMap(int lengthX, int lengthY)
	{
		for (int y = 0; y < lengthY; y++)
		{
			List<Tile> row = new List<Tile>();
			for (int x = 0; x < lengthX; x++)
			{
				GameObject templateTile = GameObject.FindGameObjectWithTag ("CheckerFloor");
				GameObject o = (GameObject)GameObject.Instantiate(templateTile, new Vector3 (x, y, this.tileZ), new Quaternion (0, 0, 0, 0));
				
				Tile tile = new Tile(Tile.TILE_CHECKER, o);
				row.Add(tile);
			}
			this.tiles.Add(row);
		}
	}
	
	public void LoadMap()
	{
		foreach (List<Tile> row in this.tiles) {
			foreach(Tile tile in row)
			{
				Destroy(tile.obj);
			}
		}
		this.tiles = new List<List<Tile>>();
		
		
		string[] lines = System.IO.File.ReadAllLines(mapName);
		int y = 0;
		foreach (string line in lines)
		{
			List<Tile> row = new List<Tile>();
			int x = 0;
			foreach(char c in line)
			{
				int type = (int)c;
				GameObject templateTile = null;
				if(type == Tile.TILE_CHECKER)
					templateTile = GameObject.FindGameObjectWithTag ("CheckerFloor");
				if(type == Tile.TILE_GRASS)
					templateTile = GameObject.FindGameObjectWithTag ("GrassFloor");
				if(type == Tile.TILE_BRICK)
					templateTile = GameObject.FindGameObjectWithTag ("BrickFloor");
				
				GameObject o = (GameObject)GameObject.Instantiate(templateTile, new Vector3 (x, y, this.tileZ), new Quaternion (0, 0, 0, 0));
				
				Tile tile = new Tile(type, o);
				row.Add(tile);
				x++;
			}
			y++;
			this.tiles.Add(row);
		}
	}
	
	
	public void SaveMap()
	{
		using (System.IO.StreamWriter file = new System.IO.StreamWriter(mapName, false))
		{
			foreach (List<Tile> row in this.tiles) {
				string line = "";
				foreach(Tile tile in row)
				{
					line += (char)tile.type;
				}
				file.WriteLine(line);
			}
		}
		Debug.Log ("Saved Map");
	}
}
