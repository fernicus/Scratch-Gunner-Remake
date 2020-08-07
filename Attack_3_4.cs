/*
A simple script to move the projectiles summoned in Attacks 3 and 4.
Upon colliding with the arena walls, the projectile is destroyed. 
The speed of the projectiles also increases as the enemy loses more 
and more HP.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Attack_3_4 : MonoBehaviour
{
	
	private CharacterController control;
	private Vector3 movement;
	
	private GameObject Enemy;
	
	private float speed;
	
    // Start is called before the first frame update
    void Start()
    {
		Enemy = GameObject.Find("Boss");
		speed = (float)Math.Exp(0.05*(double)(20-Enemy.GetComponent<Enemy_Movement>().HP));
		movement = new Vector3(0.0f, speed, 0.0f);
        control = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
		if (gameObject.name == "Attack 3/4(Clone)") control.Move(transform.TransformDirection(movement));
    }
	
	// The projectile is destroyed if it touches the walls of the arena.
	void OnTriggerEnter(Collider other) {
		string othername = other.gameObject.name.Substring(0,4);
		if (othername == "Cube") Destroy(gameObject);
	}
}
