/*
A script to manage the player's shots once they are created. More powerful shots
will travel faster. Once they touch the arena walls or the enemy, they are
destroyed, and if they touched the enemy, it will decrease the
enemy's HP by an appropriate amount.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Shot : MonoBehaviour
{
	
	private CharacterController control;
	private Vector3 movement = new Vector3(0,0,-2f);
	
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
		
		// If this object is a charged shot, it travels four times as fast
		// as a normal shot.
		else if(gameObject.name == "Charge Shot") {
			control.Move(transform.TransformDirection(4 * movement));
		}
		
		// If this object is a fully charged shot, it travels six times
		// as fast as a normal shot!
		else if(gameObject.name == "Super Charge Shot") {
			control.Move(transform.TransformDirection(6 * movement));
		}
    }
	
	// Destroys the shot when it touches the arena walls or the enemy, and damages the enemy.
	void OnTriggerEnter(Collider other) {
		string othername = other.gameObject.name.Substring(0,4);
		if (othername == "Cube" || othername == "Boss") {
			Destroy(gameObject);
		
			// If the collider the shot touched was the enemy's, it will broadcast
			// that the enemy was hit and inflict damage, but not if the enemy was already
			// defeated.
			if (other.gameObject.name == "Boss" && script.HP > 0){		
				if(gameObject.name == "Sphere(Clone)") script.HP -= 0.2f;
				else if(gameObject.name == "Charge Shot") script.HP -= 0.4f;
				else if(gameObject.name == "Super Charge Shot") script.HP -= 0.8f;
				script.EnemyHit();
			}
		}
	}
}
