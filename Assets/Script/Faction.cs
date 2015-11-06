using UnityEngine;
using System.Collections;

public class Faction : Photon.PunBehaviour {

    private int owner = 1; // 0 = neutral;
    // Use this for initialization
    void Start () {
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
            owner = info.sender.ID;
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
}
