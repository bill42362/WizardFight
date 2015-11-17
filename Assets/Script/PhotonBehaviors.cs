using UnityEngine;
using System.Collections;

public class PhotonBehaviors : Photon.PunBehaviour
{
	public bool isMine = false;
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        if ( photonView.isMine ) {
			GameManager.Instance.OnLeftRoom();
            Destroy(gameObject);
        }
    }
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        GameManager.Instance.SetPlayerCharacter(info.sender.ID, gameObject);
		isMine = photonView.isMine;
        if ( isMine ) {
            gameObject.tag = "Player";
            GameManager.Instance.SetPlayerID(info.sender.ID);
        } else {
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
