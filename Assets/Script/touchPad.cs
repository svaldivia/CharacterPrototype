using UnityEngine;
using System.Collections;

public class touchPad : MonoBehaviour {

	public Rect touchZone;
	public Vector2 position;

	private int lastFingerId = -1;
	private GUITexture gui;
	private Vector2 fingerDownPos;
	private float fingerDownTime;
	private Rect defaultRect;

	// Tap
	private int tapCount;
	private float tapTimeWindow;
	private float firstDeltaTime = 0.5f;

	void Start(){

		gui = this.GetComponent<GUITexture>();

		defaultRect = gui.pixelInset; 

		defaultRect.x += transform.position.x * Screen.width;
		defaultRect.y += transform.position.y * Screen.height;

		Debug.Log("x: "+defaultRect.max.x);
		Debug.Log("y: "+defaultRect.y);
		transform.position = new Vector2(0,0);

		touchZone = defaultRect;


	}

	void FixedUpdate(){

		int count = Input.touchCount;

		if (count == 0){
			resetTouch();

		}else{

			// Go through all finger touches
			for (int i = 0; i < count; i++){

				Touch touch = Input.GetTouch(i);
				Vector2 guiTouchPos = touch.position;

				bool shouldLatchFinger = false;

				if(touchZone.Contains(touch.position)){
					shouldLatchFinger = true;
				}

				//Latch finger if new touch
				if( shouldLatchFinger && (lastFingerId == -1 || lastFingerId != touch.fingerId)){

					lastFingerId = touch.fingerId;
					fingerDownPos = touch.position;
					fingerDownTime = Time.time;

					lastFingerId = touch.fingerId;

					Debug.Log("FingerId After: "+lastFingerId);


				}

				//Check finger Ids, to verify if it is still touching
				if (lastFingerId == touch.fingerId)	{

					//Taps?

					position.x = Mathf.Clamp( ( touch.position.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
					position.y = Mathf.Clamp( ( touch.position.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );

					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled){
						resetTouch();
					}
				}
			}	
		}

		//Dead Zone ??
	}




	private bool isFingerDown(){
		return lastFingerId != -1;
	}

	private void resetTouch(){
		lastFingerId = -1;
		gui.pixelInset = defaultRect;
		position = Vector2.zero;
		fingerDownPos = Vector2.zero;
	}

}