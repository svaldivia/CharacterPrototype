using UnityEngine;
using System.Collections;

public class GameLogic : MonoBehaviour {

	public UISlider fuelUI;

	//Private
	private RustyController player;
	private float FUEL_BASE = 1000.0f;
	private bool playerDied = false;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Rusty_Sprite").GetComponent<RustyController>();
	}
	
	// Update is called once per frame
	void Update () {
		if(fuelUI.sliderValue > 0){
			//Update fuel UI as Rusty moves
			fuelUI.sliderValue -= (player.fuelConsumption)/FUEL_BASE;
		}else if (!playerDied){
			//Player dies
			playerDied = true;
			player.dead = true;
			player.die();
		}


	}
}
