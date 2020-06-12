using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed;
	public static Vector3 movement;
	public Rigidbody rigidity;

	void Start () {
		speed = 2.0f;
		movement = new Vector3(0.0f,0.0f,-75f);
		rigidity = GetComponent<Rigidbody>();
	}
	

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
		movement.x += speed * moveHorizontal;
		movement.z += speed * moveVertical;
		rigidity.position = movement;
	}
}
