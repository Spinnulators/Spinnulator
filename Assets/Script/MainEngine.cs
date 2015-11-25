using UnityEngine;
using System.Collections;

public class MainEngine : MonoBehaviour {
    private GameObject foot;
    private GameObject shoulder;
    private bool centered = false;
    private Vector3 shoulderInitPosition;
    private float shoulderMovement;
    private Vector3 footPosition;
    private Vector3 oldFootPosition=new Vector3(0f,0f,0f);
    private GameObject distance;
    private Vector3 velocity;
    private Vector3 velocity2=new Vector3(0f,0f,0f);
    private Vector3 velocity3=new Vector3(0f,0f,0f);
    private Vector3 velocity4 = new Vector3(0f, 0f, 0f);
    private Vector3 velocity5 = new Vector3(0f, 0f, 0f);
    private Vector3 cameraMovementX = new Vector3(1f, 0f, 0f);
    //private Vector3 cameraMovementY = new Vector3(0f, 1f, 0f);
    private Vector3 cameraMovementZ = new Vector3(0f, 0f, 1f);
    private GameObject bikeCamera;

    // Use this for initialization
    void Start () {
        if (GameObject.Find("Bike Camera"))
        {
            bikeCamera = GameObject.Find("Bike Camera");
            Debug.Log("Camera found");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("AnkleLeft"))
        {
            foot = GameObject.Find("AnkleLeft");
            //Tar in de senaste 5 hastigheterna och interpolerar mellan dem.
            velocity5 = velocity4;
            velocity4 = velocity3;
            velocity3 = velocity2;
            velocity2 = velocity;
            //Här sker själva interpolationen
            var tmp = (foot.transform.position - oldFootPosition) / Time.deltaTime;
            //Tilldelar variabeln som ger förra positionen (så att vi kan få skillnaden i avstånd)
            oldFootPosition = foot.transform.position;
            velocity = (tmp + velocity2 + velocity3 + velocity4 + velocity5) / 5;
            //Om hastigheten håller sig inom rimliga ramar så ska den överföras till spelet
            if (velocity.magnitude > 1 && velocity.magnitude < 15)
            {
                bikeCamera.transform.position += bikeCamera.transform.forward * velocity.magnitude * 0.1f;
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
                if (Mathf.Abs(shoulderMovement)>0.2)
                {
                    //om du lutar dig utanför deadzone så roteras kameran
                    Vector3 rotation = new Vector3(0f, shoulderMovement * 0.12f*velocity.magnitude, 0f);
                    bikeCamera.transform.Rotate(rotation);
                    Debug.Log(shoulderMovement);
                }
                
            }

        }
    }
}
