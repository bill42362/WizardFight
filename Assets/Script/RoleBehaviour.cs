using UnityEngine;
using System.Collections;

public class RoleBehaviour : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        LookAt lookAt = this.GetComponent<LookAt>();
        lookAt.target = GameObject.FindWithTag("Enemy");
	}

    [PunRPC]
    void moveByLeftOrRight(bool isRight )
    {
        Rigidbody rigidbody = this.GetComponent<Rigidbody>();
        Role role = this.GetComponent<Role>();
        if (role == null)
            return;
        double speed = role.speed ;
        Vector3 direction = (isRight) ? new Vector3((float)speed, 0, 0) : new Vector3((float) -speed, 0, 0); 
        Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(direction  );
        if ((null != direction) && (null != rigidbody))
        {
            rigidbody.velocity = velocity;
        }
    } 
}
