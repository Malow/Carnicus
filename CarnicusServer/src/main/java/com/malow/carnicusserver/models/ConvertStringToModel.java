package com.malow.carnicusserver.models;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.malow.carnicusserver.models.requests.CharacterInfoRequest;
import com.malow.carnicusserver.models.requests.LoginRequest;
import com.malow.carnicusserver.models.responses.CharacterInfoResponse;
import com.malow.carnicusserver.models.responses.LoginFailedResponse;
import com.malow.carnicusserver.models.responses.LoginSuccessfulResponse;
import com.malow.carnicusserver.models.responses.NoCharacterFoundResponse;
import com.malow.carnicusserver.models.responses.SomethingWentHorriblyWrongResponse;
import com.malow.carnicusserver.models.responses.UnexpectedRequestResponse;
import com.malow.carnicusserver.models.updates.ActionUpdate;
import com.malow.carnicusserver.models.updates.KeypressUpdate;
import com.malow.carnicusserver.models.updates.PositionUpdate;
import com.malow.carnicusserver.models.updates.ServerSettingsUpdate;
import com.malow.carnicusserver.models.updates.WorldUpdate;


public class ConvertStringToModel
{
    public static ModelInterface toModel(String networkString)
    {
        ObjectMapper mapper = new ObjectMapper();
        

        try { return mapper.readValue(networkString, PositionUpdate.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, KeypressUpdate.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, ActionUpdate.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, ServerSettingsUpdate.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, WorldUpdate.class); } catch (Exception e) {}
        
        try { return mapper.readValue(networkString, LoginRequest.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, CharacterInfoRequest.class); } catch (Exception e) {}

        try { return mapper.readValue(networkString, CharacterInfoResponse.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, LoginFailedResponse.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, LoginSuccessfulResponse.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, UnexpectedRequestResponse.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, NoCharacterFoundResponse.class); } catch (Exception e) {}
        try { return mapper.readValue(networkString, SomethingWentHorriblyWrongResponse.class); } catch (Exception e) {}

        //throw new RuntimeException("Malow, you've forgot to add a new request/response to the converter... " + networkString);

        return null;
    }
}
