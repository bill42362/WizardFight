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
        bool direction = false;
        if ( e.type == "leftButtonClick" )
        {
            direction = true;
        }
        if (CanMove())
        {
            this.photonView.RPC("MoveBySpeed", 
                                PhotonTargets.AllBufferedViaServer, 
                                transform.position.x, 
                                transform.position.y, 
                                transform.position.z, 
                                speed , direction);
        }
    }
    [PunRPC]
    public void MoveBySpeed(float x, float y, float z, float speed , bool isLeft)
    {
        transform.position.Set(x, y, z);
        string type = (isLeft) ? "leftButtonClick" : "rightButtonClick"; 
        Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(eventPairs[type] * speed);
        if (null != rigidbody) { rigidbody.velocity = velocity; }
    }
    public bool CanMove()
    {
        return rigidbody.velocity.magnitude < 0.1;
       
    }
}
