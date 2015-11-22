using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoleEventController : Photon.PunBehaviour {
	public Dictionary<string, Vector3> eventPairs = new Dictionary<string, Vector3>();
    public bool isControllable = false;
	private Role role;
	private Rigidbody rigidbody;
	public void Start() { 
		role = GetComponent<Role>();
		rigidbody = GetComponent<Rigidbody>();

		eventPairs["leftButtonClick"] = new Vector3(-1, 0, 0);
		eventPairs["rightButtonClick"] = new Vector3(1, 0, 0);
		var e = eventPairs.GetEnumerator();
		while( isControllable && e.MoveNext()) {
			EventManager.Instance.RegisterListener(EventManager.Instance, e.Current.Key, gameObject, OnEventTriggered);
		}
	}
	public void Update () {
        //GetComponent<PhotonTransformView>().SetSynchronizedValues(rigidbody.velocity, 0);
	}
	public void OnEventTriggered(SbiEvent e) {
        float speed = 20;
        Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(eventPairs[e.type] * speed);
        if (CanMove())
        {
            this.photonView.RPC("MoveBySpeed", 
                                PhotonTargets.AllViaServer, 
                                transform.position.x, 
                                transform.position.y, 
                                transform.position.z, 
                                velocity.x,
                                velocity.y,
                                velocity.z);
        }
    }
    [PunRPC]
    public void MoveBySpeed(float x, float y, float z, float vx, float vy, float vz)
    {
        transform.position = new Vector3(x, y, z);
        if (null != rigidbody) { rigidbody.velocity = new Vector3( vx, vy, vz); }
    }
    public bool CanMove()
    {
        return rigidbody.velocity.magnitude < 0.01;
       
    }
}
