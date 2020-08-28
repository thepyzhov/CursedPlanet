using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MSInteractionArea : MonoBehaviour
{
    public int medGelAmount = 100;

    SpriteRenderer spriteRenderer;
    Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.color = Color.green;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.color = baseColor;
        }
    }
}
