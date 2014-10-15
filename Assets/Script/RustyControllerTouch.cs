using UnityEngine;
using System.Collections;

public class RustyControllerTouch : MonoBehaviour {
	
	//Public variables
	public float maxSpeed = 10.0f;
	public GameObject explosion;
	public bool dead = false;
	public float harmRadius = 0.5f;
	
	//Private variables
	bool facingRight = true;
	
	Animator anim;
	
	// Grounded state to determine if the character is on the grounded
	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public LayerMask whatKillsPlayer;
	bool waitingToReload = false;
	public float fuelConsumption;

	// Touch
	private int lastFingerId = -1;

	// Activate GUI for test values
	private float yMove;
	private float xMove;
	
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();

		Mathf.Clamp(xMove,-1.0f,1.0f);
	}
	
	// Test touch input variables
	public float minSwipeDistY;
	public float minSwipeDistX;
	private Vector2 startPos;
	
	void FixedUpdate(){
		
		//Touch Input ----------------------------
		
		if (Input.touchCount > 0){

			Touch touch = Input.touches[0];

			switch (touch.phase){

			case TouchPhase.Began:
				startPos = touch.position;
				break;
				
			case TouchPhase.Ended:
				float swipeDistVertical = (new Vector3(0,touch.position.y,0) - new Vector3(0,startPos.y,0)).magnitude;
				
				if(swipeDistVertical > minSwipeDistY){
					float swipeValue = Mathf.Sign(touch.position.y - startPos.y);
					if(swipeValue > 0){ //up swipe
						Debug.Log("Up");
						rigidbody2D.AddForce(new Vector2(0,20.0f),ForceMode2D.Impulse);
					}
					else if (swipeValue <0) //down swipe
						Debug.Log("Down");
				}
				
				float swipeDistHorizontal = (new Vector3(touch.position.x,0,0) - new Vector3(startPos.x,0,0)).magnitude;
				
				if(swipeDistHorizontal > minSwipeDistX){
					float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
					if(swipeValue > 0){ //right swipe
						Debug.Log("Right");
						rigidbody2D.AddForce(new Vector2(5.0f,0),ForceMode2D.Impulse);
					}else if (swipeValue <0){ //left swipe
						Debug.Log("Left");
						rigidbody2D.AddForce(new Vector2(-5.0f,0),ForceMode2D.Impulse);
					}
				}
				break;
			}
		}else{	
			resetTouch();
		}

		
		//----------------------------------------

//		Debug.Log ("xMove: "+ xMove);


		//Character movement
//		xMove = Input.GetAxis("Horizontal");
//		yMove = Input.GetAxis("Vertical");
		
		if(yMove >=0){
			fuelConsumption = Mathf.Abs(xMove) + Mathf.Abs (yMove);
		}
		
		//Constantly update if the character is on the ground
		grounded = Physics2D.OverlapCircle(groundCheck.position,groundRadius,whatIsGround);
		//^^Analyzes the overlapping with a collider
		anim.SetBool("Ground",grounded);
		anim.SetFloat("vSpeed",Mathf.Abs (yMove)); //How fast we are moving vertically
		anim.SetFloat("hSpeed",Mathf.Abs (xMove));
		
		// Ensure user can only go up
//		if( yMove > 0){
//			rigidbody2D.velocity = new Vector2(xMove*maxSpeed, yMove*maxSpeed);
//		}else if (yMove == 0){
//			rigidbody2D.velocity = new Vector2(xMove*maxSpeed, rigidbody2D.velocity.y);
//		}
//		
		// Flip sprites
		
		if (xMove >0 && !facingRight){
			Flip ();
		}else if (xMove < 0 && facingRight){
			Flip ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Check if dead
		dead = Physics2D.OverlapCircle(this.transform.position,harmRadius,whatKillsPlayer);
		if(dead && !waitingToReload){
			die ();
		}
	}
	
	IEnumerator LoadAfterDelay(string levelName){
		yield return new WaitForSeconds(3);
		Application.LoadLevel(levelName);
	}
	
	// --------Touch Controls -------------
	private bool isFingerDown(){
		return lastFingerId != -1;
	}

	private void resetTouch(){
		lastFingerId = -1;
	}
	// --------Touch Controls -------------


	void Flip(){
		facingRight = !facingRight;
		Vector2 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
	
	public void die(){
		waitingToReload =true;
		Instantiate(explosion,this.transform.position,this.transform.rotation);
		//Stop camera tracking
		GameObject.Find("Main Camera").GetComponent<CameraFollow>().playerDead = true;
		//Kill player
		renderer.enabled = false;
		//Reload the scene
		StartCoroutine(LoadAfterDelay(Application.loadedLevelName));
	}
}
