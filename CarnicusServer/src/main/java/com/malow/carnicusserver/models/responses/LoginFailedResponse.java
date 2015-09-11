package com.malow.carnicusserver.models.responses;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.IOException;

import com.malow.carnicusserver.models.ModelInterface;

public class LoginFailedResponse implements ModelInterface
{
    public final String errorMsg;

    @JsonCreator
    public LoginFailedResponse(@JsonProperty("errorMsg") String errorMsg)
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