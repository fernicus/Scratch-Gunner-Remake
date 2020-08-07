/*
A simple script to create the player's shots. Pressing the space bar will create a 
shot, and holding it down will allow the player to produce faster and more 
powerful shots!
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Create_Shot : MonoBehaviour {
	
	private GameObject clone;
	private Vector3 movement = new Vector3(0,0,10f);
	private GameObject Shooter;
	private CharacterController ShooterControl;

	
	// chargecount tracks how long a shot is charged for, in frames.
	public int chargecount = 0;
	
	// shotdelay is reset to 0 when a shot is fired, and will increment
	// every frame after this. A shot can only be fired when this value is 60.
	int shotdelay = 60;

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer>().material.SetColor("_Color",new Color(1f,0.3f,0.3f,1f));
		Shooter = GameObject.Find("Shooter");
		ShooterControl = Shooter.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		// There is a one-second delay between normal shots. If a new shot is not ready, then
		// increment the counter.
		if (shotdelay < 60) shotdelay++;
		
		// All this code is related to instantiating new shots, so it will only run for the original
		// Sphere object and not any of its clones.
		if(gameObject.name == "Sphere") {
			
			// If the space bar is pressed and the shot recharge time has elapsed, create a clone.
			if(Input.GetKeyDown("space") && shotdelay == 60){
				shotdelay = 0;
				clone = Instantiate(gameObject) as GameObject;
				clone.transform.position = Shooter.transform.position;
				clone.transform.rotation = Shooter.transform.rotation;
			}
			
			// After that, while the space bar is still being held, charge a stronger shot.
			if (Input.GetKey("space") && chargecount < 360) chargecount++;
				
			// Releasing space after at least 120 frames, but fewer than 360 frames, will produce
			// a charged shot. Doing this will reset the shot delay.
			if (chargecount >= 120 && chargecount < 360 && Input.GetKeyUp("space")) {
				clone = Instantiate(gameObject) as GameObject;
				clone.transform.position = Shooter.transform.position;
				clone.transform.rotation = Shooter.transform.rotation;			
				clone.name = "Charge Shot";
				clone.transform.localScale = new Vector3(20, 20, 20);
				shotdelay = 0;
			}
			
			// Releasing space after 360 or more frames produces a supercharged shot! This also
			// will reset the shot delay.
			if (chargecount == 360 && Input.GetKeyUp("space")) {
				clone = Instantiate(gameObject) as GameObject;
				clone.transform.position = Shooter.transform.position;
				clone.transform.rotation = Shooter.transform.rotation;			
				clone.name = "Super Charge Shot";
				clone.transform.localScale = new Vector3(25, 25, 40);
				shotdelay = 0;
			}
		}
		// Once the space bar is released, all charge is reset, whether a charged shot was
		// successfully fired or not.
		if (Input.GetKeyUp("space")) chargecount = 0;
	}
}
