﻿/*
A large script to manage the majority of the enemy character's behavior.
The enemy will naturally warp around the arena and fire a standard shot
at the player, and it will also periodically perform flashy (and difficult)
attacks. As its HP decreases, it will do these more often, and generally more
aggressively. It will also react to being hit by the player, and teleport away 
when this happens.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
	private GameObject shot_parent;
	private GameObject clone;
	private GameObject Shooter;
	
	private GameObject attack1_2_parent;
	Attack_1_2 script1;
	
	private GameObject attack3_4_parent;
	Attack_3_4 script2;
	
	private GameObject attack5_parent;
	Attack_5 script3;
	
	private GameObject attack6_parent;
	Attack_6 script4;
	
	private GameObject attack7_parent;
	Attack_7 script5;
	
	MeshRenderer rend;
	
	// The float HP is how much health the enemy has. Much of
	// its behavior depends on how much HP is remaining.
	public float HP = 20.0f;
	
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
		
		attack1_2_parent = GameObject.Find("Attack 1/2");
		script1 = attack1_2_parent.GetComponent<Attack_1_2>();		
		
		attack3_4_parent = GameObject.Find("Attack 3/4");
		script2 = attack3_4_parent.GetComponent<Attack_3_4>();
		
		attack5_parent = GameObject.Find("Attack 5");
		script3 = attack5_parent.GetComponent<Attack_5>();		
		
		attack6_parent = GameObject.Find("Attack 6");
		script4 = attack6_parent.GetComponent<Attack_6>();		
		
		attack7_parent = GameObject.Find("Attack 7");
		script5 = attack7_parent.GetComponent<Attack_7>();
		
		attacknum = 0;
		newattacknum = 0;
		
		rend = GetComponentInChildren<MeshRenderer>();
		rend.material.SetColor("_Color", Color.gray);
    }

    // Update is called once per frame
    void Update()
    {
		
		// Debug line
		if(Input.GetKeyDown("1")) StartCoroutine(Attack_6());
		
		
		// Once the enemy is defeated, this function stops running.
		if (HP > 0) {
			
			// If the enemy object does not have a y-position of 0, it is changed to have one, since this is extremely important.
			if(transform.position.y != 0) {
				transform.position = new Vector3(transform.position.x, 0, transform.position.z);
			}
			
			// This procedure becomes suspended while the enemy is attacking or teleporting.
			if(!attacking && !teleporting) {
				
				// If the teleport counter is not exhausted, decrement it. Do the same for attackthres.
				if(teleportthres > 0) teleportthres--;
				if(attackthres > 0) attackthres--;
				
				if (attackthres <= 0) {
					// When the attack threshold is exhausted, reset it immediately, since it will not be decremented while
					// the enemy attacks anyways.
					ResetAttack();
					
					// If not attacking already, pick a random attack and execute it (except for the attack that was previously used).
					if (!attacking) {
						
						while (newattacknum == attacknum) newattacknum = Random.Range(1, 8);
						
						attacknum = newattacknum;
						
						if(attacknum == 1) 		 StartCoroutine(Attack_1());
						else if (attacknum == 2) StartCoroutine(Attack_2());
						else if (attacknum == 3) StartCoroutine(Attack_3());
						else if (attacknum == 4) StartCoroutine(Attack_4());
						else if (attacknum == 5) StartCoroutine(Attack_5());
						else if (attacknum == 6) StartCoroutine(Attack_6());
						else if (attacknum == 7) StartCoroutine(Attack_7());
					
					}
				}				
				
				else if (teleportthres <= 0) {
					// When the teleport threshold is exhausted, start the teleport coroutine.
					StartCoroutine(Teleport(.5f, true, true));
				}
			}
			
		}
    }

// MISC FUNCTIONS
//-----------------------------------------------------------------------------------------------------------------------------
	
	// These two functions reset their respective counters.
	void ResetTeleport() {
        teleportthres = (int)Random.Range(0,(int)(5 * HP)) + (int)(5 * HP) + 240;
	}
	
	void ResetAttack() {
		attackthres = (int)Random.Range(0, (int)(10 * HP)) + (int)(10 * HP) + 480;		
	}
	
	// This function is called from the Move_Shot script, when the player's shot connects
	// with the enemy. When hit, the enemy swiftly teleports. It will not do this if hit while 
	// it is already teleporting or attacking. Once the enemy's HP is totally depleted, the 
	// game is won!
	public void EnemyHit() {
		if (!teleporting && !attacking && HP > 0){
			StartCoroutine(Teleport(0.25f, false, false));
		}
		else if (HP <= 0) {
			StopAllCoroutines();
			StartCoroutine(YouWon());
		}
	}
	
	
	// This coroutine runs when the enemy's HP is depleted. It is purely cinematic.
	public IEnumerator YouWon() {
		Vector3 large = new Vector3(5,5,5);
		Vector3 small = new Vector3(0,0,0);
		Vector3 evenlarger = new Vector3 (10,10,10);
		Debug.Log("You won!");
		
		float currenttime = 0.0f;
		
		for (int i = 0; i < 50; i++) {
			StartCoroutine(Teleport(0.1f, false, false));
			
			currenttime = 0.0f;
			do{
				currenttime += Time.deltaTime;
				yield return null;
			}while(currenttime<= 0.2f);
			
		}
		
		StartCoroutine(TeleportTo(0,0,0.1f));

		currenttime = 0.0f;
		
		do{
			currenttime += Time.deltaTime;
			yield return null;
		}while(currenttime <= 1.5f);
		
		
		currenttime = 0.0f;
		
		do{
			transform.localScale = Vector3.Lerp(large, evenlarger, currenttime / 5);
			
			rend.material.SetColor("_Color", Color.Lerp(Color.gray, Color.red, currenttime / 15)); 
			
			currenttime += Time.deltaTime;
			yield return null;
		}while(currenttime <= 5f);
		
		currenttime = 0.0f;
		
		do{
			transform.localScale = Vector3.Lerp(evenlarger, small, 10 * currenttime);
			currenttime += Time.deltaTime;
			yield return null;
		}while(currenttime <= 0.1f);
		
		Destroy(gameObject);
	}
	
	// This function calculates the angle between two points. It comes up a lot, and Transform.LookAt is screwy sometimes,
	// so this is a big timesaver.
	float AngleBetween(Vector3 a, Vector3 b) {
		return Mathf.Atan2(a.z - b.z, a.x - b.x) * Mathf.Rad2Deg;
	}
	
// MOVEMENT
//-----------------------------------------------------------------------------------------------------------------------------

	// A coroutine that teleports the enemy. If the attackAfter argument is true, it will
	// create a normal shot after teleporting. If the resetCount argument is true, it will
	// reset the normal teleport counter.
	public IEnumerator Teleport(float time, bool attackAfter, bool resetCount){
		// Set teleporting to true immediately.
		teleporting = true;
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// large and small are two scale vectors, with large being the enemy's normal size
		// and small being a scale of 0 for when it teleports.
		Vector3 large = new Vector3(5,5,5);
		Vector3 small = new Vector3(0.5f,0.5f,0.5f);
		
		// For the first 40% of the allotted time, shrink the enemy down until it has a scale of 0.
		do 
		{
			transform.localScale = Vector3.Lerp(large, small, 5 * currenttime / (2 * time));
			currenttime += Time.deltaTime;
			yield return null;
			
		}while (currenttime <= 2*time/5);
		
		// Now that the enemy is no longer visible, move it to a new random location.
		int newx = Random.Range(-378, 379);
		int newz = Random.Range(-378,379);
		Vector3 newpos = new Vector3(newx, 0, newz);
		Vector3 currentpos = transform.position;
		
		// Use 20% of the allotted time to move the enemy.
		currenttime = 0.0f;
		do
		{
			transform.position = Vector3.Lerp(currentpos,newpos, 5 * currenttime / time);
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= time/5);
		
		// For the final 40% of the allotted time, make the enemy grow back to its normal size.
		currenttime = 0.0f;
		do 
		{
			transform.localScale = Vector3.Lerp(small, large, 5 * currenttime / (2 * time));
			currenttime += Time.deltaTime;
			yield return null;
			
		}while (currenttime <= 2*time/5);
		
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
	
	// Sometimes an attack will require the enemy to move to a specific location, and that 
	// is what TeleportTo is for. Almost all of this procedure is the same as for random 
	// teleportation.
	public IEnumerator TeleportTo(int x, int z, float time) {
		
		teleporting = true;

		float currenttime = 0.0f;

		Vector3 large = new Vector3(5,5,5);
		Vector3 small = new Vector3(0,0,0);

		do 
		{
			transform.localScale = Vector3.Lerp(large, small, 5 *currenttime / (2 * time));
			currenttime += Time.deltaTime;
			yield return null;
			
		}while (currenttime <= 2*time/5);
		
		// Now that the enemy is no longer visible, move it to the designated location, instead of 
		// a random location.
		Vector3 newpos = new Vector3(x, 0, z);
		Vector3 currentpos = transform.position;

		currenttime = 0.0f;
		do
		{
			transform.position = Vector3.Lerp(currentpos,newpos, 5 * currenttime / time);
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= time/5);
		
		currenttime = 0.0f;
		do 
		{
			transform.localScale = Vector3.Lerp(small, large, 5 * currenttime / (2 * time));
			currenttime += Time.deltaTime;
			yield return null;
			
		}while (currenttime <= 2*time/5);
	}
	
	
// ATTACKS
//-----------------------------------------------------------------------------------------------------------------------------
	
	// The enemy's first attack. It teleports to a new location, summons a ring of projectiles around
	// itself, and launches them. As its HP drops, it will summon more projectiles.
	public IEnumerator Attack_1() {		
		// Set attacking to true immediately.
		attacking = true;
		
		// speed dictates how long it will take to summon a single projectile. The less HP the enemy
		// has remaining, the less time this will take.
		float speed = (HP + 5) * .005f;
		
		// offset is a random variable that dictates where the projectiles begin to be summoned
		// from, so that the first one is not always summoned at angle 0.
		float offset = (float)Random.Range(0,360);
		
		Vector3 large = new Vector3(3,3,3);
		Vector3 small = new Vector3(0,0,0);
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Teleport immediately to begin.
		StartCoroutine(Teleport(0.5f, false, false));
		
		//Do nothing while teleporting, and for a short while after.
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 1.0f);
		
		// Reset the current relative time after this.
		currenttime = 0.0f;
		
		// If the enemy has more than half its HP remaining, it will summon a ring
		// of 8 projectiles.
		if(HP > 10) {
			
			for (int i = 0; i < 8; i++) {
				
				// Create a projectile.
				clone = Instantiate(attack1_2_parent) as GameObject;
				clone.transform.position = gameObject.transform.position;
				clone.transform.eulerAngles = new Vector3(90f, 45f * i + offset, 0f);
				
				// During the allotted speed time, grow the projectile.
				do
				{
					if(clone) clone.transform.localScale = Vector3.Lerp(small, large, currenttime / speed);
					currenttime += Time.deltaTime;
					yield return null;
					
				}while (currenttime <= speed);
					
				currenttime = 0.0f;
			}
		}
		
		// If it has less than half its HP, it will summon 12 projectiles instead.
		else {
			
			for (int i = 0; i < 12; i++) {	
				clone = Instantiate(attack1_2_parent) as GameObject;
				clone.transform.position = gameObject.transform.position;
				clone.transform.eulerAngles = new Vector3(90f, 30f * i + offset, 0f);
				do
				{
					if(clone) clone.transform.localScale = Vector3.Lerp(small, large, currenttime / speed);
					currenttime += Time.deltaTime;
					yield return null;
					
				}while (currenttime <= speed);
				currenttime = 0.0f;
			}
			
		}
		
		// Once the projectiles are summoned, wait a short time and then teleport somewhere.
		currenttime = 0.0f;
		do{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime < 1.0f);
		
		attacking = false;
		
		StartCoroutine(Teleport(0.5f, false, false));
	}
	
	// The enemy's second attack. It teleports to new locations and fires bursts of 4 projectiles, or
	// 6 if its HP is below half.
	public IEnumerator Attack_2() {
		// Set attacking to true immediately.
		attacking = true;
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// The bursts of shots will be randomly offset by either 0 or 45 degrees (or 0 to 30 degrees
		// if HP is below half).
		float offset;
		
		// Perform this procedure 5 times.
		for(int i = 0; i < 5; i++) {
			StartCoroutine(Teleport(0.5f, false, false));
			
			// Do nothing while teleporting, and for a short while after.
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while (currenttime <= 1.0f);

			if (HP > 10) {
				// Select the offset and summon 4 projectiles.
				offset = (float)45 * Random.Range(0,2);
				
				for (int j = 0; j < 4; j++) {
					clone = Instantiate(attack1_2_parent) as GameObject;
					clone.transform.localScale = new Vector3(2,2,2);
					clone.transform.position = gameObject.transform.position;
					clone.transform.eulerAngles = new Vector3(90f, 90f * j + offset, 0f);		
					
				}
			}			
			
			else {
				// Select a different offset and summon 6 projectiles instead.
				offset = (float)30 * Random.Range(0,2);
				
				for (int j = 0; j < 6; j++) {
					clone = Instantiate(attack1_2_parent) as GameObject;
					clone.transform.position = gameObject.transform.position;
					clone.transform.localScale = new Vector3(2,2,2);
					clone.transform.eulerAngles = new Vector3(90f, 60f * j + offset, 0f);		
					
				}
			}
			
			// Once the projectiles are summoned, wait a short time.
			currenttime = 0.0f;
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while (currenttime <= 0.5f);
			
		}
		
		// Once this is done, teleport somewhere new.
		attacking = false;
		
		StartCoroutine(Teleport(0.5f, false, false));
		
	}

	// The enemy's third attack. It teleports to new locations 4 times and fires a spray of shots at the 
	// player each time.
	public IEnumerator Attack_3() {
		// Set attacking to true immediately.
		attacking = true;
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;

		// Do this procedure 4 times.
		for (int i = 0; i < 4; i++) {
			StartCoroutine(Teleport(0.5f, false, false));
			
			// Do nothing while teleporting, and for a short while after.
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while (currenttime <= 1.0f);			
			
			// Summon 3 waves of projectiles per pass.
			for (int j = 0; j < 3; j++) {
			
				// Summon 3 projectiles at once.
				for (int k = -1; k < 2; k++) {
					clone = Instantiate(attack3_4_parent) as GameObject;
					
					Vector3 enemypos = gameObject.transform.position;
					Vector3 playerpos = Shooter.transform.position;
					
					clone.transform.position = enemypos;
					clone.transform.rotation = Quaternion.Euler(new Vector3(90,25*k+270-AngleBetween(enemypos,playerpos), 0));
				}
				
				currenttime = 0.0f;
				do
				{
					currenttime += Time.deltaTime;
					yield return null;
				}while (currenttime <= 0.1f);
			}
			
			// Wait a short time before teleporting once more.
			currenttime = 0.0f;
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while (currenttime <= 0.5f);
			
		}
		
		// Once this is done, teleport somewhere new.
		attacking = false;
		
		StartCoroutine(Teleport(0.5f, false, false));
	}
	
	// The enemy's fourth attack. It moves to the center of the arena and sprays shots all around itself
	// for a while, attacking for longer the lower its HP is.
	public IEnumerator Attack_4() {
		// Set attacking to true immediately.
		attacking = true;
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Teleport to the center of the arena.
		StartCoroutine(TeleportTo(0,0,0.5f));
		
		// Do nothing while teleporting, and for a short while after.
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 1.0f);
		
		// numshots is how many shots are to be created.
		int numshots = (int)(10*Mathf.Exp(.1f * (20-HP))+40);
		
		int offset = 0;
		
		// Instantiate that many shots.
		for (int i = 0; i < numshots; i++) {
			// Each shot is fired in a random direction.
			offset = Random.Range(0,360);
			clone = Instantiate(attack3_4_parent) as GameObject;
			clone.transform.position = gameObject.transform.position;
			clone.transform.rotation = Quaternion.Euler(new Vector3(90,offset,0));
			
			currenttime = 0.0f;
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while (currenttime <= 0.1f);
		}
		
		// Wait a short time before teleporting once more.
		currenttime = 0.0f;
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 1.0f);

		attacking = false;
		
		StartCoroutine(Teleport(0.5f, false, false));		
		
	}
	
	
	// The enemy's fifth attack. It disappears, and several small shots rain down from above before exploding.
	// As the enemy loses HP, it will drop more and more shots.
	public IEnumerator Attack_5() {
		// Set attacking to true immediately.
		attacking = true;
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Move the enemy offscreen.
		StartCoroutine(TeleportTo(1000,1000,0.5f));
		
		// Wait a half second.
		do 
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 0.5f);
		
		// Create shots. The number of shots increases linearly as the enemy loses HP, starting from 3 and up to
		// a maximum of 6.
		for (int i = 0; i < (int)Mathf.Floor((float)(15-3*HP/10)); i++) {
			currenttime = 0.0f;
			
			clone = Instantiate(attack5_parent) as GameObject;
			
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while (currenttime <= 0.5f);
			
			
		}
		
		// Wait for the last shot to explode, then teleport back into the arena.
		currenttime = 0.0f;
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 6f);
		
		attacking = false;
		
		StartCoroutine(Teleport(0.5f, false, false));
		
	}

	// The enemy's sixth attack. It teleports to a new location and charges up, before unleashing a large and 
	// powerful beam of shots directly at the player.
	public IEnumerator Attack_6() {
		// Set attacking to true immediately.
		attacking = true;
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Teleport immediately to begin.
		StartCoroutine(Teleport(0.5f, false, false));
		
		// Wait a short time.
		do{
			currenttime += Time.deltaTime;
			yield return null;
		}while(currenttime <= 1f);
		
		// Store the position of the enemy to save some trouble.
		Vector3 originalpos = gameObject.transform.position;
		
		// Create 2 position vectors that are slightly offset from the original.
		Vector3 pos1 = new Vector3(originalpos.x - 5, 0, originalpos.z);
		Vector3 pos2 = new Vector3(originalpos.x + 5, 0, originalpos.z);
		
		// Do nothing while charging, but indicate charging by vibrating.
		for (int i = 0; i < 20; i++) {
			currenttime = 0.0f;
			do{
				transform.position = pos1;
				currenttime += Time.deltaTime;
				yield return null;	
			}while(currenttime <= .025f);			
			
			currenttime = 0.0f;
			do{
				transform.position = originalpos;
				currenttime += Time.deltaTime;
				yield return null;
			}while(currenttime <= .025f);
			
			currenttime = 0.0f;
			do{
				transform.position = pos2;
				currenttime += Time.deltaTime;
				yield return null;	
			}while(currenttime <= .025f);			
			
			currenttime = 0.0f;
			do{
				transform.position = originalpos;
				currenttime += Time.deltaTime;
				yield return null;
			}while(currenttime <= .025f);
			
		}
		
		// Create a large stream of Attack 6 projectiles aimed at the player's position. The beam will not
		// track the player, only aim at their position initially.
		Vector3 playerpos = Shooter.transform.position;
		
		float aimangle = 180-AngleBetween(originalpos,playerpos);
		
		for (int i = 0; i < 200; i++) {
			clone = Instantiate(attack6_parent) as GameObject;
			
			clone.transform.position = gameObject.transform.position;
					
			clone.transform.rotation = Quaternion.Euler(new Vector3(0,aimangle + Random.Range(-10f,10f), 0));
			
			currenttime = 0.0f;
			do 
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while(currenttime <= 0.00625);
		}
		
		// Once this is done, wait a short time and then teleport somewhere new.		
		currenttime = 0.0f;
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 0.5f);
			
		
		attacking = false;
		
		StartCoroutine(Teleport(0.5f, false, false));
	}
	
	// The enemy's seventh attack. It teleports to new locations and creates lethargic projectiles that
	// follow sinusoidal paths and leave behind damaging trails. As it loses HP, it will create more projectiles.
	public IEnumerator Attack_7() {
		// Set attacking to true immediately.
		attacking = true;
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;	
		
		// Do this procedure a number of times.
		for (int i = 0; i < (int)(11 - .25 * HP); i++) {
			
			StartCoroutine(Teleport(0.5f, false, false));
			
			// Do nothing while teleporting, and for a short time after.
			currenttime = 0.0f;
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while (currenttime <= .75f);
			
			Vector3 enemypos = gameObject.transform.position;
			Vector3 playerpos = Shooter.transform.position;
					
			clone = Instantiate(attack7_parent) as GameObject;
			clone.transform.position = enemypos;
			
			clone.transform.rotation = Quaternion.Euler(new Vector3(0,180-AngleBetween(enemypos,playerpos), 0));

			// After the projectile is summoned, wait a short time before
			// teleporting again.
			currenttime = 0.0f;
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			}while (currenttime <= 0.25f);			
			
		}
		
		// Once this is done, teleport somewhere new.
		attacking = false;
		
		StartCoroutine(Teleport(0.5f, false, false));
	}
}
