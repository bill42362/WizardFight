using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoleEventController : Photon.PunBehaviour {
	public Dictionary<string, Vector3> eventPairs = new Dictionary<string, Vector3>();
    public bool isControllable = false;
	private Role role;
    private bool isLeft;
    private double startTime;
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
        if ( startTime > 0 && PhotonNetwork.time >= startTime )
        {
            string type = (isLeft) ? "leftButtonClick" : "rightButtonClick";
            Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(eventPairs[type] * speed);
            rigidbody.velocity = velocity;
            startTime = -1f;
        }
        GetComponent<PhotonTransformView>().SetSynchronizedValues(rigidbody.velocity, 0);

    }
	public void OnEventTriggered(SbiEvent e) {
        bool isLeft = (e.type == "leftButtonClick");
        
        if (CanMove())
        {
            this.photonView.RPC("StartMove",PhotonTargets.All, PhotonNetwork.time + 0.1 , isLeft);
        }
    }

    public bool CanMove()
    {
        return rigidbody.velocity.magnitude < 0.01;
       
    }
    [PunRPC]
    public void StartMove( double startTime , bool isLeft)
    {
        this.startTime = startTime;
        this.isLeft = isLeft;
    }
}
