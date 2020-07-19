/*
A very simple script to move the camera around. It will always track the 
player's location.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
	public static Vector3 movement;
	public GameObject Shooter;
    // Start is called before the first frame update
    void Start()
    {
		movement = new Vector3(0.0f,400f,0.0f);
		Shooter = GameObject.Find("Shooter");
    }

    // Update is called once per frame
    void Update()
    {
		// The camera's xz position always matches the player's.
		movement.x = Shooter.transform.position.x;
		movement.z = Shooter.transform.position.z;
		transform.position = movement;
    }
}
