package com.malow.carnicusserver.models.responses;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.ModelInterface;

import java.io.IOException;

public class CharacterInfoResponse implements ModelInterface
{
    public final String name;
    public final float posx;
    public final float posy;
    public final float posz;
    public final long GUID;

    @JsonCreator
    public CharacterInfoResponse(@JsonProperty("name") String name,
    							 @JsonProperty("posx") float posx,
    							 @JsonProperty("posy") float posy,
    							 @JsonProperty("posz") float posz,
    							 @JsonProperty("GUID") long GUID)
    {
        this.name = name;
        this.posx = posx;
        this.posy = posy;
        this.posz = posz;
        this.GUID = GUID;
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
