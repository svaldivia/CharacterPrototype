using UnityEngine;
using System.Collections;

public class touchPad : MonoBehaviour {

	class Boundary{
		public Vector2 max = Vector2.zero;
		public Vector2 min = Vector2.zero;
	}

	public Rect touchZone;
	public Vector2 position;

	private int lastFingerId = -1;
	private Vector2 fingerDownPos;
	private float fingerDownTime;

	// Tap
	private int tapCount;
	private float tapTimeWindow;
	private float firstDeltaTime = 0.5f;

	//Touch Mode
	public bool touchPadMode;
	private GUITexture gui;
	private Rect defaultRect;
	private Boundary guiBoundary = new Boundary();
	private Vector2 guiTouchOffset;
	private Vector2 guiCenter;

	//Dead Zone
	private Vector2 deadZone = new Vector2(0,0); //Control when position is output
	bool normalize = false;

    void OnEnable(){

		gui = this.GetComponent<GUITexture>();

		defaultRect = gui.pixelInset; 

		defaultRect.x += transform.position.x * Screen.width;
		defaultRect.y += transform.position.y * Screen.height;

		transform.position = new Vector2(0,0);

		if( touchPadMode){
			if(gui.texture)
			touchZone = defaultRect;	
		}else{

			// Offset for touch input to match with the top left of the GUi
			guiTouchOffset.x = defaultRect.width*0.5f;
			guiTouchOffset.y = defaultRect.height*0.5f;

			//Cache center of the GUI
			guiCenter.x = defaultRect.x + guiTouchOffset.x;
			guiCenter.y = defaultRect.y + guiTouchOffset.y;

			// GUI Boundary
			guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
			guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
			guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
			guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
		}
	}

	void Update(){

		int count = Input.touchCount;

		if (count == 0){
			resetTouch();

		}else{

			// Go through all finger touches
			for (int i = 0; i < count; i++){

				Touch touch = Input.GetTouch(i);
				Vector2 guiTouchPos = touch.position - guiTouchOffset;

				bool shouldLatchFinger = false;
				if(touchPadMode){
					if(touchZone.Contains(touch.position)){
						shouldLatchFinger = true;
					}
				}else if (gui.HitTest(touch.position)){
					shouldLatchFinger = true;
				}
				

				//Latch finger if new touch
				if( shouldLatchFinger && (lastFingerId == -1 || lastFingerId != touch.fingerId)){


					if(touchPadMode){
						lastFingerId = touch.fingerId;
						fingerDownPos = touch.position;
						fingerDownTime = Time.time;
					}
					

					lastFingerId = touch.fingerId;

//					Debug.Log("FingerId After: "+lastFingerId);


				}

				//Check finger Ids, to verify if it is still touching
				if (lastFingerId == touch.fingerId)	{

					//Taps?

					if ( touchPadMode ){
						position.x = Mathf.Clamp( ( touch.position.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
						position.y = Mathf.Clamp( ( touch.position.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );
					}else{
						// Change the location of the GUI to where the touch is
						Rect tempRect = gui.pixelInset;
						tempRect.x = Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
						tempRect.y = Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );
						gui.pixelInset = tempRect;

					}
					
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled){
						resetTouch();
					}
				}
			}	
		}

		if( !touchPadMode){
			// Get a value between -1 and 1 based on the joystick graphic location
			position.x = ( gui.pixelInset.x + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
			position.y = ( gui.pixelInset.y + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
		}

		//Dead Zone
		float absoluteX = Mathf.Abs(position.x);
		float absoluteY = Mathf.Abs(position.y);

		if(absoluteX < deadZone.x){
			// Report the joystick as being at the center if it is within the dead zone
			position.x = 0;
		}else if(normalize){
			//Rescale the output after taking the deadzone into account
			position.x = Mathf.Sign(position.x)*(absoluteX - deadZone.x)/(1 - deadZone.x );
		}

		if(absoluteY < deadZone.y){
			//Report the joystick as being at the center if it is within the dead zone
			position.y = 0;
		}else if(normalize){
			// Rescale the output after taking the dead zone into account
			position.y = Mathf.Sign(position.y)*(absoluteY - deadZone.y)/(1 - deadZone.y );
		}
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