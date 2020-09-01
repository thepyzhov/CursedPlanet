using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class HoverCarControl : MonoBehaviour
{
    Rigidbody rb;
    float m_deadZone = 0.1f;

    public float m_hoverForce = 9.0f;
    public float m_hoverHeight = 2.0f;
    public GameObject[] m_hoverPoints;

    public float m_forwardAcl = 100.0f;
    public float m_backwardAcl = 25.0f;
    float m_currThrust = 0.0f;

    public float m_turnStrength = 10f;
    float m_currTurn = 0.0f;

    public GameObject m_leftAirBrake;
    public GameObject m_rightAirBrake;

    public Transform spawnPoint;

    public LayerMask ignoreLayerMasks;
    //int m_layerMask;

    AudioManager audioManager;

    void Start() {
        rb = GetComponent<Rigidbody>();

        //m_layerMask = 1 << LayerMask.NameToLayer("Player");
        //m_layerMask = ~m_layerMask;

        audioManager = FindObjectOfType<AudioManager>();
    }

    void OnDrawGizmos() {

        //  Hover Force
        RaycastHit hit;
        for (int i = 0; i < m_hoverPoints.Length; i++) {
            var hoverPoint = m_hoverPoints [i];
            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit,m_hoverHeight, ignoreLayerMasks)) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
                Gizmos.DrawSphere(hit.point, 0.5f);
            } else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(hoverPoint.transform.position, hoverPoint.transform.position - Vector3.up * m_hoverHeight);
            }
        }
    }
	
    void Update()
    {

        // Main Thrust
        m_currThrust = 0.0f;
        float aclAxis = Input.GetAxis("Vertical");
        if (aclAxis > m_deadZone)
            m_currThrust = aclAxis * m_forwardAcl;
        else if (aclAxis < -m_deadZone)
            m_currThrust = aclAxis * m_backwardAcl;

        // Turning
        m_currTurn = 0.0f;
        float turnAxis = Input.GetAxis("Horizontal");
        if (Mathf.Abs(turnAxis) > m_deadZone) {
            m_currTurn = turnAxis;
        }

		if ((aclAxis != 0) && !audioManager.IsPlaying("PlayerMovement")) {
			audioManager.Play("PlayerMovement");
		} else if (aclAxis == 0) {
			audioManager.Stop("PlayerMovement");
		}
		//Debug.Log("Turn axis: " + turnAxis + "\nAcl axis: " + aclAxis);
    }

    void FixedUpdate() {
        //  Hover Force
        RaycastHit hit;
        for (int i = 0; i < m_hoverPoints.Length; i++)
        {
            var hoverPoint = m_hoverPoints [i];
            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, m_hoverHeight, ignoreLayerMasks))
            rb.AddForceAtPosition(Vector3.up 
                * m_hoverForce
                * (1.0f - (hit.distance / m_hoverHeight)), 
                                        hoverPoint.transform.position);
            else
            {
            if (transform.position.y > hoverPoint.transform.position.y)
                rb.AddForceAtPosition(
                hoverPoint.transform.up * m_hoverForce,
                hoverPoint.transform.position);
            else
                rb.AddForceAtPosition(
                hoverPoint.transform.up * -m_hoverForce,
                hoverPoint.transform.position);
            }
        }

        // Forward
        if (Mathf.Abs(m_currThrust) > 0)
            rb.AddForce(transform.forward * m_currThrust);

        // Turn
        if (m_currTurn > 0) {
            rb.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
        } else if (m_currTurn < 0) {
            rb.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
        }
    }
}
