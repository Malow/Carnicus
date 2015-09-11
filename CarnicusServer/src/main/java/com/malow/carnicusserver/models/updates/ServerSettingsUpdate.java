package com.malow.carnicusserver.models.updates;

import java.io.IOException;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.ModelInterface;

public class ServerSettingsUpdate implements ModelInterface
{
    public final float serverFps;

    @JsonCreator
    public ServerSettingsUpdate(@JsonProperty("serverFps") float serverFps)
    {
        this.serverFps = serverFps;
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