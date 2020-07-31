/*
A script to move the player character around using the WASD keys and its 
attached CharacterController, as well as use various other mechanics.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	// 
	PlayerColor colorscript;
	ShieldScript shieldscript;

	// speed dictates how speedily the player can move.
    public static float speed = 2.0f;
	
	public Vector3 movement;
	private CharacterController control;
	
	// The player's HP starts at 5, taking 1 damage from every enemy attack.
	int HP = 5;
	
	// shielded and invincible both prevent the player from being damaged when true, but
	// become true under different circumstances. shielded becomes true when the player uses 
	// the shield, while invincible becomes true immediately after taking damage.
	bool shielded;
	bool invincible;
	
	// The shield needs time to recharge after using it, and shieldcharged tracks if it is 
	// recharged or not.
	bool shieldcharged;

	void Start () {
		movement = new Vector3(0.0f,0.0f,0.0f);
		control = gameObject.GetComponent<CharacterController>();
		shielded = invincible = false;
		shieldcharged = true;
		
		colorscript =  GetComponentInChildren<PlayerColor>();
		shieldscript = GameObject.Find("Shield").GetComponent<ShieldScript>();
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
		
		// If a shot is being charged, the player's speed is halved while charging.
		if (Create_Shot.chargecount >= 20) speed = 1.0f;
		else speed = 2.0f;
		
		// When the WASD keys are not pressed, moveHorizontal and moveVertical are 0, so the movement vector will not be modified.
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		movement.x = speed * moveHorizontal;
		movement.z = speed * moveVertical;
		// The player's CharacterController is updated.
		control.Move(movement);
		
		// If the E key is pressed, and the shield is charged, activate the shield.
		if (Input.GetKeyDown("e") && shieldcharged) StartCoroutine(BecomeShielded());
	}
	
	// Runs whenever the player touches another object's collider.
	void OnTriggerEnter(Collider other) {
		// If the player is shielded or invincible from being hit previously, 
		// nothing happens when getting hit.
		if (shielded || invincible) return;
		
		// Otherwise, if the player touched something that is not itself, the enemy itself, its own shot, or the arena
		// boundaries, the player takes damage.
		string othername = other.gameObject.name;
		if (othername != "Boss" && othername != "Shooter" && othername != "Sphere(Clone)" && othername != "Charge Shot" 
		  && othername != "Super Charge Shot" && othername.Substring(0,4) != "Cube") {
			Debug.Log("haha get rekt nerd");
			StartCoroutine(GotHit());
		}
		
		
	}
	
	// This coroutine runs when the player is hit by an attack.
	public IEnumerator GotHit() {
		// Reduce the player's HP. If the player still has HP left, become invincible for one second.
		HP--;
		
		// The player becomes red to signify taking damage.
		colorscript.BecomeRed();
		
		if (HP > 0) {
			
			invincible = true;
			
			// The current relative time starts at 0.
			float currenttime = 0.0f;
		
			// Do nothing for 1 second, then make the player vulnerable again.
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while (currenttime <= 1.0f);
			
			invincible = false;
		}
		
		// Otherwise, the player is defeated and the game ends. :(
		
		//else StartCoroutine(GameOver())
		
	}
	
	// When the player's shield activates, this coroutine runs. It blocks all damage
	// taken for 1 second, before needing 2.5 seconds to recharge.
	public IEnumerator BecomeShielded() {
		shieldscript.ShieldActivate();
		
		// Discharge the shield immediately to stop it from being activated again.
		shieldcharged = false;
		
		// Become invulnerable.
		shielded = true;
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Do nothing for 1 second, then drop the shield.
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 1.0f);
			
		shielded = false;
		
		// Do nothing for 2.5 more seconds while the shield recharges.
		currenttime = 0.0f;
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 2.5f);
		
		colorscript.BecomeWhite();
		
		shieldcharged = true;
		
	}
}
