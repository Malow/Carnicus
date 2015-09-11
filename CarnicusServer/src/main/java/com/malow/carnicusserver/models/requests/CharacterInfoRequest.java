package com.malow.carnicusserver.models.requests;


import com.fasterxml.jackson.annotation.JsonCreator;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.ModelInterface;

import java.io.IOException;


public class CharacterInfoRequest implements ModelInterface
{
    @JsonCreator
    public CharacterInfoRequest()
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
