package com.malow.carnicusserver.world;

public class Tile
{
	public static int TILE_CHECKER = 36;
	public static int TILE_GRASS = 37;
	public static int TILE_BRICK = 38;

	public int type;
	
	public Tile(int type)
	{
		this.type = type;
	}
}
