package com.malow.carnicusserver.models.responses;

import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.annotation.JsonProperty;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.ModelInterface;

import java.io.IOException;

public class UnexpectedRequestResponse implements ModelInterface
{
	public final String request;
	
    @JsonCreator
    public UnexpectedRequestResponse(@JsonProperty("request") String request)
    {
    	this.request = request;
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