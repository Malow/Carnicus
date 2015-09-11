package com.malow.carnicusserver.models.updates;

import java.io.IOException;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.ModelInterface;

public class WorldUpdate implements ModelInterface
{
    public final String dataString;
    public final float tileZ;
    public final int sequenceNumber;
    public final int totalSequencePackets;

    @JsonCreator
    public WorldUpdate(@JsonProperty("dataString") String dataString, 
			   		   @JsonProperty("tileZ") float tileZ, 
    				   @JsonProperty("sequenceNumber") int sequenceNumber, 
    				   @JsonProperty("totalSequencePackets") int totalSequencePackets)
    {
        this.dataString = dataString;
        this.tileZ = tileZ;
        this.sequenceNumber = sequenceNumber;
        this.totalSequencePackets = totalSequencePackets;
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