using UnityEngine;
using System.Collections;
using Kinect = Windows.Kinect;
using System.Collections.Generic;

public class KinectInterface : MonoBehaviour {

    public GameObject bodySourceGameObject;
    private BodySourceManager bodySourceManager;

    private GameObject bikeSeat;
    private GameObject closestPlayer;

    private Vector3 prevFootPosition = new Vector3(0f, 0f, 0f);

	private KeyboardInterface keyboardController;
	private MotionData ankleVelocityMotionData;
    private Kinect.Body[] bodies;

    // The maximum distance the player is allowed to be away from the bike
    private float playerDistanceCutoff = 5.0f;
    private int motionDataSize = 20;

    private int numTrackedPlayers;
    private int numTrackedClosePlayers;

    void Start() {
        bikeSeat = GameObject.FindGameObjectWithTag("BikeSeat");
        ankleVelocityMotionData = new MotionData(motionDataSize);
    }

    void FixedUpdate() {

        bodySourceGameObject = GameObject.Find("BodyManager");

        if (bodySourceGameObject == null) {
            Debug.Log("Couldn't find body manager object");
            return;
        }

        bodySourceManager = bodySourceGameObject.GetComponent<BodySourceManager>();
        if (bodySourceManager == null) {
            Debug.Log("Couldn't find BodySourceManager");
            return;
        }

        bodies = bodySourceManager.GetData();
        
        if (bodies == null) {
            return;
        }

        updateClosestPlayer();
    }

    private Kinect.Body[] getBodies() {
        return bodies;
    }

    public bool isTracking() {
        return (closestPlayer != null && getChildGameObject(closestPlayer, "SpineBase") != null && 
            playerDistanceFromBike(closestPlayer) < playerDistanceCutoff);
    }

    private float playerDistanceFromBike(GameObject bodyObject) {
        GameObject spineBase = getChildGameObject(bodyObject, "SpineBase");

        float distance = Vector3.Distance(spineBase.transform.position, bikeSeat.transform.position);
        return distance;
    }

    private void updateClosestPlayer() {

        float closestDistance = 999f;
        GameObject closestPlayerTemp = null;

        int numTrackedPlayersTemp = 0;
        int numTrackedClosePlayersTemp = 0;

        foreach(var body in getBodies()) {
            if (body.IsTracked){
                GameObject bodyObject = GameObject.Find("Body:" + body.TrackingId);

                float distance = playerDistanceFromBike(bodyObject);

                // Checks if this is the closest player, and if the player is close enough
                if (distance < playerDistanceCutoff) {

                    numTrackedClosePlayersTemp++;

                    if(distance < closestDistance) {
                        numTrackedClosePlayersTemp++;
                        closestPlayerTemp = bodyObject;
                    }
                }
            }
        }

        closestPlayer = closestPlayerTemp;

        numTrackedClosePlayers = numTrackedClosePlayersTemp;
        numTrackedPlayers = numTrackedPlayersTemp;
    }

    public int getNumTrackedPlayers() {
        return numTrackedPlayers;
    }

    public int getNumTrackedClosePlayers() {
        return numTrackedClosePlayers;
    }

    public bool tooManyClosePlayers() {
        return (getNumTrackedClosePlayers() > 1 || getNumTrackedPlayers() > 4);
    }
	
	private Vector3 getAnkleRightPosition() {
        return getChildGameObject(closestPlayer, "AnkleRight").transform.position;
	}
	
	private Vector3 getSpineTopPosition() {
		return getChildGameObject(closestPlayer, "SpineShoulder").transform.position;
	}

    private Vector3 getSpineBasePosition() {
        return getChildGameObject(closestPlayer, "SpineBase").transform.position;
    }

    private Vector3 getHipLeftPosition() {
        return getChildGameObject(closestPlayer, "HipLeft").transform.position;
    }

    private Vector3 getHipRightPosition() {
        return getChildGameObject(closestPlayer, "HipRight").transform.position;
    }

    private Vector3 getHandLeftPosition() {
        return getChildGameObject(closestPlayer, "HandLeft").transform.position;
    }

    private Vector3 getHandRightPosition() {
        return getChildGameObject(closestPlayer, "HandRight").transform.position;
    }

    private Vector3 getKneeLeftPosition() {
        return getChildGameObject(closestPlayer, "KneeLeft").transform.position;
    }

    private Vector3 getKneeRightPosition() {
        return getChildGameObject(closestPlayer, "KneeRight").transform.position;
    }

	// Updates the ankle velocity every frame
	private void updateAnkleVelocity() {
		if (isTracking()) {
			Vector3 footPosition = getAnkleRightPosition ();
			
			Vector3 distance = footPosition - prevFootPosition;

            float dist = distance.magnitude*6f;

            if (dist < 24f) {
                ankleVelocityMotionData.add(dist);
            }
			
			prevFootPosition = footPosition;
		}
	}

	// Get interpolated ankle velocity
	public float getAnkleVelocity () {
        //return distance;
        updateAnkleVelocity();
        return ankleVelocityMotionData.getAverage ();
	}

	// Computes lean in x axis
	public float getHorizontalLean () {
        if (isTracking()) {

            Vector3 spineBase = getSpineBasePosition();
            Vector3 spineTop = getSpineTopPosition();
            Vector3 spineVector = spineTop - spineBase;

            Vector3 hipRight = getHipRightPosition();

            Vector3 hipVector = hipRight - spineBase;
            hipVector.y = 0;

            hipVector.Normalize();

            float lean = Vector3.Dot(spineVector, hipVector);

            
            Debug.Log(lean);

            return lean;
        } else {
            return 0f;
        }
	}

    private GameObject getChildGameObject(GameObject fromGameObject, string withName) {

        Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>();
        
        foreach (Transform t in ts) {
            if (t.gameObject.name == withName) {
                return t.gameObject;
            }
        }

        return null;
    }
}
