using UnityEngine;
using System.Collections;

public class CharacterFollow : MonoBehaviour {

	public Transform character;
	public float yOffset = 0.3f;
	
	// Update is called once per frame
	void Update () {
		if(!GameObject.Find("Main Camera").GetComponent<CameraFollow>().playerDead){
			this.transform.localPosition = new Vector3(character.localPosition.x,character.localPosition.y - yOffset);
		}
	}
}
