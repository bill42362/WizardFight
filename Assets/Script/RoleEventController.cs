using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoleEventController : Photon.PunBehaviour {
	public Dictionary<string, Vector3> eventPairs = new Dictionary<string, Vector3>();
    public bool isControllable = false;
	private Role role;
    private bool isLeft;
    private double startTime = -1;
    private const float speed = 15;
    private const float acceleration = -30; 
    private bool isMoving = false;
	public void Start() { 
		role = GetComponent<Role>();

		eventPairs["leftButtonClick"] = new Vector3(-1, 0, 0);
		eventPairs["rightButtonClick"] = new Vector3(1, 0, 0);
		var e = eventPairs.GetEnumerator();
		while( isControllable && e.MoveNext()) {
			EventManager.Instance.RegisterListener(EventManager.Instance, e.Current.Key, gameObject, OnEventTriggered);
		}
	}
	public void Update () {
        UpdateAnimation();
        if ( isControllable ) {
            UpdateVelocity();
        }

    }
    private void UpdateVelocity()
    {
        GetComponent<PhotonTransformView>().SetSynchronizedValues(GetComponent<Rigidbody>().velocity, 0);
    }
    private void UpdateAnimation()
    {
        if (startTime > 0 && PhotonNetwork.time >= startTime)
        {
            string type = (isLeft) ? "leftButtonClick" : "rightButtonClick";
            Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(eventPairs[type] * speed);
            GetComponent<Rigidbody>().velocity = velocity;
            startTime = -1f;
        }
    }

	public void OnEventTriggered(SbiEvent e) {
        bool isLeft = (e.type == "leftButtonClick");
        
        if (CanMove())
        {
            float delay = NetworkManager.Instance.RPCDelay;
            this.photonView.RPC("StartMove",PhotonTargets.All, (float)PhotonNetwork.time + delay, isLeft);
        }
    }

    public bool CanMove()
    {
        return startTime < 0 && GetComponent<Rigidbody>().velocity.magnitude < 0.01;
       
    }
    [PunRPC]
    public void StartMove( float startTime , bool isLeft)
    {
        if (!isControllable) { 
            NetworkManager.Instance.UpdateRPCDelay((float)PhotonNetwork.time - (float)startTime);
        }
        this.startTime = startTime;
        this.isLeft = isLeft;
    }
}
