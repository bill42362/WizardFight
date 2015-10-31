using UnityEngine;
using System.Collections;

public class PhotonBehaviors : Photon.PunBehaviour
{
    public static GameObject me;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);

        if ( this.photonView.isMine )
        {
            Debug.Log("Entering OnPhotonInstantiate of mine");
            me = this.gameObject;
            me.tag = "Player";
        }
        else
        {
            Debug.Log("Entering OnPhotonInstantiate of others'");
            me.GetComponent<LookAt>().target = this.gameObject;
        }
    }

}
