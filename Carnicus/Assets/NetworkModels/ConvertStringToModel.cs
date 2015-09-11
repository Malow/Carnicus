using UnityEngine;
using System;


public class ConvertStringToModel
{
	public static ModelInterface ToModel(String networkString)
	{
		ModelInterface mi = null;

		
		mi = KeypressUpdate.ToModel (networkString);
		if (mi != null)
			return mi;

		mi = PositionUpdate.ToModel (networkString);
		if (mi != null)
			return mi;		

		mi = ActionUpdate.ToModel (networkString);
		if (mi != null)
			return mi;

		mi = ServerSettingsUpdate.ToModel (networkString);
		if (mi != null)
			return mi;
		
		mi = WorldUpdate.ToModel (networkString);
		if (mi != null)
			return mi;



		mi = LoginRequest.ToModel (networkString);
		if (mi != null)
			return mi;

		mi = CharacterInfoRequest.ToModel (networkString);
		if (mi != null)
			return mi;




		mi = LoginSuccessfulResponse.ToModel (networkString);
		if (mi != null)
			return mi;

		mi = LoginFailedResponse.ToModel (networkString);
		if (mi != null)
			return mi;

		mi = UnexpectedRequestResponse.ToModel (networkString);
		if (mi != null)
			return mi;

		mi = CharacterInfoResponse.ToModel (networkString);
		if (mi != null)
			return mi;

		mi = NoCharacterFoundResponse.ToModel (networkString);
		if (mi != null)
			return mi;

		mi = SomethingWentHorriblyWrongResponse.ToModel (networkString);
		if (mi != null)
			return mi;

		if (mi == null)
			Debug.Log ("FAILED! FAILED! FAILED! to add new network model: " + networkString);

		return mi;
	}
}


