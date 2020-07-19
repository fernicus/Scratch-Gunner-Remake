/*
A simple script to move the projectiles summoned in Attack 1.
Upon colliding with something that is not itself or the enemy, the
projectile is destroyed.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_1 : MonoBehaviour
{
	
	private CharacterController control;
	private Vector3 movement = new Vector3(0.0f, 1.0f, 0.0f);
	
	
    // Start is called before the first frame update
    void Start()
    {
        control = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
		if (gameObject.name != "Attack 1") control.Move(transform.TransformDirection(movement));
    }
	
	// The projectile is destroyed if it touches a collider other than its own or the enemy's.
	void OnTriggerEnter(Collider other) {
		
		if (other.gameObject.name != "Boss" && other.gameObject.name != "Attack 1" && other.gameObject.name != "Attack 1(Clone)") Destroy(gameObject);
	
	}
}
