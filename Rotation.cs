using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
	
	public Camera cam;
	public Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));
		cam = Camera.main;
    }

    // This code hybridized from the code from:
	// user BenZed at https://answers.unity.com/questions/855976/make-a-player-model-rotate-towards-mouse-location.html
	// user mistermashu at https://www.reddit.com/r/Unity3D/comments/aiar6f/point_towards_mouse_around_yaxis_in_world_space/
	// Thank you both tremendously!
    void Update()
    {
		Plane p = new Plane(Vector3.up, transform.position.y);
		ray = cam.ScreenPointToRay(Input.mousePosition);
		float rayLength;
		if(p.Raycast(ray, out rayLength)){
			Vector3 mouseWorldPoint = ray.GetPoint(rayLength);
    
			//Get the Screen positions of the object
			Vector2 positionOnScreen = new Vector2(transform.position.x, transform.position.z);
		
			//Get the Screen position of the mouse
			Vector2 mouseOnScreen = new Vector2(mouseWorldPoint.x, mouseWorldPoint.z);

			//Get the angle between the points
			float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

			//Ta Daaa
			transform.rotation =  Quaternion.Euler (new Vector3(0f,90f-angle,0f));
		}
	}

	float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}
	
}
