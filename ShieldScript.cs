/*
A simple script to manage the shield model. The model itself is purely cosmetic and has no effect 
on gameplay, but it looks pretty! When the player activates the shield, it scales itself up to size,
and when it is exhausted it disappears once more.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
	
	MeshRenderer shieldrend;
	public GameObject Shooter;
	
	// large and small are two scale vectors, with large being the shield's normal size
	// and small being a scale of 0 for when it is inactive.
	Vector3 large;
	Vector3 small;
	
    // Start is called before the first frame update
    void Start()
    {
		large = new Vector3(40,40,40);
		small  = new Vector3(0,0,0);	
        shieldrend = GetComponent<MeshRenderer>();
		shieldrend.material.SetColor("_Color", Color.cyan);
		shieldrend.enabled = false;
		
		transform.localScale = small;
		
		Shooter = GameObject.Find("Shooter");
    }


    // Update is called once per frame
    void Update()
    {
		// The shield's position always matches the player's.
		transform.position = Shooter.transform.position;
    }
	
	// Called from the Movement script, this function just starts the 
	// ShieldRoutine.
	public void ShieldActivate() {
		StartCoroutine(ShieldRoutine());
		
	}

	// Makes the shield model visible for 1 second.
	IEnumerator ShieldRoutine() {
		// Enable the mesh renderer.
		shieldrend.enabled = true;

		// The current relative time starts at 0.
		float currenttime = 0.0f;
		
		// Use 0.05 seconds to grow the shield.
		do 
		{
			transform.localScale = Vector3.Lerp(small, large, currenttime / 0.05f);
			currenttime += Time.deltaTime;
			yield return null;
			
		}while (currenttime <= 0.05f);		
		
		// Wait 0.9 seconds.
		currenttime = 0.0f;
		do 
		{
			currenttime += Time.deltaTime;
			yield return null;
			
		}while (currenttime <= 0.9f);
		
		// Use 0.05 seconds to remove the shield model.
		currenttime = 0.0f;
		do 
		{
			transform.localScale = Vector3.Lerp(large, small, currenttime / 0.05f);
			currenttime += Time.deltaTime;
			yield return null;
			
		}while (currenttime <= 0.05f);
		
		// Disable the mesh renderer.
		shieldrend.enabled = false;
		
	} 
}
