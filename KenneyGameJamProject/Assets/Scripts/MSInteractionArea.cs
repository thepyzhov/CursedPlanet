using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSInteractionArea : MonoBehaviour
{
    public float refuelTime = 3f;
    float elapsedTime = 0f;
    bool hasFuel = true;

    public Color emptyAreaColor = Color.white;
    public Color fuelingColor = Color.yellow;
    public Color fueldColor = Color.green;

    SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasFuel) {
                spriteRenderer.color = fuelingColor;
            }
        }
    }

	private void OnTriggerStay(Collider other) {
		if (other.CompareTag("Player")) {
            if (hasFuel) {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > refuelTime) {
                    hasFuel = false;
                    spriteRenderer.color = fueldColor;
                    SBInteractionArea.MSUnfueled();
                }
            }
		}
	}

	private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (hasFuel) {
                spriteRenderer.color = emptyAreaColor;
                elapsedTime = 0f;
            }
        }
    }
}
