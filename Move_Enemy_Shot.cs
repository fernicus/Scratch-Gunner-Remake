/*
A very simple script to move the enemy's standard shot. It simply moves forward
until touching the arena walls, at which point it is destroyed.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Enemy_Shot : MonoBehaviour
{
	
	private CharacterController control;
	
	private Vector3 movement = new Vector3(0,0,1.0f);
	
    // Start is called before the first frame update
    void Start()
    {
        control = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
      	// If the shot has a nonzero y-position, it is changed to have one, since this is extremely important.
		if(transform.position.y != 0) {
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}

		// If this object is a clone of the original enemy shot, move it.
        if(gameObject.name == "Enemy Shot(Clone)") {
			control.Move(transform.TransformDirection(movement));
		}		
		
    }
	
	void OnTriggerEnter(Collider other) {
		// The shot is destroyed when touching the walls of the arena.
		string othername = other.gameObject.name.Substring(0,4);
		if (othername == "Cube") Destroy(gameObject);
	}
}
