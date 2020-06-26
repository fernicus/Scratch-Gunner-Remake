using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public static float speed = 2.0f;
	public Vector3 movement;
	private CharacterController control;

	void Start () {
		// speed will likely be modified in the future for balancing purposes, and may even be changed mid-game.
		movement = new Vector3(0.0f,0.0f,0.0f);
		control = gameObject.GetComponent<CharacterController>();
	}
	

    void Update()
    {
		// If the player object does not have a y-position of 0, it is changed to have one, since this is extremely important.
		if(transform.position.x != 0) {
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
		
		// When the WASD keys are not pressed, moveHorizontal and moveVertical are 0, so the movement vector will not be modified.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		movement.x = speed * moveHorizontal;
		movement.z = speed * moveVertical;
		// The player's CharacterController is updated.
		control.Move(movement);
	}
}
