/*
A short and simple script to move the player character around using the WASD keys and its 
attached CharacterController.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	// speed dictates how speedily the player can move.
    public static float speed = 2.0f;
	public Vector3 movement;
	private CharacterController control;

	void Start () {
		movement = new Vector3(0.0f,0.0f,0.0f);
		control = gameObject.GetComponent<CharacterController>();
	}
	

    void Update()
    {
		// If the player object does not have a y-position of 0, it is changed to have one, since this is extremely important.
		if(transform.position.y != 0) {
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
		
		// The player can actually phase through the collision of the boundaries by spinning around rapidly enough ://///
		// Until that issue is fixed (or if it cannot be fixed), these position constraints will suffice.
		
		if (transform.position.x < -440) transform.position = new Vector3(-440, 0, transform.position.z);
		else if (transform.position.x > 440) transform.position = new Vector3(440, 0, transform.position.z);

		if (transform.position.z < -440) transform.position = new Vector3(transform.position.x, 0, -440);
		else if (transform.position.z > 440) transform.position = new Vector3(transform.position.x, 0, 440);
		
		// If a shot is being charged, the player's speed is reduced while charging.
		if (Create_Shot.chargecount >= 20) speed = 1.0f;
		else speed = 2.0f;
		
		// When the WASD keys are not pressed, moveHorizontal and moveVertical are 0, so the movement vector will not be modified.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		movement.x = speed * moveHorizontal;
		movement.z = speed * moveVertical;
		// The player's CharacterController is updated.
		control.Move(movement);
	}
}
