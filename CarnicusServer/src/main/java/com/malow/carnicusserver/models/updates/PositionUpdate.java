package com.malow.carnicusserver.models.updates;

import java.io.IOException;
import java.util.ArrayList;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.Entity;
import com.malow.carnicusserver.models.ModelInterface;

public class PositionUpdate implements ModelInterface
{
    public final ArrayList<Entity> entities;

    @JsonCreator
    public PositionUpdate(@JsonProperty("entities") ArrayList<Entity> entities)
    {
        this.entities = entities;
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