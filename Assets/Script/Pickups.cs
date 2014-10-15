using UnityEngine;
using System.Collections;

public class Pickups : MonoBehaviour {

	public void taken(){
		Destroy (this.gameObject);
	}
}
