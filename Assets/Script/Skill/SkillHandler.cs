using UnityEngine;
using System.Collections;

public class SkillHandler : Photon.PunBehaviour{
    Hashtable skillCasterTable = new Hashtable();
    void Awake()
    {
        EventManager.Instance.RegisterListener(GameManager.Instance, "playerSkillsReady", this.gameObject, OnPlayerSkillsReady);
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnPlayerSkillsReady(SbiEvent e)
    {
        PlayerSkillsReadyEventData data = (PlayerSkillsReadyEventData)e.data;
        for ( int i = 0; i < data.skillIDs.Length; i++ )
        {
            GameObject obj = data.skillCasters[i];
            skillCasterTable[i] = obj;
            obj.transform.parent = this.gameObject.transform;
        }

    }

    [PunRPC]
    void StartCasting( int skillID) {
    }
    [PunRPC]
    void StopCasting( int skillID )
    {
   
    }
}
