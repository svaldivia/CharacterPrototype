using UnityEngine;
using System.Collections;

public class RustyController : MonoBehaviour {

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
	public LayerMask pickups;
	bool waitingToReload = false;
	public float fuelConsumption;

	// Activate GUI for test values
	public bool activateDevBox = false;
	private float yMove;
	private float xMove;


	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	void FixedUpdate(){
		//Character movement
		xMove = Input.GetAxis("Horizontal");
		yMove = Input.GetAxis("Vertical");

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
		if( yMove > 0){
			rigidbody2D.velocity = new Vector2(xMove*maxSpeed, yMove*maxSpeed);
		}else if (yMove == 0){
			rigidbody2D.velocity = new Vector2(xMove*maxSpeed, rigidbody2D.velocity.y);
		}

		// Flip sprites

		if (xMove >0 && !facingRight){
			Flip ();
		}else if (xMove < 0 && facingRight){
			Flip ();
		}
	}
	
	// Update is called once per frame
	void Update () {

		// Check if dead
		dead = Physics2D.OverlapCircle(this.transform.position,harmRadius,whatKillsPlayer);
		if(dead && !waitingToReload){
			die ();
		}

		// Check if batteries are picked up
		if(Physics2D.OverlapCircle(this.transform.position,harmRadius,pickups)){
			Collider2D collider = Physics2D.OverlapCircle(this.transform.position,harmRadius,pickups);
			collider.gameObject.GetComponent<Pickups>().taken();
			fuelConsumption = -100.0f;
		}
	}

	void OnGUI(){
		if (activateDevBox){
			GUI.Box(new Rect(0,0,200,100), "Dev info");
			GUI.Label(new Rect(10,15,100,20),"Y Move: "+yMove);
			GUI.Label(new Rect(10,30,100,20),"X Move: "+xMove);


		}
	}

	IEnumerator LoadAfterDelay(string levelName){
		yield return new WaitForSeconds(3);
		Application.LoadLevel(levelName);
	}


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
