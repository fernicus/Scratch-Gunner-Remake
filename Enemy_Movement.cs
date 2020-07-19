/*
A large script to manage the majority of the enemy character's behavior.
The enemy will naturally warp around the arena and fire a standard shot
at the player, and it will also periodically perform more flashy (and difficult)
attacks. As its HP decreases, it will do these more often. It will also react to
being hit by the player, and teleport away when this happens.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
	private GameObject shot_parent;
	private GameObject clone;
	private GameObject Shooter;
	private GameObject attack1_parent;
	Attack_1 script1;
	
	// The int HP is how much health the enemy has. Much of
	// its behavior depends on how much HP is remaining.
	public int HP = 20;
	
	// The bools attacking and teleporting are true when the enemy
	// attacks and teleports, respectively. This makes it so that 
	// certain functions are suspended in these situations.
	bool attacking = false;
	bool teleporting = false;
	
	// The int teleportthres gives a threshold value, in frames, for when
	// the enemy should move to a new location while not attacking.
	int teleportthres;
	
	// In a similar vein, the int attackthres gives a threshold value, in 
	// frames, for when the enemy should begin a special attack.
	int attackthres;
	
	// The int attacknum simply stores a random value to dictate what attack is
	// used. newattacknum checks the new random value for an attack, and if it is 
	// the same as the old one, it will reroll to keep things varied.
	int attacknum;
	int newattacknum;
	
    // Start is called before the first frame update
    void Start()
    {
		ResetTeleport();
		ResetAttack();
		
		shot_parent = GameObject.Find("Enemy Shot");
		Shooter = GameObject.Find("Shooter");
		attack1_parent = GameObject.Find("Attack 1");
		script1 = attack1_parent.GetComponent<Attack_1>();
		
		attacknum = 0;
		newattacknum = 0;
    }

    // Update is called once per frame
    void Update()
    {
		//if(Input.GetKeyDown("space")) StartCoroutine(Attack_1());
		
		
		// Once the enemy is defeated, this script disables itself.
		if (HP <= 0) this.enabled = false;
		
		// If the enemy object does not have a y-position of 0, it is changed to have one, since this is extremely important.
		if(transform.position.y != 0) {
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
		
		// This procedure becomes suspended while the enemy is attacking or teleporting.
        if(!attacking && !teleporting) {
			
			// If the teleport counter is not exhausted, decrement it. Do the same for attackthres.
			if(teleportthres > 0) teleportthres--;
			if(attackthres > 0) attackthres--;
			
			else if (attackthres <= 0) {
				// When the attack threshold is exhausted, reset it immediately, since it will not be decremented while
				// the enemy attacks anyways.
				ResetAttack();
				
				// If not attacking already, pick a random attack and execute it (except for the attack that was previously used).
				if (!attacking) {
					
					while (newattacknum == attacknum) newattacknum = Random.Range(1, 3);
					
					attacknum = newattacknum;
					
					if(attacknum == 1) StartCoroutine(Attack_1());
					
					else if (attacknum == 2) Debug.Log("Attack 2 goes here!");
				
				}
			}				
			
			else if (teleportthres <= 0) {
				// When the teleport threshold is exhausted, and if not already teleporting,
				// start the teleport coroutine.
				if (!teleporting) StartCoroutine(Teleport(.5f, true, true));
			}
		}
    }
	
	
	// A coroutine that teleports the enemy. If the attackAfter argument is true, it will
	// create a normal shot after teleporting. If the resetCount argument is true, it will
	// reset the normal teleport counter.
	public IEnumerator Teleport(float time, bool attackAfter, bool resetCount){
		if (HP > 0) {
			// Set teleporting to true immediately.
			teleporting = true;
			
			// The current relative time starts at 0.
			float currenttime = 0.0f;
			
			// large and small are two scale vectors, with large being the enemy's normal size
			// and small being a scale of 0 for when it teleports.
			Vector3 large = new Vector3(5,5,5);
			Vector3 small = new Vector3(0,0,0);
			
			// For the first half of the allotted time, shrink the enemy down until it has a scale of 0.
			do 
			{
				transform.localScale = Vector3.Lerp(large, small, 2 *currenttime / time);
				currenttime += Time.deltaTime;
				yield return null;
				
			}while (currenttime <= time/2);
			
			// Now that the enemy is no longer visible, move it to a new random location.
			int newx = Random.Range(-378, 378);
			int newz = Random.Range(-378,378);
			transform.position = new Vector3(newx, 0, newz);
			
			// For the second half of the allotted time, make the enemy grow back to its normal size.
			do 
			{
				transform.localScale = Vector3.Lerp(small, large, 2 * (currenttime - time/2)/ time);
				currenttime += Time.deltaTime;
				yield return null;
				
			}while (currenttime > time/2 && currenttime < time);
			
			// Now that the teleport is completed, teleporting is false once again.
			teleporting = false;
			
			// Teleportation is utilized in some attacks, so the teleport threshold is reset here in
			// those instances. In this way, when an attack finishes the enemy will have to wait the 
			// full period to teleport again in the standard way.
			if (resetCount == true) ResetTeleport();
			
			// If attackAfter is true, the enemy immediately creates a shot after it arrives at the
			// new location.
			if(attackAfter) {
				
				clone = Instantiate(shot_parent) as GameObject;
				clone.transform.position = gameObject.transform.position;
				clone.transform.LookAt(Shooter.transform);
			
			}
		}
	}
	
	// These two functions reset their respective counters.
	void ResetTeleport() {
        teleportthres = (int)Random.Range(0,5 * HP) + 5 * HP + 120;
	}
	
	void ResetAttack() {
		attackthres = (int)Random.Range(0, 10 * HP) + 5 * HP + 240;		
	}
	
	// The enemy's first attack. It teleports to a new location, summons a ring of projectiles around
	// itself, and launches them.
	public IEnumerator Attack_1() {		
		// Set attacking to true immediately.
		attacking = true;
		
		// speed dictates how long it will take to summon a single projectile. The less HP the enemy
		// has remaining, the less time this will take.
		float speed = (HP + 5) * .005f;
		
		Vector3 large = new Vector3(3,3,3);
		Vector3 small = new Vector3(0,0,0);
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Teleport immediately to begin.
		StartCoroutine(Teleport(0.5f, false, false));
		
		//Do nothing while teleporting, and for a short while after.
		do{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime < 1.0f);
		
		// Reset the current relative time after this.
		currenttime = 0.0f;
		
		for (int i = 0; i < 8; i++) {
			
			// Create a projectile.
			clone = Instantiate(attack1_parent) as GameObject;
			clone.transform.position = gameObject.transform.position;
			clone.transform.eulerAngles = new Vector3(90f, 45f * i, 0f);
			
			// During the allotted speed time, grow the projectile.
			do
			{
				clone.transform.localScale = Vector3.Lerp(small, large, currenttime / speed);
				currenttime += Time.deltaTime;
				yield return null;
				
			}while (currenttime < speed);
				
			currenttime = 0.0f;
		}
		
		// Once the projectiles are summoned, wait a short time and then teleport somewhere.
		currenttime = 0.0f;
		do{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime < 1.0f);
		
		attacking = false;
		
		StartCoroutine(Teleport(0.5f, false, true));
	}
	
	
	// This function is called from the Move_Shot script, when the player's shot connects
	// with the enemy. When hit, the enemy swiftly teleports. It will not do this if hit while 
	// it is already teleporting or attacking, however.
	public void EnemyHit() {
		if (!teleporting && !attacking){
			StartCoroutine(Teleport(0.25f, false, false));
		}
	}
}
