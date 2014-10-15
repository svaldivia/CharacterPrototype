using UnityEngine;
using System.Collections;

public class ThrusterController : MonoBehaviour {

	//Private variables
	private Animator anim;
	private Vector2 theOriginalScale;
	
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		theOriginalScale = transform.localScale;
	}
	
	void FixedUpdate(){

		if(GameObject.Find("Rusty_Sprite").GetComponent<RustyController>().dead){
			anim.SetBool("Dead",true);
		}

		//Character movement
		float xMove = Input.GetAxis("Horizontal");
		float yMove = Input.GetAxis("Vertical");

		if( yMove >=0){
			anim.SetFloat("Speed",Mathf.Abs (xMove)+Mathf.Abs (yMove));
		}

		// Check player movement for thruster position
		if (xMove > 0){
			Flip ('R');
		}else if(xMove < 0){
			Flip ('L');
		}else if(yMove > 0){
			Flip ('U');
		}
		if(yMove > 0 && xMove > 0 ){
			Flip ('C');
		}else if(yMove > 0 && xMove < 0 ){
			Flip ('Z');
		}
	}
	
	void Flip(char direction){

		transform.localScale = theOriginalScale;
		Quaternion theRotation = new Quaternion(0,0,1,0);;
		//Right
		if(direction == 'R'){
			//Original - do nothing
		}
		//Left
		else if(direction == 'L'){
			//flip
			theRotation = new Quaternion(-1,0,0,0);
		}
		//Up
		else if ( direction == 'U'){
			//rotate
			theRotation = new Quaternion(1,-1,0,0);
			transform.localRotation = theRotation;
		}
		//Up - Right
		else if ( direction == 'C'){
			//rotate
			theRotation = new Quaternion(1,-1.5f,0,0);
			transform.localRotation = theRotation;
		}
		//Up - Left
		else if ( direction == 'Z'){
			//rotate
			theRotation = new Quaternion(-1,0.5f,0,0);
			transform.localRotation = theRotation;
		}

		transform.localRotation = theRotation;
	}
}
