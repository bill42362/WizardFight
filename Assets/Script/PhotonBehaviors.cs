using UnityEngine;
using System.Collections;

public class PhotonBehaviors : Photon.PunBehaviour
{
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
            GameObject me = this.gameObject;
            me.tag = "Player";
            GameManager.Instance.SetPlayerCharacter( me );
            EventManager.Instance.CastEvent(me, "playerChange", new PlayerChangeEventData(me) );
        }
        else
        {
            Debug.Log("Entering OnPhotonInstantiate of others'");
            GameManager.Instance
                       .GetPlayerCharacter()
                       .GetComponent<LookAt>()
                       .target = this.gameObject;
            EventManager.Instance.CastEvent(this.gameObject, "enemyChange", new PlayerChangeEventData(this.gameObject));
        }
    }
    public void OnLeftClicked()
    {
        GameManager.Instance
                   .GetPlayerCharacter()
                   .GetPhotonView()
                   .RPC("moveByLeftOrRight", PhotonTargets.All, false);
    }
    public void OnRightClicked()
    {
        GameManager.Instance
                   .GetPlayerCharacter()
                   .GetPhotonView()
                   .RPC("moveByLeftOrRight", PhotonTargets.All, true);
    }
}
