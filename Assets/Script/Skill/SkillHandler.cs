using UnityEngine;
using System.Collections;

public class SkillHandler : Photon.PunBehaviour{
    public PhotonView photonView;
    private GameObject owner;
    private Hashtable skillCasterTable = new Hashtable();
    void Awake () {
		owner = gameObject.transform.parent.gameObject;
		photonView = GetComponent<PhotonView>();
    }
	void Start () {
		int playerId = owner.GetComponent<Role>().playerId;
		foreach(GameObject caster in GameManager.Instance.GetCharacterSkillCastersById(playerId)) {
			caster.transform.parent = transform;
		}
	}

    public void CastRPC(int skillID) {
		print("SkillHandler.CastRPC() owner: " + owner.ToString());
		photonView.RPC("Cast", PhotonTargets.AllViaServer, skillID);
	}
    public void StartGuidingRPC(int skillID) {
		photonView.RPC("StartGuiding", PhotonTargets.AllViaServer, skillID);
	}
    public void StopGuidingRPC(int skillID) {
		photonView.RPC("StopGuiding", PhotonTargets.AllViaServer, skillID);
	}

    [PunRPC]
    public void Cast(int skillID) {
		print("SkillHandler.Cast() owner: " + owner.ToString());
		CastingEventData castingData = new CastingEventData("casting", owner, skillID);
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
}
