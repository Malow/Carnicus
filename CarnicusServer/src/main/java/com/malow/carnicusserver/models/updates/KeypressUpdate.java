package com.malow.carnicusserver.models.updates;

import java.io.IOException;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.ModelInterface;


public class KeypressUpdate implements ModelInterface
{
    public final boolean up;
    public final boolean down;
    public final boolean left;
    public final boolean right;
    public final boolean shift;
    public final boolean attack;
    public final boolean attack2;
    public final boolean attack3;
    public final float diff;
	public final int inputSequenceNumber;
	public final float forwardX;
	public final float forwardY;
	public final float forwardZ;
    public final boolean noInput;

    @JsonCreator
    public KeypressUpdate(@JsonProperty("up") boolean up,
    				      @JsonProperty("down") boolean down, 
    				      @JsonProperty("left") boolean left, 
    				      @JsonProperty("right") boolean right, 
    				      @JsonProperty("shift") boolean shift, 
    				      @JsonProperty("attack") boolean attack, 
    				      @JsonProperty("attack2") boolean attack2, 
    				      @JsonProperty("attack3") boolean attack3, 
    				      @JsonProperty("diff") float diff,
    				      @JsonProperty("inputSequenceNumber") int inputSequenceNumber,
    				      @JsonProperty("forwardX") float forwardX,
    				      @JsonProperty("forwardY") float forwardY,
    				      @JsonProperty("forwardZ") float forwardZ, 
    				      @JsonProperty("noInput") boolean noInput)
    {
        this.up = up;
        this.down = down;
        this.left = left;
        this.right = right;
        this.shift = shift;
        this.attack = attack;
        this.attack2 = attack2;
        this.attack3 = attack3;
        this.diff = diff;
    	this.inputSequenceNumber = inputSequenceNumber;
    	this.forwardX = forwardX;
    	this.forwardY = forwardY;
    	this.forwardZ = forwardZ;
    	this.noInput = noInput;
    }

    @Override
    public String toNetworkString()
    {
        ObjectMapper mapper = new ObjectMapper();
        String str = "";
        try
        {
            str = mapper.writeValueAsString(this);
        } catch (IOException e)
        {
            e.printStackTrace();
        }
        return str;
    }
}