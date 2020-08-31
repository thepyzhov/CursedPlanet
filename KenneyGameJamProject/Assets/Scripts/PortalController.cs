using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
	public SpriteRenderer[] activationDots;

	public Color inactiveColor = Color.red;
	public Color activeColor = Color.green;

	bool isActive = false;

	private void Awake() {
		ChangeActivationDotsColor(inactiveColor);
	}

	public void ActivePortal() {
		isActive = true;

		ChangeActivationDotsColor(activeColor);
	}

	private void OnTriggerEnter(Collider other) {
		if (isActive) {
			if (other.CompareTag("Player")) {
				SceneManager.LoadScene(0);
			}
		}
	}

	private void ChangeActivationDotsColor(Color newColor) {
		foreach (SpriteRenderer dot in activationDots) {
			dot.color = newColor;
		}
	}
}
