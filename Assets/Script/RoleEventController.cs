using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoleEventController : MonoBehaviour {
	public Dictionary<string, Vector3> eventPairs = new Dictionary<string, Vector3>();
	private Role role;
	private Rigidbody rigidbody;
	public void Start() { 
		role = GetComponent<Role>();
		rigidbody = GetComponent<Rigidbody>();

		eventPairs["leftButtonClick"] = new Vector3(-1, 0, 0);
		eventPairs["rightButtonClick"] = new Vector3(1, 0, 0);
		var e = eventPairs.GetEnumerator();
		while(e.MoveNext()) {
			EventManager.Instance.RegisterListener(EventManager.Instance, e.Current.Key, gameObject, OnEventTriggered);
		}
	}
	public void OnEventTriggered(SbiEvent e) {
		float speed = 10;
		if(null != role) { speed = (float)role.speed; }
		Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(eventPairs[e.type]*speed);
		if(null != rigidbody) { rigidbody.velocity = velocity; }
        this.GetComponent<PhotonTransformView>().SetSynchronizedValues(velocity, rigidbody.angularVelocity.magnitude);
	}
}
