package com.malow.carnicusserver.models.updates;

import java.io.IOException;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.ModelInterface;

public class ActionUpdate implements ModelInterface
{
    public final int actionType;
    public final long creatorGUID;
    public final long victimGUID;
    public final long actionGUID;
    
    public final float posx;
    public final float posy;
    public final float posz;
    
    public final float dirx;
    public final float diry;
    public final float dirz;

    public final float damage;
    public final float blockReduction;

    @JsonCreator
    public ActionUpdate(@JsonProperty("actionType") int actionType,
    					@JsonProperty("creatorGUID") long creatorGUID,
    				    @JsonProperty("victimGUID") long victimGUID,
    				    @JsonProperty("actionGUID") long actionGUID,
    				    @JsonProperty("posx") float posx,
    				    @JsonProperty("posy") float posy,
    				    @JsonProperty("posz") float posz,
    				    @JsonProperty("dirx") float dirx,
    				    @JsonProperty("diry") float diry,
    				    @JsonProperty("dirz") float dirz,
    				    @JsonProperty("damage") float damage,
    				    @JsonProperty("blockReduction") float blockReduction)
    {
        this.actionType = actionType;
        this.creatorGUID = creatorGUID;
        this.victimGUID = victimGUID;
        this.actionGUID = actionGUID;
        this.posx = posx;
        this.posy = posy;
        this.posz = posz;
        this.dirx = dirx;
        this.diry = diry;
        this.dirz = dirz;
        this.damage = damage;
        this.blockReduction = blockReduction;
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