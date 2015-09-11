package com.malow.carnicusserver.models.responses;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.databind.ObjectMapper;

import java.io.IOException;

import com.malow.carnicusserver.models.ModelInterface;

public class LoginSuccessfulResponse implements ModelInterface
{
    @JsonCreator
    public LoginSuccessfulResponse()
    {
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