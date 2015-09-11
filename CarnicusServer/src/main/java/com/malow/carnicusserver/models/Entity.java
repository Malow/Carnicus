package com.malow.carnicusserver.models;

public class Entity
{
	public long GUID;
	public float posx;
	public float posy;
	public float posz;
	public float speed;
	public float forwardx;
	public float forwardy;
	public float forwardz;
	public int lastProcessedInput;
	public float blockLevel;

	public Entity (long GUID, float posx, float posy, float posz, float speed,
			float forwardx, float forwardy, float forwardz, int lastProcessedInput, float blockLevel)
	{
		this.GUID = GUID;
		this.posx = posx;
		this.posy = posy;
		this.posz = posz;
		this.speed = speed;
		this.forwardx = forwardx;
		this.forwardy = forwardy;
		this.forwardz = forwardz;
		this.lastProcessedInput = lastProcessedInput;
		this.blockLevel = blockLevel;
	}
}