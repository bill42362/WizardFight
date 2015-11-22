using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoleEventController : Photon.PunBehaviour {
	public Dictionary<string, Vector3> eventPairs = new Dictionary<string, Vector3>();
    public bool isControllable = false;
	private Role role;
    private Vector3 startPos;
    private double startTime;
    private Vector3 startVelo;
	private Rigidbody rigidbody;
    private const float speed = 15;
    private const float acceleration = -30; 
    private bool isMoving = false;
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
        GetComponent<PhotonTransformView>().SetSynchronizedValues(rigidbody.velocity, 0);

    }
	public void OnEventTriggered(SbiEvent e) {
        Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(eventPairs[e.type] * speed);
        if (CanMove())
        {
            this.rigidbody.velocity = velocity;
        }
    }

    public bool CanMove()
    {
        return rigidbody.velocity.magnitude < 0.01;
       
    }
}
