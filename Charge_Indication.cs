/*
A simple script to manage the charge indicator for the player. While
no charged shot is available, the indicator is invisible. When a partially
charged shot is ready, the indicator appears. When the shot is fully charged,
the indicator grows larger.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge_Indication: MonoBehaviour
{
	MeshRenderer rend;
	
	Create_Shot script;
	
	GameObject Shooter;
	
	Vector3 small = new Vector3(0.3f,0.3f,0.3f);
	
	Vector3 large = new Vector3(0.6f,0.6f,0.6f);
	
	int angle = 0;
	
	// Start is called before the first frame update
	void Start() 
	{
		Shooter = GameObject.Find("Shooter");
		
		script = GameObject.Find("Sphere").GetComponent<Create_Shot>();
		
		rend = GetComponentInChildren<MeshRenderer>();
		
		rend.material.SetColor("_Color",Color.yellow);
		
		rend.enabled = false;
		
	}
	
	// Update is called once per frame
	void Update() 
	{
		
		// Disable this function if the player is not charging any shot.
		if (script.chargecount >= 120){
			// Turn the charge indicator, and make sure it is always on top of 
			// the player.
			angle += 2;
			if (angle >= 360) angle = 0;
			transform.rotation = Quaternion.Euler(new Vector3(0,angle,0));
			transform.position = Shooter.transform.position;
			
			rend.enabled = true;
			
			if (script.chargecount >= 120 && script.chargecount < 360) transform.localScale = small;
			
			else if (script.chargecount == 360) transform.localScale = large;
			
		}
		
		// If the player is not charging, disable the indicator.
		else rend.enabled = false;
		
	}
	
	
}