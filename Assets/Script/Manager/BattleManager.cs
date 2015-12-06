using UnityEngine;
using System.Collections;

public class BattleManager : Photon.PunBehaviour {
    // ***** Shared Singleton Related *****
    private static BattleManager _instance = null;
    protected BattleManager()
    {
    }
    public static BattleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.Log("Get BattleManager when the _instance is null! Do nothing and return.");
                return null;
            }
            return _instance;
        }
    }
    public static void CreateInstance()
    {
        if (_instance != null)
        {
            Debug.Log("Create BattleManager when the _instance is not null! Do nothing and return.");
            return;
        }
        GameObject bm = new GameObject("BattleManager");
        _instance = bm.AddComponent<BattleManager>();
    }

    private int readyCount = 0;
    public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        foreach( string key in propertiesThatChanged.Keys )
        {
            Debug.Log("Room PropertyChanged -> " + key + " : " + propertiesThatChanged[key]);
            if ( PhotonNetwork.isMasterClient && propertiesThatChanged[key] == "ready" )
            {
                readyCount += 1;
                if ( readyCount == GameManager.Instance.maxPlayer)
                {
                    this.photonView.RPC("BattleStart",
                                         PhotonTargets.All,
                                         PhotonNetwork.time + 5 ); 
                }
            }
            
        }
        
    }
    [PunRPC]
    public void BattleStart( float startTime)
    {
        // TODO
        Debug.Log("Battle Start at time -> " + startTime);
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
