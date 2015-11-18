using UnityEngine;
using System.Collections;

public class Faction : Photon.PunBehaviour {

    public int ownerId = -1; // -1 == unset , 0 == neutral;
    void Start () {
        if (ownerId == -1)
            SetNeutral();
    }
    public void SetNeutral()
    {
        ownerId = 0;
    }
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        if ( ownerId == -1 && !NetworkManager.Instance.isOffline )
        {
            SetFaction(info.sender.ID);
        }
    }
    
    public bool IsRival(Faction faction)
    {
		return ( faction == null || ownerId != faction.ownerId );
    }
    public int GetFaction()
    {
        return ownerId;
    }
    public void SetFaction( int ownerid )
    {
        this.ownerId = ownerid;
    }
    public void SetFaction(Faction ownerFaction) { ownerId = ownerFaction.ownerId; }
}
