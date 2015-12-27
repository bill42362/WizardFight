using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RoleEventController : Photon.PunBehaviour {
	public Dictionary<string, Vector3> eventPairs = new Dictionary<string, Vector3>();
    public bool isControllable = false;
	private Role role;
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
        UpdateVelocity();
        

    }
    private void UpdateVelocity()
    {
        GetComponent<PhotonTransformView>().SetSynchronizedValues(GetComponent<Rigidbody>().velocity, 0);
    }

	public void OnEventTriggered(SbiEvent e) {
        bool isLeft = (e.type == "leftButtonClick");
        
        if (CanMove())
        {
            string type = (isLeft) ? "leftButtonClick" : "rightButtonClick";
            Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(eventPairs[type] * speed);
            GetComponent<Rigidbody>().velocity = velocity;
        }
    }

    public bool CanMove()
    {
        return  GetComponent<Rigidbody>().velocity.magnitude < 0.01;
       
    }

}
