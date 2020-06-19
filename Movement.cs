using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public static float speed;
	public Vector3 movement;
	public Rigidbody rigidity;

	void Start () {
		// speed will likely be modified in the future for balancing purposes, and may even be changed mid-game.
		speed = 5.0f;
		movement = new Vector3(0.0f,0.0f,10.0f);
		rigidity = GetComponent<Rigidbody>();
	}
	

    void Update()
    {
		// When the WASD keys are not pressed, moveHorizontal and moveVertical are 0, so the movement vector will not be modified.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		movement.x = speed * moveHorizontal;
		movement.z = speed * moveVertical;
		Debug.Log(movement);
		// The player's rigidbody is updated.
		if (movement.x == 0 && movement.z == 0){
			rigidity.velocity = new Vector3(0,0,0);
		}
			
		else {
			
			rigidity.AddForce(movement);
		}
	}
}
