/*
A script to manage the player's shots once they are created. More powerful shots
will travel faster. Once they touch a collider that is not the player, they are
destroyed, and if the collider they touched was the enemy, it will decrease the
enemy's HP by an appropriate amount.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Shot : MonoBehaviour
{
	
	private CharacterController control;
	private Vector3 movement = new Vector3(0,0,-1.0f);
	
	GameObject Boss;
	Enemy_Movement script;
	
    // Start is called before the first frame update
    void Start()
    {
		control = gameObject.GetComponent<CharacterController>();
		Boss = GameObject.Find("Boss");
		script = Boss.GetComponent<Enemy_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
		// If the shot has a nonzero y-position, it is changed to have one, since this is extremely important.
		if(transform.position.y != 0) {
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
		
		// If this object is a clone of the original shot, move it.
        if(gameObject.name == "Sphere(Clone)") {
			control.Move(transform.TransformDirection(movement));
		}
		
		// If this object is a charged shot, it travels twice as fast
		// as a normal shot.
		else if(gameObject.name == "Charge Shot") {
			control.Move(transform.TransformDirection(2 * movement));
		}
		
		// If this object is a fully charged shot, it travels six times
		// as fast as a normal shot!
		else if(gameObject.name == "Super Charge Shot") {
			control.Move(transform.TransformDirection(6 * movement));
		}
    }
	
	// Destroys the shot when it touches a collider that is not the player, along with other effects.
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name != "Shooter") {
			Destroy(gameObject);
		
			// If the collider the shot touched was the enemy's, it will broadcast
			// that the enemy was hit and inflict damage.
			if (other.gameObject.name == "Boss"){		
				if(gameObject.name == "Sphere(Clone)") script.HP -= 1;
				else if(gameObject.name == "Charge Shot") script.HP -= 2;
				else if(gameObject.name == "Super Charge Shot") script.HP -= 4;
				script.EnemyHit();
			}
		}
	}
}
