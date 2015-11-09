using UnityEngine;
using System.Collections;

public class PhotonBehaviors : Photon.PunBehaviour
{
	public bool isMine = false;
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if ( photonView.isMine )
        {
            Destroy(gameObject);
        }
    }
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);

		isMine = photonView.isMine;
        if ( this.photonView.isMine )
        {
            GameObject me = gameObject;
            me.tag = "Player";
            GameManager.Instance.SetPlayerCharacter( me );
        }
        else
        {
            GameManager.Instance
                       .GetPlayerCharacter()
                       .GetComponent<LookAt>()
                       .target = gameObject;
            EventManager.Instance.CastEvent(
				this, "enemyChange", new PlayerChangeEventData(gameObject)
			);
        }
    }
}
