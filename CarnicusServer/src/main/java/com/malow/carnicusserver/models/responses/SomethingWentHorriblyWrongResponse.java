package com.malow.carnicusserver.models.responses;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.ModelInterface;

import java.io.IOException;

public class SomethingWentHorriblyWrongResponse implements ModelInterface
{
    public final String errorMsg;

    @JsonCreator
    public SomethingWentHorriblyWrongResponse(@JsonProperty("errorMsg") String errorMsg)
    {
        this.errorMsg = errorMsg;
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