/*
A simple script to manage the projectiles summoned in Attack 7.
The projectiles slowly oscillate as they move, leaving a damaging
trail behind them, and are destroyed upon touching the arena walls.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_7 : MonoBehaviour
{
	
	private CharacterController control;
	private Vector3 movement;
	
	private GameObject clone;
	
	Collider col;
	
	//MeshRenderer rend;
	
	Vector3 large = new Vector3(30,30,30);
	Vector3 medium = new Vector3(20,20,20);
	Vector3 small = new Vector3(0,0,0);
	
	int k;
	float offset;
	
    // Start is called before the first frame update
    void Start()
    {
		GetComponent<MeshRenderer>().material.SetColor("_Color",Color.cyan);
		
		col = GetComponent<Collider>();
		
		col.enabled = true;
		
		// k will range from 0 to 359, which will represent a full period of oscillation.
		k = 0;
        control = GetComponent<CharacterController>();
		
		// If the object is a clone OF a clone, it is not the main projectile but the trail
		// left behind. Instead of running Update(), it will run the trail coroutine.
		if (name == "Attack 7(Clone)(Clone)") {
			StartCoroutine(Trails());
		}
    }

    // Update is called once per frame
    void Update()
    {
        if (name == "Attack 7(Clone)") {
			offset = Mathf.Cos(k * Mathf.PI / 180);
			movement = new Vector3(0.5f, 0.0f,2*offset);
			control.Move(transform.TransformDirection(movement));
		
			// If k is divisible by 10, create a trail particle.
			if(k%10 == 0) clone = Instantiate(gameObject) as GameObject;
			
			// Once k reaches 360, set it back to 0 to start a new period.
			k++;
			if (k >= 360) k = 0;
			
		}
		
		
    }
	
	IEnumerator Trails() {
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Shrink the trail particle down to nothing over 0.7 seconds.
		do{
			transform.localScale = Vector3.Lerp(large,small,currenttime/0.7f);
			currenttime += Time.deltaTime;
			
			// Once the trail particle is smaller than a certain size, disable the
			// collider so that the player doesn't take damage from objects they 
			// can barely see.
			if (transform.localScale.x <= 1) col.enabled = false;
			
			yield return null;
			
		}while(currenttime <= 0.7f);
		
		// Destroy the trail.
		Destroy(gameObject);
		
	}
	
	void OnTriggerEnter(Collider other) {
		string othername = other.gameObject.name.Substring(0,4);
		if (othername == "Cube") Destroy(gameObject);	
	}
}
