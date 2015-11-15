using UnityEngine;
using System.Collections;

public class SkillHandler : Photon.PunBehaviour{
    private GameObject owner;
    public PhotonView photonView;
    private Hashtable skillCasterTable = new Hashtable();
    void Awake ()
    {
		owner = gameObject.transform.parent.gameObject;
		photonView = GetComponent<PhotonView>();
        EventManager.Instance.RegisterListener(GameManager.Instance, "playerSkillsReady", gameObject, OnPlayerSkillsReady);
    }
	void Start () {
		int[] skillIds = {0, 1, 2};
		GameObject[] skillCasters = {
			(GameObject)Instantiate(Resources.Load("Prefab/Skill/FireBallCaster"), Vector3.zero, Quaternion.identity),
			(GameObject)Instantiate(Resources.Load("Prefab/Skill/BlizzardCaster"), Vector3.zero, Quaternion.identity),
			(GameObject)Instantiate(Resources.Load("Prefab/Skill/ThunderNovaCaster"), Vector3.zero, Quaternion.identity)
		};
		PlayerSkillsReadyEventData data = new PlayerSkillsReadyEventData(owner, skillIds, skillCasters);
        EventManager.Instance.CastEvent(GameManager.Instance, "playerSkillsReady", data);
	}
    public void OnPlayerSkillsReady(SbiEvent e)
    {
        PlayerSkillsReadyEventData data = (PlayerSkillsReadyEventData)e.data;
		if(owner != data.player) { return; }
        for ( int i = 0; i < data.skillIDs.Length; i++ )
        {
            GameObject obj = data.skillCasters[i];
            skillCasterTable[i] = obj;
            obj.transform.parent = this.gameObject.transform;
        }

    }

    public void CastRPC(int skillID) {
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
