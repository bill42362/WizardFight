using System;
using UnityEngine;

public class BlizzardCaster : SkillCasterBase {
    private double cooldownTime = 20;
    private double guideTime = 10;
	private Timer cooldownTimer;
	private Timer guideTimer;
	private GameObject blizzard;
    
    [PunRPC]
    public void StartGuideRPC( double createTime ) {
        guideTimer.InitTiming(createTime, createTime + guideTime);
        cooldownTimer.InitTiming(createTime, createTime + cooldownTime);
        Vector3 targetPosition = transform.position;
        if (null != target) { targetPosition = target.transform.position; }
        if (null == blizzard) {
            blizzard = Blizzard.CreateInstance(targetPosition, faction);
        } else {
            blizzard.transform.position = targetPosition;
        }
        blizzard.gameObject.SetActive(true);
    }
    [PunRPC]
	private void StopGuideRPC() {
		guideTimer.CancelTiming();
        blizzard.gameObject.SetActive(false);
    }

    protected override void SetSkillID() { skillID = 2; }
    protected override void SetSkillName() { skillName = "Blizzard"; }
    protected override void SetSkillColor() { buttonColor = new Color(50, 150, 201); } 
    protected override void Init() {
        guideTimer = GetTimerByType("Guide");
        guideTimer.startEventName = "startGuide";
        guideTimer.finishEventName = null;
        guideTimer.stopEventName = "stopGuide";
        cooldownTimer = GetTimerByType("Cooldown");
        cooldownTimer.startEventName = null;
        cooldownTimer.finishEventName = null;
        cooldownTimer.stopEventName = null;
    }
    protected override void OnSkillButtonDown(SbiEvent e) {
        SkillButtonEventData data = e.data as SkillButtonEventData;
        if (index!= data.index) {
            if (guideTimer.isTiming) { StopGuide(); }
            return;
        }
        StartGuide();
    }
    private void StartGuide() {
        photonView.RPC("StartGuideRPC", PhotonTargets.All, PhotonNetwork.time);
    }
    private void StopGuide() {
        if (guideTimer.isTiming)
            photonView.RPC("StopGuideRPC",PhotonTargets.All);
    }
    protected override void OnSkillButtonUp(SbiEvent e) {
        SkillButtonEventData data = e.data as SkillButtonEventData;
        if (index != data.index) return;
        StopGuide();
    }
    protected override void OnPlayerMove(SbiEvent e) { StopGuide(); }
}
