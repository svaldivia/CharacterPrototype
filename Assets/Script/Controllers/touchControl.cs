using UnityEngine;
using System.Collections;

public class touchControl : MonoBehaviour {

	public touchPad moveTouchPad;
	public float thrusterForce = 4.0f;
	public float fuelConsumption;
	public GameObject explosion;
	public bool dead = false;
	public float harmRadius = 0.5f;

	private Transform thisTransform;

	// Use this for initialization
	void Start () {

		// Cache component look up at startup
		thisTransform = this.transform;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		// Movement vector
		Vector2 movement = Vector2.zero;

		// Reset fuel
		fuelConsumption = 0.0f;

		// Check for joystick movement
		if(moveTouchPad.position.y >0 || (moveTouchPad.position.x != 0 && moveTouchPad.position.y == 0))
		{
			// Add movement
			movement = thrusterForce * moveTouchPad.position;

			// Update fuel
			fuelConsumption = movement.magnitude;
			
		}

		//Add force
		rigidbody2D.AddForce(movement);




		
	}
}
