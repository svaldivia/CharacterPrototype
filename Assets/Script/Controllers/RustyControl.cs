using UnityEngine;
using System.Collections;

/*
 *
 *
 */

 //TODO: Grounded

public class RustyControl : MonoBehaviour {

	//---------Public----------
	public GameObject explosion;
	public float FUEL_FACTOR = 100.0f;
	// Ground check
	float groundRadius = 0.2f;
	public bool grounded = false;
	public Transform groundCheck;
//	public LayerMask whatIsGround;
	// Death
	public float fuelConsumption;
	public bool dead = false;
	public float harmRadius = 0.5f;
	public LayerMask whatKillsPlayer;

	//-----Private------------
	private touchControl touchController;
	private RustyModel playerModel;
	private bool playerDied = false;
	private bool waitingToReload = false;

	// Use this for initialization
	void Start () {

		// Get touch controller
		touchController = this.GetComponent<touchControl>();
		// Get player model
		playerModel = this.GetComponent<RustyModel>();
	}
	
	// Update is called once per frame
	void Update () {
		


		//=====================================
		// Check if player is dead
		//=====================================

		//Check if collided with dangerous things!
		dead = Physics2D.OverlapCircle(this.transform.position,harmRadius,whatKillsPlayer);
		if(dead && !waitingToReload){
			die ();
		}

		// Out of fuel?
		if(playerModel.fuel > 0){
			playerModel.fuel -= touchController.fuelConsumption/FUEL_FACTOR;	
		}else{
			// Player died ======
			if(!playerDied){
				die();
				playerDied = true;
			}

			//TODO: Lives handling
			// Take a life
			playerModel.lives--;

			//Check player lives
			if(playerModel.lives > 0){
				//Reset game - from checkpoint
			}else{
				//Player lost
			}

		}
	}

	//---------------Helper Functions-------------

	//Load level after 3 seconds
	IEnumerator LoadAfterDelay(string levelName){
		yield return new WaitForSeconds(3);
		Application.LoadLevel(levelName);
	}

	// Kill player
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
