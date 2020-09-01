using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SBInteractionArea : MonoBehaviour
{
    public float unfuelTime = 3f;
    public static int countMS = 1;
    public Color emptyAreaColor = Color.white;
    public Color fuelingColor = Color.yellow;
    public Color fueledColor = Color.green;

    float elapsedTime = 0f;
    bool isFueled = false;

    static int unfueledMS = 0;
    static bool isAllMSUnfueled = false;

    SpriteRenderer spriteRenderer;

    void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public static void MSUnfueled() {
        unfueledMS++;
        ActivateInteractionArea();
	}

    static void ActivateInteractionArea() {
        if (unfueledMS >= countMS) {
            isAllMSUnfueled = true;
		}
	}

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            if (!isFueled && isAllMSUnfueled) {
                spriteRenderer.color = fuelingColor;
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Player")) {
            if (!isFueled && isAllMSUnfueled) {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > unfuelTime) {
                    isFueled = true;
                    spriteRenderer.color = fueledColor;
                    SessionManager.SBFueld();

                    FindObjectOfType<AudioManager>().Play("Complete");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            if (!isFueled) {
                spriteRenderer.color = emptyAreaColor;
                elapsedTime = 0f;
            }
        }
    }
}
