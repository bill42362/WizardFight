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
		isMine = photonView.isMine;
        if (isMine && !NetworkManager.Instance.isOffline) {
			// On line player.
        } else if(NetworkManager.Instance.isOffline) {
			object[] instantiationData = GetComponent<PhotonView>().instantiationData;
			if((null != instantiationData) && (0 != instantiationData.Length)) {
				Hashtable instHashtable = (Hashtable)instantiationData[0];
				if(instHashtable.ContainsKey("isNeutral") && ((bool)instHashtable["isNeutral"] == true)) {
					// Offline neutral.
					GameManager.Instance.SetPlayerCharacter(2, gameObject);
				}
			} else {
				// Offline player.
				GameManager.Instance.SetPlayerCharacter(1, gameObject);
			}
        } else {
			// On line enemy.
			GameObject player = GameManager.Instance.GetPlayerCharacter();
            player.GetComponent<LookAt>().target = gameObject;
            GetComponent<LookAt>().target = player;
			int playerId = (GameManager.Instance.GetPlayerID() == 1) ? 2 : 1;
			GetComponent<Role>().playerId = playerId;
			GameManager.Instance.SetPlayerCharacter(playerId, gameObject);
            EventManager.Instance.CastEvent(
				this, "enemyChange", new PlayerChangeEventData(gameObject)
			);
		}
    }
}
