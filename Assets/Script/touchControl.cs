using UnityEngine;
using System.Collections;

public class touchControl : MonoBehaviour {

	public touchPad moveTouchPad;
	public float thrusterForce = 4.0f;
//	public Vector2 movement = Vector2.zero;

	private Transform thisTransform;
//	Vector2 velocity;

	// Use this for initialization
	void Start () {

		// Cache component look up at startup
		thisTransform = this.transform;

		//Move chacter to spawnpoint
		//TODO: 
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 movement = Vector2.zero;

		if(moveTouchPad.position.x != 0){
			movement = Vector2.right * thrusterForce * moveTouchPad.position.x;
		}else if(moveTouchPad.position.x == 0 && movement.x > 0){
			movement.x = Mathf.Clamp(movement.x - 0.1f ,0,1);
		}else if(moveTouchPad.position.x == 0 && movement.x > 0){
			movement.x = Mathf.Clamp(movement.x + 0.2f ,-1,0);
		}

		if(moveTouchPad.position.y >0){
			movement = Vector2.up * thrusterForce * moveTouchPad.position.y;
		}
		//Check grounded

		//Apply gravity while on the air
		// velocity.y += Physics2D.gravity.y*Time.deltaTime;
		// or
		// velocity.y += Physics2D.gravity.y;
		// movement.x *= inAirMultiplier;

		movement += Physics2D.gravity;

		Debug.Log("Mov x:"+movement.x+" y:"+movement.y);

		rigidbody2D.velocity = new Vector2(movement.x, movement.y);
	}
}
