using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collectable : MonoBehaviour {
	
	public Text counterText;
	private int countSum=0;
	private int Apple=1;
    public AudioClip CollectSound;
    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = CollectSound;
    }

	void Update(){
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Apple")) {;
            audioSource.Play();
			//col.gameObject.SetActive(false);
			Destroy (col.gameObject);
			countSum +=Apple;
			counterText.text=countSum.ToString()+ "";
			//Destroy (this.gameObject);
		}
	}
}
