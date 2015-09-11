package com.malow.carnicusserver.world;

import java.io.BufferedReader;
import java.io.FileReader;
import java.io.PrintWriter;
import java.util.ArrayList;

public class World {
	public static int MAX_DATASTRING_LENGTH = 300;
	public static final float TILE_Z = 0.1f;
	public ArrayList<ArrayList<Tile>> tiles = new ArrayList<ArrayList<Tile>>();
	public ArrayList<String> dataStrings;
	
	public void Save()
	{
		try
		{
			PrintWriter writer = new PrintWriter("world.txt", "UTF-8");
			for(ArrayList<Tile> row : this.tiles)
			{
				String line = "";
				for(Tile tile : row)
				{
					line += (char)tile.type;
				}
				writer.println(line);
			}
			writer.close();
		}
		catch (Exception e) {
			System.out.println("FAILED TO SAVE WORLD TO FILE!");
			e.printStackTrace();
		}
	}
	
	public void Load()
	{
		try
		{
			BufferedReader br = new BufferedReader(new FileReader("world.txt"));
			String line = br.readLine();
			while(line != null && line != "")
			{
				ArrayList<Tile> row = new ArrayList<Tile>();
				for(char c : line.toCharArray())
				{
					int type = (int)c;
					row.add(new Tile(type));
				}
				this.tiles.add(row);
	            line = br.readLine();
			}
			br.close();
	    } catch (Exception e) {
			System.out.println("FAILED TO LOAD WORLD FROM FILE!");
			e.printStackTrace();
		}
		
		this.CreateWorldUpdateStrings();
	}
	
	private void CreateWorldUpdateStrings()
	{
		ArrayList<String> ss = new ArrayList<String>();

		String totalString = "";
		
		for(ArrayList<Tile> row : this.tiles)
		{
			for(Tile tile : row)
			{
				totalString += (char)tile.type;
			}
			totalString += (char)35;
		}
				
		while(totalString.length() > MAX_DATASTRING_LENGTH)
		{
			String s = totalString.substring(0, MAX_DATASTRING_LENGTH);
			ss.add(s);
			totalString = totalString.substring(MAX_DATASTRING_LENGTH);
		}
		
		ss.add(totalString);
		
		this.dataStrings = ss;		
	}
	
	public ArrayList<String> GetWorldUpdateStrings()
	{
		return this.dataStrings;
	}
}
