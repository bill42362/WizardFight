using UnityEngine;
using System.Collections;
using System;

public class SkillHandler : Photon.PunBehaviour{
    private GameObject owner;
    private Hashtable skillCasterTable = new Hashtable();
    void Awake () {
		owner = gameObject.transform.parent.gameObject;

    }
	void Start () {
        SetSkillCasters();
	}

    private void SetSkillCasters()
    {
        int ownerID = GetOwnerID();
        GameObject[] skillCasters = GameManager.Instance.GetCharacterSkillCastersById(ownerID);
        foreach (GameObject caster in skillCasters) { caster.transform.parent = transform; }
        PlayerSkillsReadyEventData data = new PlayerSkillsReadyEventData(owner, skillCasters);
        EventManager.Instance.CastEvent(this, "playerSkillsReady", data);
    }

    private int GetOwnerID()
    {
        return owner.GetComponent<Role>().playerId;
    }

    public void CastRPC(int skillID) {
		photonView.RPC("Cast", 
                        PhotonTargets.All, 
                        skillID, 
                        transform.position,
                        GetDirection(),
                        PhotonNetwork.time + 0.1);
	}
    public void StartGuidingRPC(int skillID) {
		photonView.RPC("StartGuiding", PhotonTargets.AllViaServer, skillID);
	}
    public void StopGuidingRPC(int skillID) {
		photonView.RPC("StopGuiding", PhotonTargets.AllViaServer, skillID);
	}

    [PunRPC]
    public void Cast(int skillID, Vector3 createPosition, Vector3 forward ,double createTime) {
		CastingEventData castingData = new CastingEventData("casting", 
                                                             owner, 
                                                             skillID, 
                                                             createPosition, 
                                                             forward, 
                                                             createTime);
        Debug.Log("CreatePosition: " + createPosition);
		EventManager.Instance.CastEvent(EventManager.Instance, "casting", castingData);
    }
    [PunRPC]
    public void StartGuiding(int skillID) {
		GuideTimer guideTimer = null;
		foreach(SkillProperties p in GetComponentsInChildren<SkillProperties>()) {
			if(skillID == p.skillId) {
				guideTimer = p.gameObject.GetComponent<GuideTimer>();
			}
		}
		if(null != guideTimer) {
			GuidingEventData guideData = new GuidingEventData("startGuiding", owner, guideTimer, skillID);
			EventManager.Instance.CastEvent(EventManager.Instance, "startGuiding", guideData);
		}
    }
    [PunRPC]
    public void StopGuiding(int skillID) {
		GuideTimer guideTimer = null;
		foreach(SkillProperties p in GetComponentsInChildren<SkillProperties>()) {
			if(skillID == p.skillId) {
				guideTimer = p.gameObject.GetComponent<GuideTimer>();
			}
		}
		if(null != guideTimer) {
			GuidingEventData guideData = new GuidingEventData("stopGuiding", owner, guideTimer, skillID);
			EventManager.Instance.CastEvent(EventManager.Instance, "stopGuiding", guideData);
		}
    }

    public Vector3 GetDirection()
    {
        return (owner.GetComponent<LookAt>().target.transform.position - this.transform.position).normalized;
    } 
}
