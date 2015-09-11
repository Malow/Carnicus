using UnityEngine;
using System.Collections.Generic;
using System.Threading;

public class WorldScript : MonoBehaviour {

	private ServerConnection server = null;

	private List<OtherPlayersScript> otherPlayers = new List<OtherPlayersScript> ();
	public List<ProjectileScript> arrows = new List<ProjectileScript> ();

	public World world = new World();

	public GUIScript gui;

	public PlayerScript player;

	public Vector3 lastForward = new Vector3();

	public bool startSendingInputs = false;
	public bool hasSentNoInput = false;
	public bool isApplicationFocus = true;

	void Start () {
		this.server = new ServerConnection ("malow.mooo.com", 7000);
		this.server.SendMessage(new LoginRequest("a", "a"));

		// Create player
		GameObject templatePlayer = GameObject.FindGameObjectWithTag ("PlayerTemplate");
		GameObject g = (GameObject)Instantiate(templatePlayer, new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0));
		g.AddComponent<PlayerScript>();
		this.player = g.GetComponent<PlayerScript>();
		this.player.animationScript = g.GetComponent<CharacterAnimationScript>();
		this.gui.player = this.player;
	}

	public float counter = 0.0f;

	void Update () {
		ModelInterface msg = server.GetMessage ();
		if (msg != null) {
			this.HandleServerMessage(msg);
		}
		
		if (this.startSendingInputs)
			this.ProcessInputs ();
	}

	private void HandlePositionUpdate(PositionUpdate pu)
	{
		List<Entity> entities = pu.entities;
		foreach(Entity entity in entities)
		{
			if(entity.GUID == this.player.GUID)
			{
				this.player.ProcessServerMessage(entity);
			}
			else
			{
				this.ProcessOtherPlayersPositions(entity);
			}
		}
	}

	private void HandleActionUpdate(ActionUpdate au)
	{
		if(au.actionType == Action.PROJECTILE_START)
		{
			Vector3 arrowDir = new Vector3(au.dirx, au.diry, au.dirz);
			GameObject templateArrow = GameObject.FindGameObjectWithTag ("Arrow");
			GameObject g = (GameObject)Instantiate(templateArrow, new Vector3(au.posx, au.posy, au.posz), new Quaternion (0, 0, 0, 0));
			ProjectileScript s = g.GetComponent<ProjectileScript>();
			s.orig = g.transform.position;
			s.direction = arrowDir;
			s.speed = 10.0f;
			s.GUID = au.actionGUID;
			this.arrows.Add(s);
			
			float AngleRad = Mathf.Atan2 (arrowDir.x, -arrowDir.y);
			float AngleDeg = (180 / Mathf.PI) * AngleRad;
			g.transform.rotation = Quaternion.Euler (0, 0, AngleDeg);				
			return;
		}
		
		if(au.creatorGUID == this.player.GUID || au.victimGUID == this.player.GUID)
			this.player.ProcessAction(au);
		
		foreach(OtherPlayersScript p in this.otherPlayers)
		{
			if(p.GUID == au.creatorGUID || p.GUID == au.victimGUID)
				p.ProcessAction(au);
		}
		
		if(au.actionType == Action.PROJECTILE_END)
		{
			ProjectileScript arrow = null;
			foreach(ProjectileScript s in this.arrows)
			{
				if(s.GUID == au.actionGUID)
					arrow = s;
			}
			if(arrow != null)
			{
				this.arrows.Remove(arrow);
				Destroy(arrow.gameObject);
				Destroy(arrow);
			}
			else
			{
				Debug.LogError("Error: Couldn't find arrow to delete");
			}
		}
	}
	
	private void HandleServerMessage(ModelInterface msg)
	{
		PositionUpdate pu = msg as PositionUpdate;
		if(pu != null)
		{
			this.HandlePositionUpdate(pu);
			return;
		}

		ActionUpdate au = msg as ActionUpdate;
		if(au != null)
		{
			this.HandleActionUpdate(au);
			return;
		}

		ServerSettingsUpdate ssu = msg as ServerSettingsUpdate;
		if(ssu != null)
		{
			Debug.Log ("Server Settings Update, ServerFPS: " + ssu.serverFps);
			Constants.SERVER_FPS = ssu.serverFps;
			return;
		}

		WorldUpdate wu = msg as WorldUpdate;
		if(wu != null)
		{
			this.world.HandleWorldUpdate(wu);
			return;
		}

		LoginSuccessfulResponse lsr = msg as LoginSuccessfulResponse;
		if(lsr != null)
		{
			Debug.Log ("Login Successfull!");
			this.server.SendMessage(new CharacterInfoRequest());
			return;
		}

		LoginFailedResponse lfr = msg as LoginFailedResponse;
		if(lfr != null)
		{
			Debug.Log ("Login Failed: " + lfr.errorMsg);
			return;
		}

		UnexpectedRequestResponse urr = msg as UnexpectedRequestResponse;
		if(urr != null)
		{
			Debug.Log ("UnexpectedRequestResponse: " + urr.request);
			return;
		}

		NoCharacterFoundResponse ncfr = msg as NoCharacterFoundResponse;
		if(ncfr != null)
		{
			Debug.Log ("NoCharacterFound");
			return;
		}

		CharacterInfoResponse cir = msg as CharacterInfoResponse;
		if(cir != null)
		{
			Debug.Log ("Character Info received for: " + cir.ToNetworkString());
			this.player.transform.position = new Vector3(cir.posx, cir.posy, cir.posz);
			this.player.GUID = cir.GUID;
			this.startSendingInputs = true;
			return;
		}
	}

	private void ProcessInputs()
	{
		Vector3 forward = new Vector3 (0, 0, 0);
		if (this.isApplicationFocus) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			Vector3 point = ray.GetPoint (ray.origin.z / -ray.direction.z);
			forward = point - this.player.transform.position;
			forward.Normalize ();
		} else {
			forward = this.lastForward;
		}

		KeypressUpdate ku = new KeypressUpdate (Input.GetKey ("w"), 
		                                        Input.GetKey ("s"), 
		                                        Input.GetKey ("a"), 
		                                        Input.GetKey ("d"), 
		                                        Input.GetKey (KeyCode.LeftShift),
		                                        Input.GetKey (KeyCode.Mouse0),
		                                        Input.GetKey (KeyCode.Mouse1),
		                                        Input.GetKey (KeyCode.Mouse2),
		                                        Time.deltaTime,
		                                        this.player.inputSequenceNumber,
		                                        forward.x,
		                                        forward.y,
		                                        forward.z, 
		                                        false);

		if (!ku.up && !ku.down && !ku.left && !ku.right && !ku.shift && !ku.attack && !ku.attack2 && !ku.attack3 && forward == this.lastForward) 
		{
			ku.noInput = true;
			if(!this.hasSentNoInput)
			{
				this.hasSentNoInput = true;
				this.lastForward = forward;
				this.server.SendMessage (ku);
				
				this.player.inputSequenceNumber++;
				this.player.ApplyInput (ku, true);
				this.player.pendingInputs.Add(ku);
			}
			return;
		}
		this.hasSentNoInput = false;
		this.lastForward = forward;
		this.server.SendMessage (ku);
		
		this.player.inputSequenceNumber++;
		this.player.ApplyInput (ku, true);
		this.player.pendingInputs.Add(ku);
	}

	private void ProcessOtherPlayersPositions(Entity entity)
	{
		bool found = false;
		foreach(OtherPlayersScript p in this.otherPlayers)
		{
			if(entity.GUID == p.GUID)
			{
				found = true;
				p.SetNewTargetPos(new Vector3(entity.posx, entity.posy, entity.posz), entity.speed, new Vector3(entity.forwardx, entity.forwardy, entity.forwardz), entity.blockLevel);
			}
		}
		if(!found)
		{
			GameObject templatePlayer = GameObject.FindGameObjectWithTag ("PlayerTemplate");
			GameObject g = (GameObject)Instantiate(templatePlayer, new Vector3 (entity.posx, entity.posy, entity.posz), new Quaternion (0, 0, 0, 0));
			g.AddComponent<OtherPlayersScript>();
			OtherPlayersScript p = g.GetComponent<OtherPlayersScript>();
			p.animationScript = g.GetComponent<CharacterAnimationScript>();
			p.GUID = entity.GUID;
			p.speed = entity.speed;
			p.transform.position = new Vector3(entity.posx, entity.posy, entity.posz);
			this.otherPlayers.Add(p);
		}
	}





	
	void OnApplicationFocus(bool focusStatus) {
		this.isApplicationFocus = focusStatus;
	}
	
	void OnApplicationQuit()
	{
		this.server.Quit();
	}
}
