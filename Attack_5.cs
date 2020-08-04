/*
A script to manage the projectiles from Attack 5. The shots are
summoned high up above the arena and drop down, harmless at first.
After a few seconds, they explode one by one. After this, they are
destroyed.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_5 : MonoBehaviour
{
	
	Collider col;
	
	int randx;
	int randz;
	
	Vector3 zero = new Vector3 (0,0,0);
	Vector3 small = new Vector3 (5,5,5);
	Vector3 large = new Vector3 (200,200,200);
	
    // Start is called before the first frame update
    void Start()
    {
        // If this object is a clone of the original, do several things.
		if (gameObject.name == "Attack 5(Clone)") {
			// Move it to a random position above the player.
			randx = Random.Range(-350, 351);
			randz = Random.Range(-350, 351);		
			transform.position = new Vector3(randx, 400, randz);
			
			// Make it small.
			transform.localScale = small;
			
			// Disable its collider.
			col = GetComponent<Collider>();
			col.enabled = false;

			StartCoroutine(Drop_Shot());
		}
    }

    // Update is called once per frame
    void Update()
    {
		
    }
	
	IEnumerator Drop_Shot() {
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Use Vector3.Lerp to smoothly drop the shot down to y = 0.
		Vector3 start = transform.position;
		Vector3 final = new Vector3(transform.position.x, 0, transform.position.z);
		do
		{
			transform.position = Vector3.Lerp(start, final, currenttime);
			currenttime += Time.deltaTime;
			yield return null;				
		}while (currenttime <= 1f);
		
		// Wait a short time.
		currenttime = 0.0f;
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 3f);
		
		// Now, enable the collider and rapidly grow the shot to max size.
		currenttime = 0.0f;
		col.enabled = true;
		
		do 
		{
			transform.localScale = Vector3.Lerp(small, large, 10 * currenttime);
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 0.1f);
		
		// Once the shot has exploded, wait for one second and then remove it.
		currenttime = 0.0f;
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 1f);
		
		currenttime = 0.0f;
		
		do 
		{
			transform.localScale = Vector3.Lerp(large, zero, 10 * currenttime);
			currenttime += Time.deltaTime;
			yield return null;
		}while (currenttime <= 0.1f);	

		Destroy(gameObject);
	}
}
