using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Shot : MonoBehaviour
{
	
	private CharacterController control;
	private Vector3 movement = new Vector3(0,0,-1.0f);

    // Start is called before the first frame update
    void Start()
    {
		control = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
		// If the shot has a nonzero y-position, it is changed to have one, since this is extremely important.
		if(transform.position.x != 0) {
			transform.position = new Vector3(transform.position.x, 0, transform.position.z);
		}
		
		// If this object is a clone of the original shot, move it.
        if(gameObject.name == "Sphere(Clone)") {
			control.Move(transform.TransformDirection(movement));
		}
		
		else if(gameObject.name == "Charge Shot") {
			control.Move(transform.TransformDirection(2 * movement));
		}
		
		else if(gameObject.name == "Super Charge Shot") {
			control.Move(transform.TransformDirection(6 * movement));
		}
    }
	
	// Destroys the shot when it touches a collider.
	void OnTriggerEnter(Collider other) {
		Destroy(gameObject);
	}
}
