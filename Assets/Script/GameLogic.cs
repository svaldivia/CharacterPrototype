using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	public UISlider fuelUI;

	//Private
	private RustyModel player;
	private bool playerDied = false;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Rusty_Sprite").GetComponent<RustyModel>();
	}
	
	// Update is called once per frame
	void Update () {

		fuelUI.sliderValue = player.fuel/player.FUEL_BASE;
		

		// if(fuelUI.sliderValue > 0){
			//Update fuel UI as Rusty moves
			// fuelUI.sliderValue = player.fuel/player.FUEL_BASE;
		// }else if (!playerDied){
		// 	//Player dies
		// 	playerDied = true;
		// 	player.dead = true;
		// 	player.die();
		// }


	}
}
