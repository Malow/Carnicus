using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIExpandableScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void HideAllExpandables()
	{
		GUIExpandableScript[] expandables = (GUIExpandableScript[])FindObjectsOfType (typeof(GUIExpandableScript));
		foreach(GUIExpandableScript expandable in expandables)
		{
			//if(expandable.gameObject)
				expandable.gameObject.SetActive(false);

			InputField f = expandable.GetComponent<InputField>();
			if(f)
			{
				f.text = string.Empty;
			}
		}
	}
}
