using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
	public static Vector3 movement;
	
    // Start is called before the first frame update
    void Start()
    {
        movement = new Vector3(0.0f,200f,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
		// When the WASD keys are not pressed, moveHorizontal and moveVertical are 0, so the movement vector will not be modified.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		movement.x += Movement.speed * moveHorizontal;
		movement.z += Movement.speed * moveVertical;
		transform.position = movement;
    }
}
