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
        //GetComponent<PhotonTransformView>().SetSynchronizedValues(rigidbody.velocity, 0);
        if ( isMoving) {
            float diffTime = (float)(PhotonNetwork.time - startTime);
            if (diffTime < 0)
            {
                return;
            }
            else if (diffTime > Mathf.Abs(speed / acceleration))
            {
                isMoving = false;
            }
            else
            {
                transform.position = startPos
                                   + startVelo * diffTime
                                   + 0.5f * acceleration * startVelo.normalized * diffTime * diffTime;
            }
        }
    }
	public void OnEventTriggered(SbiEvent e) {
        Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(eventPairs[e.type] * speed);
        if (CanMove())
        {
            this.photonView.RPC("MoveBySpeed", 
                                PhotonTargets.All, 
                                transform.position.x, 
                                transform.position.y, 
                                transform.position.z, 
                                velocity.x,
                                velocity.y,
                                velocity.z,
                                PhotonNetwork.time + 0.1);
        }
    }
    [PunRPC]
    public void MoveBySpeed(float x, float y, float z, float vx, float vy, float vz, double time)
    {
        startPos = new Vector3(x, y, z);
        startVelo = new Vector3(vx, vy, vz);
        startTime = time;
        isMoving = true;
        Debug.Log("time:" + time);

    }
    public bool CanMove()
    {
        return !isMoving;
       
    }
}
