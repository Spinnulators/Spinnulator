using UnityEngine;
using System.Collections;

public class SpeedShader : MonoBehaviour {
	
	// We attach our custom shader to a material
	public Material material;
	public PlayerController playerController;

	private float minMomentumTreshold = 27.0f;
	private float maxMomentumTreshold = 40.0f;
	private float momentum = 0.0f;

	public void Update() {
		float newMomentum = playerController.momentum;

		// Scale momentum according to tresholds
		newMomentum = (newMomentum - minMomentumTreshold) / maxMomentumTreshold;

		// Clamp between 0 to 1
		newMomentum = Mathf.Clamp01 (newMomentum);

		// Change momentum only by 10%
		momentum += (newMomentum - momentum) * 0.1f;
	}
	
	// And then we use that material on the texture, before the final image is displayed
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat("_Momentum", momentum);
		Graphics.Blit (source, destination, material);
	}
}