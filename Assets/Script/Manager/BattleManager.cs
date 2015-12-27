using UnityEngine;
using System.Collections;

public class BattleManager : Photon.PunBehaviour {
    // ***** Shared Singleton Related *****
    private static BattleManager _instance = null;
    protected BattleManager() { }
    public static BattleManager Instance { get {
		if (_instance == null) {
			Debug.Log("Get BattleManager when the _instance is null! Do nothing and return.");
			return null;
		}
		return _instance;
	} }
    public static void CreateInstance() {
        if (_instance != null) {
            Debug.Log("Create BattleManager when the _instance is not null! Do nothing and return.");
            return;
        }
        GameObject bm = NetworkManager.Instance.Instantiate("BattleManager", Vector3.zero, Quaternion.identity, 0, null);
    }
    public override void OnPhotonInstantiate(PhotonMessageInfo info) {
        base.OnPhotonInstantiate(info);
        _instance = this;
        Debug.Log("OnPhotonInstantiation of BattleManager");
    }
    public int readyCount = 0;
    public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged) {
        foreach( string key in propertiesThatChanged.Keys ) {
            Debug.Log("Outer Room PropertyChanged -> " + key + " : " + propertiesThatChanged[key]);
            if ( photonView.isMine && (string)propertiesThatChanged[key] == "ready" ) {
                Debug.Log("Inner Room PropertyChanged -> " + key + " : " + propertiesThatChanged[key]);
                readyCount += 1;
                if ( readyCount == GameManager.Instance.maxPlayer) {
                    this.photonView.RPC("BattleStart", PhotonTargets.All, PhotonNetwork.time + 5 ); 
                }
            }
        }
    }
    [PunRPC]
    public void BattleStart( double startTime) {
        // TODO
        Debug.Log("Battle Start at time -> " + startTime);
        GameObject[] characters = GameManager.Instance.GetAllCharacter();
        // FIXME, this is weird lol
        characters[0].GetComponent<LookAt>().target = characters[1];
        characters[1].GetComponent<LookAt>().target = characters[0];
    }
}
