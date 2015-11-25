using UnityEngine;
using System.Collections;

public class MainEngine : MonoBehaviour {
    private GameObject foot;
    private GameObject shoulder;
	private GameObject bikeCamera;

    private bool centered = false;
    
	private Vector3 shoulderInitPosition;
    private float shoulderMovement;
	private float shoulderMovementDeadZone = 0.2f;
    private Vector3 footPosition;

	private MotionData motionData;
	public int motionDataSize = 200;
	private float velocityMinTreshold = 1f;
	private float velocityMaxTreshold = 15f;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Bike Camera"))
        {
            bikeCamera = GameObject.Find("Bike Camera");
            Debug.Log("Camera found");
        }

		motionData = new MotionData (motionDataSize);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameObject.Find("AnkleLeft"))
        {
            foot = GameObject.Find("AnkleLeft");

            motionData.add(foot.transform.position.magnitude);

			float averageVelocity = motionData.getAverage();
		            
			//Om hastigheten håller sig inom rimliga ramar så ska den överföras till spelet
			if (averageVelocity > velocityMinTreshold && averageVelocity < velocityMaxTreshold)
            {
				float movementScaleFactor = 0.1f;
				bikeCamera.transform.position += bikeCamera.transform.forward * averageVelocity;
            }
        }
        if (GameObject.Find("ShoulderLeft"))
        {
            shoulder = GameObject.Find("ShoulderLeft");
            if (centered == false)
            {
                //Skapar en ursprungsposition varifrån alla andra positioner beror.
                shoulderInitPosition = shoulder.transform.position;
                centered = true;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                //Resettar ursprungspositionen om det känns weird
                shoulderInitPosition = shoulder.transform.position;
                centered = true;
            }
            if (centered == true)
            {
                //Tar ut hur mycket du lutat dig i x-led
                shoulderMovement=shoulder.transform.position.x-shoulderInitPosition.x;
                if (Mathf.Abs(shoulderMovement) > shoulderMovementDeadZone)
                {
					float rotationScalingFactor = motionData.getAverage() * 0.12f;

                    //om du lutar dig utanför deadzone så roteras kameran
                    Vector3 rotation = new Vector3(0f, shoulderMovement, 0f);

                    bikeCamera.transform.Rotate(rotation * rotationScalingFactor);
                    Debug.Log(shoulderMovement);
                }
                
            }
        }
    }
}
