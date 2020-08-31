using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour
{
	public static void SBFueld() {
		ActivePortal();
	}

	static void ActivePortal() {
		FindObjectOfType<PortalController>().ActivePortal();
	}
}
