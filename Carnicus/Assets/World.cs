
using UnityEngine;
using System;
using System.Collections.Generic;

public class World
{
	public List<List<Tile>> tiles = null;
	public float tileZ = 0.1f;

	public List<WorldUpdate> updates = new List<WorldUpdate>();

	public World ()
	{
	}

	public void HandleWorldUpdate(WorldUpdate wu)
	{
		this.tileZ = wu.tileZ;
		this.updates.Add (wu);
		if (this.updates.Count == wu.totalSequencePackets)
			this.GenerateWorldFromDataStrings (wu.totalSequencePackets);
	}

	private void GenerateWorldFromDataStrings(int totalPackets)
	{
		String totString = "";

		for (int i = 0; i < totalPackets; i++) {
			foreach(WorldUpdate update in this.updates)
			{
				if(update.sequenceNumber == i)
					totString += update.dataString;
			}
		}

		if (this.tiles == null) {
			this.tiles = new List<List<Tile>>();
			float posx = 0.0f;
			float posy = 0.0f;

			List<Tile> wRow = new List<Tile>();
			foreach(char tile in totString)
			{
				if(tile == (char)35)
				{
					posy += 1.0f;
					posx = 0.0f;
					this.tiles.Add(wRow);
					wRow = new List<Tile>();
				}
				else {
					GameObject templateTile = null;
					int nTile = (char)tile;
					if(nTile == Tile.TILE_CHECKER)
						templateTile = GameObject.FindGameObjectWithTag ("CheckerFloor");
					else if(nTile == Tile.TILE_GRASS)
						templateTile = GameObject.FindGameObjectWithTag ("GrassFloor");
					else if(nTile == Tile.TILE_BRICK)
						templateTile = GameObject.FindGameObjectWithTag ("BrickFloor");
					else
						Debug.Log("Critical error in World, couldn't find tile: " + nTile);
					
					GameObject o = (GameObject)GameObject.Instantiate(templateTile, new Vector3 (posx, posy, this.tileZ), new Quaternion (0, 0, 0, 0));
					posx += 1.0f;
					
					Tile wTile = new Tile(nTile, o);
					wRow.Add(wTile);
				}
			}

		} else {
			Debug.Log("Critical error in World, world already exists!");
		}
		Debug.Log ("World Received and Genereted with: " + totalPackets + " packets.");
	}
}


