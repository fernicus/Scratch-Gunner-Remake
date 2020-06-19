using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(0f,0f,0f));
    }

    // This code obtained from user BenZed at https://answers.unity.com/questions/855976/make-a-player-model-rotate-towards-mouse-location.html
    void Update()
    {
		//Get the Screen positions of the object
		Vector2 positionOnScreen = Camera.main.WorldToViewportPoint (transform.position);
		
		//Get the Screen position of the mouse
		Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

		//Get the angle between the points
		float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

		//Ta Daaa
		transform.rotation =  Quaternion.Euler (new Vector3(0f,90f-angle,0f));
	}

	float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}
	
}
