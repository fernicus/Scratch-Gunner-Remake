/*
A simple script to manage the player's color in various situations. When the player gets
hit, they will flash red for the time that they are invincible, and if they use their shield,
they will flash white for a moment when it recharges.
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
	MeshRenderer rend;
	Color color;
	Color defaultcolor;
	
    // Start is called before the first frame update
    void Start()
    {
        rend = gameObject.GetComponent<MeshRenderer>();
		defaultcolor = new Color(0.5f, 1f, 0.5f, 1f);
		rend.material.SetColor("_Color",defaultcolor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	// Called from the Movement script, this function just starts the TookDamage
	// coroutine.
	public void BecomeRed() {
		StartCoroutine(TookDamage());
		
	}
	
	// When the player takes damage, they become invincible for 1 second. They will
	// flash red during this period to indicate the damage and also the 
	// invincibility period.
	IEnumerator TookDamage() {
		
		// The current relative time starts at 0.
		float currenttime  = 0.0f;
		
		// Repeat this process 10 times.
		for (int i = 0; i < 10; i++) {
			// Turn the player red for 0.05 seconds.
			currenttime = 0.0f;
			rend.material.SetColor("_Color",Color.red);
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			
			
			}while(currenttime < 0.05f);			
			
			// Turn the player back to the default color for 0.05 seconds.
			currenttime = 0.0f;
			rend.material.SetColor("_Color",defaultcolor);
			do
			{
				currenttime += Time.deltaTime;
				yield return null;
			
			}while(currenttime < 0.05f);
		
		}
	}
	
	
	
	// Called from the Movement script, this function just starts the ShieldCharged
	// coroutine.
	public void BecomeWhite() {
		StartCoroutine(ShieldCharged());
	}
	
	// Once the player's shield recharges, they flash white for a moment to signify this.
	IEnumerator ShieldCharged() {
		
		// Store the player's initial color.
		color = rend.material.color;
		
		// Turn the player white.
		rend.material.SetColor("_Color", Color.white);
		
		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Do nothing for 0.2 seconds, then change the player back to their original color.
		do
		{
			currenttime += Time.deltaTime;
			yield return null;
			
		}while(currenttime < 0.2f);
		
		rend.material.SetColor("_Color",color);
	}
	
}
