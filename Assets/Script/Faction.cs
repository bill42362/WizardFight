using UnityEngine;
using System.Collections;

public class Faction : Photon.PunBehaviour {

    public int owner = -1; // -1 == unset , 0 == neutral;
    // Use this for initialization
    void Start () {
        if (owner == -1)
            SetNeutral();
    }
	// Update is called once per frame
	void Update () {
	}
    public void SetNeutral()
    {
        owner = 0;
    }
    public override void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        base.OnPhotonInstantiate(info);
        if ( !NetworkManager.Instance.isOffline )
        {
            SetFaction(info.sender.ID);
        }
    }
    
    public bool IsRival(Faction faction)
    {
        return ( faction == null || owner != faction.owner );
    }
    public int GetFaction()
    {
        return owner;
    }
    public void SetFaction( int ownerid )
    {
        this.owner = ownerid;
    }
}
