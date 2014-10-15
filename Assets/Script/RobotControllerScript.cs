using UnityEngine;
using System.Collections;

public class RobotControllerScript : MonoBehaviour {


	public float maxSpeed = 10.0f; //Maximum speed for character
	bool facingRight = true;	//Check where the character is facing
	public float jumpForce = 700.0f;

	//Used our double jump
	bool doubleJump;

	Animator anim;

	// Grounded state to determine if the character is on the grounded
	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
	
		anim = GetComponent<Animator>();
	}

	// Animator is in sync with fixed update
	// Do physics stuff here :),  do not need to use time.deltaTime stuff
	void FixedUpdate () {

		//Initialize double jump
		if(grounded){
			doubleJump = false;
		}

		//Constantly update if the character is on the ground
		grounded = Physics2D.OverlapCircle(groundCheck.position,groundRadius,whatIsGround);
					//^^Analyzes the overlapping with a collider
		anim.SetBool("Ground",grounded);
		anim.SetFloat("vSpeed",rigidbody2D.velocity.y); //How fast we are moving vertically


		float move = Input.GetAxis("Horizontal"); //How the character is going to move

		anim.SetFloat("Speed",Mathf.Abs (move));

		rigidbody2D.velocity = new Vector2(move*maxSpeed, rigidbody2D.velocity.y);
	
		if (move >0 && !facingRight){
			Flip ();
		}else if (move < 0 && facingRight){
			Flip ();
		}
	}

	//Input is read more accurately
	void Update(){

		// Do no use keycode, set mapping in the input menu, it is a better practice
		if((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space)){
			anim.SetBool("Ground",false);
			rigidbody2D.AddForce(new Vector2(0,jumpForce));

			if(!doubleJump && !grounded){
				doubleJump = true;
			}
		}
	}

	void Flip(){
		facingRight = !facingRight;
		Vector2 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
