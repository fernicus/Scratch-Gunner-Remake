using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Create_Shot : MonoBehaviour {
	
	private GameObject clone;
	private Vector3 movement = new Vector3(0,0,10f);
	private GameObject Shooter;
	private CharacterController ShooterControl;
	public static int chargecount = 0;
	int shotdelay = 60;

	// Use this for initialization
	void Start () {
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
			if (Input.GetKey("space") && chargecount < 240) chargecount++;
				
			// Releasing space after at least 120 frames, but fewer than 240 frames, will produce
			// a charged shot. Doing this will reset the shot delay.
			if (chargecount >= 120 && chargecount < 240 && Input.GetKeyUp("space")) {
				clone = Instantiate(gameObject) as GameObject;
				clone.transform.position = Shooter.transform.position;
				clone.transform.rotation = Shooter.transform.rotation;			
				clone.name = "Charge Shot";
				clone.transform.localScale = new Vector3(10, 10, 10);
				shotdelay = 0;
			}
			
			// Releasing space after 240 or more frames produces a supercharged shot! This also
			// will reset the shot delay.
			if (chargecount == 240 && Input.GetKeyUp("space")) {
				clone = Instantiate(gameObject) as GameObject;
				clone.transform.position = Shooter.transform.position;
				clone.transform.rotation = Shooter.transform.rotation;			
				clone.name = "Super Charge Shot";
				clone.transform.localScale = new Vector3(20, 20, 20);
				shotdelay = 0;
			}
		}
		// Once the space bar is released, all charge is reset, whether a charged shot was
		// successfully fired or not.
		if (Input.GetKeyUp("space")) chargecount = 0;
	}
}
