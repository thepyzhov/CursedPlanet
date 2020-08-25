using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float normalSpeed = 25f;
    public float accelerationSpeed = 45f;
    public Transform cameraPosition;
    public Camera mainCamera;
    public Transform spaceshipRoot;
    public float rotationSpeed = 2f;
    public float cameraSmooth = 4f;
    public RectTransform crosshairTexture;

    private float deadZone = 0.1f;

    public float forwardAcceleration = 100f;
    public float backwardAcceleration = 25f;
    private float currThurst = 0.0f;

    public float turnStrength = 10f;
    private float currTurn = 0.0f;

    public LayerMask layerMask;
    public float hoverForce = 9f;
    public float hoverHeight = 2f;
    public GameObject[] hoverPoints;


    private float speed;
    private Rigidbody rb;
    private Quaternion lookRotation;
    private float rotationZ = 0f;
    private float moveXSmooth = 0f;
    private float moveYSmooth = 0f;
    private Vector3 defaultShipRotation;

	private void Start() {
        rb = GetComponent<Rigidbody>();
        layerMask = ~layerMask;
        //r.useGravity = false;
        //lookRotation = transform.rotation;
        //defaultShipRotation = spaceshipRoot.localEulerAngles;
        //rotationZ = defaultShipRotation.z;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
	}

    private void Update() {
        currThurst = 0.0f;
        float accelerationAxis = Input.GetAxis("Vertical");

        if (accelerationAxis > deadZone) {
            currThurst = accelerationAxis * forwardAcceleration;
        } else if (accelerationAxis < -deadZone) {
            currThurst = accelerationAxis * backwardAcceleration;
        }

        currTurn = 0.0f;
        float turnAxis = Input.GetAxis("Horizontal");

        if (Mathf.Abs(turnAxis) > deadZone) {
            currTurn = turnAxis;
        }
    }

    private void FixedUpdate() {
        //      // Press Right Mouse Button to accelerate
        //if (Input.GetMouseButton(1)) {
        //          speed = Mathf.Lerp(speed, accelerationSpeed, Time.deltaTime * 3);
        //} else {
        //          speed = Mathf.Lerp(speed, accelerationSpeed, Time.deltaTime * 3);
        //      }

        //      //Vector3 moveDirection = new Vector3(0, 0, speed);
        //      float moveX = Input.GetAxis("Horizontal");
        //      float moveZ = Input.GetAxis("Vertical");
        //      Vector3 moveDirection = new Vector3(moveX, 0, moveZ);
        //      moveDirection = transform.TransformDirection(moveDirection * normalSpeed);

        //      r.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);

        RaycastHit hit;
        for (int i = 0; i < hoverPoints.Length; i++) {
            var hoverPoint = hoverPoints[i];
            if (Physics.Raycast(hoverPoint.transform.position, Vector3.down, out hit, hoverHeight, layerMask)) {
                rb.AddForceAtPosition(Vector3.up * hoverForce * (-1f - (hit.distance / hoverHeight)), hoverPoint.transform.position);
			} else {
                if (transform.position.y > hoverPoint.transform.position.y) {
                    rb.AddForceAtPosition(hoverPoint.transform.up * hoverForce, hoverPoint.transform.position);
				} else {
                    rb.AddForceAtPosition(hoverPoint.transform.up * -hoverForce, hoverPoint.transform.position);
                }
            }
		}

        // Forward

        if (Mathf.Abs(currThurst) > 0) {
            rb.AddForce(transform.forward * currThurst);
		}

        if (currTurn > 0) {
            rb.AddRelativeTorque(Vector3.up * currTurn * turnStrength);
		} else if (currTurn < 0) {
            rb.AddRelativeTorque(Vector3.up * currTurn * turnStrength);
		}

		// Camera follow
		mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, cameraPosition.position, Time.deltaTime * cameraSmooth);
        mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, cameraPosition.rotation, Time.deltaTime * cameraSmooth);
	}

	private void OnCollisionEnter(Collision collision) {
        //Debug.Log("Collision detected: " + collision.gameObject.name);
	}
}
