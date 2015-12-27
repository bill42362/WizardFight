using UnityEngine;
using System.Collections;

public class PhotonBehaviors : Photon.PunBehaviour
{

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
        object[] instantiationData = GetComponent<PhotonView>().instantiationData;
        int ID = (int)instantiationData[0];
        Debug.Log("ID " + ID);
        if ( ID != 0 && photonView.isMine) // Case 1 : Player
        {

            this.gameObject.name = "Player";
            this.gameObject.tag = "Player";
            this.gameObject.AddComponent<LabelLookAtTarget>();
            this.gameObject.GetComponent<RoleEventController>().isControllable = true;
            EventManager.Instance.CastEvent(this, "playerChange", new PlayerChangeEventData(this.gameObject));

        }
        else // Case 2: Enemy.
        {
            if (ID == 0)
                this.gameObject.name = "NeutralRole";
            
            EventManager.Instance.CastEvent(this, "enemyChange", new PlayerChangeEventData(gameObject) );
        }
        GameManager.Instance.SetCharacter(ID, this.gameObject);
        this.gameObject.GetComponent<Role>().playerId = ID;
        this.gameObject.GetComponent<Faction>().SetFaction(ID);
        Debug.Log("GetComponent<Role>().playerId " + this.gameObject.GetComponent<Role>().playerId);

    }
}
