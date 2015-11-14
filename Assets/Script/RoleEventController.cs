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
			EventManager.Instance.RegisterListener(EventManager.Instance, e.Current.Key, gameObject, OnEventTriggeredRPC);
		}
	}
	public void OnEventTriggeredRPC(SbiEvent e) {
		GetComponent<PhotonView>().RPC("OnEventTriggered", PhotonTargets.AllBufferedViaServer, e.type);	
	}
	[PunRPC]
	public void OnEventTriggered(string type) {
		float speed = 10;
		if(null != role) { speed = (float)role.speed; }
		Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(eventPairs[type]*speed);
		if(null != rigidbody) { rigidbody.velocity = velocity; }
	}
}
