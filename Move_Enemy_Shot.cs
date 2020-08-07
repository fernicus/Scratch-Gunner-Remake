/*
A very simple script to move the enemy's standard shot. It simply moves forward
until touching the arena walls, at which point it is destroyed. As the enemy's HP
gets lower, the shot travels faster.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Enemy_Shot : MonoBehaviour
{
	
	private GameObject Enemy;
	
	private CharacterController control;
	
	private Vector3 movement;
	
    // Start is called before the first frame update
    void Start()
    {
		Enemy = GameObject.Find("Boss");
        control = gameObject.GetComponent<CharacterController>();
		movement = new Vector3 (0.0f,0.0f, (float)(5-0.125*Enemy.GetComponent<Enemy_Movement>().HP));
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
