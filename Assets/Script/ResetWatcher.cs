using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResetWatcher : MonoBehaviour {

    GameObject[] apples;
    public GameObject player;
    public PlayerController playerController;
    public Collectable collectable;
    public TimeManager timeManager;
    
    public GameObject introPanel;
    public Text introText;

    public GameObject sensorView;

    void Start() {
        apples = GameObject.FindGameObjectsWithTag("Apple");
    }
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKey (KeyCode.Escape) || Input.GetKey(KeyCode.Mouse2)) {
            //Application.LoadLevel (0);
            player.transform.position=new Vector3(-21.28f,20.32f,13.42f);
            player.transform.rotation = Quaternion.identity;
            playerController.momentum = 0;
            collectable.countSum=0;
            
            timeManager.disable();
            
            GameObject.Find("TimeText").GetComponent<Text>().text = "02" + " : " + "00";
            collectable.counterText.text = collectable.countSum.ToString();
            
            if (!sensorView.activeSelf) {
                sensorView.SetActive(true);
            }
            
            if (!introPanel.activeSelf) {
                introPanel.SetActive(true);
                //introText.SetActive(true);    
            }

            introText.text = "Click mouse to start";
            if (playerController.hasStarted) {
                playerController.hasStarted = false;
            }
            if (playerController.hasEnded)
            {
                playerController.hasEnded = false;
            }
            foreach (GameObject apple in apples)
            {
                if (!apple.activeSelf)
                {
                    apple.SetActive(true);
                }
            }
		}
	}
}
