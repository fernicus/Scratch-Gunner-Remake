/*
A simple script to manage the projectiles summoned in Attack 6.
These projectiles move extremely quickly, and upon colliding with the arena
walls, they are destroyed.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_6 : MonoBehaviour
{
	private CharacterController control;
	private Vector3 movement = new Vector3(15.0f, 0.0f, 0.0f);
	
    // Start is called before the first frame update
    void Start()
    {
        control = gameObject.GetComponent<CharacterController>();       
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name != "Attack 3/4") control.Move(transform.TransformDirection(movement));
    }
	
	void OnTriggerEnter(Collider other) {
		string othername = other.gameObject.name.Substring(0,4);
		if (othername == "Cube") Destroy(gameObject);
		
	}
	
}
