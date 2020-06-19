using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Normal_Shot : MonoBehaviour {
	
public Object projectile;
private GameObject clone;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		// If the space bar is pressed
		if(Input.GetKeyDown("space")) {
			// If this object is not a clone
			if(projectile.name == "Sphere"){
				clone = Instantiate(projectile) as GameObject;
				Destroy(clone,0.5f);
			}
			
		}
		
	}
}
