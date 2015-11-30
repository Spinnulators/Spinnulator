using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Collectable : MonoBehaviour {
	
	public Text counterText;
	public int countSum;
	public int coinValue=1;
	void Update(){
		//transform.Rotate (new Vector3 (0, 40, 0) * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Coin")) {;
			//col.gameObject.SetActive(false);
			Destroy (col.gameObject);
			countSum +=coinValue;
			counterText.text=countSum.ToString();
			//Destroy (this.gameObject);
		}
	}
}
