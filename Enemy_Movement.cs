using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
	private GameObject shot_parent;
	private GameObject clone;
	private GameObject Shooter;
	
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
	
    // Start is called before the first frame update
    void Start()
    {
        teleportthres = (int)Random.Range(0,20 * HP) + 500;
		shot_parent = GameObject.Find("Enemy Shot");
		Shooter = GameObject.Find("Shooter");
    }

    // Update is called once per frame
    void Update()
    {
		// Once the enemy is defeated, this script disables itself.
		if (HP <= 0) this.enabled = false;
		
		// If the enemy object does not have a y-position of 0, it is changed to have one, since this is extremely important.
		if(transform.position.y != 0) {
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
		
		// This procedure becomes suspended while the enemy is attacking or teleporting.
        if(!attacking && !teleporting) {
			
			// If the teleport counter is not exhausted, decrement it.
			if(teleportthres > 0) teleportthres--;
			
			else if (teleportthres <= 0) {
				// When the teleport threshold is exhausted, then teleport.
				// If not already teleporting, start the teleport coroutine.
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
			int newx = (int)Random.Range(-378, 378);
			int newz = (int)Random.Range(-378,378);
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
			if (resetCount == true) teleportthres = (int)Random.Range(0,20 * HP) + 500;
			
			// If attackAfter is true, the enemy immediately creates a shot after it arrives at the
			// new location.
			if(attackAfter) {
				
				clone = Instantiate(shot_parent) as GameObject;
				clone.transform.position = gameObject.transform.position;
				clone.transform.LookAt(Shooter.transform);
			
			}
		}
	}
	
	
	public void EnemyHit() {
		// The enemy swiftly teleports when hit. It will not do this if hit while already 
		// teleporting or attacking, however.
		if (!teleporting && !attacking){
			StartCoroutine(Teleport(0.25f, false, false));
		}
	}
}
