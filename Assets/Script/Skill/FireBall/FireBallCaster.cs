using System;
using UnityEngine;

public class FireBallCaster : SkillCasterBase{
	private bool isButtonPressed = false;
    private double chantTime = 1;
    private double cooldownTime = 8;
    private double createTime = 0;
	private Timer cooldownTimer;
	private Timer chantTimer;
    private GameObject bullet = null;
	
	void Update () {
        if ((bullet != null) && (PhotonNetwork.time > createTime)) {
            bullet.SetActive(true);
        }
	}
    public Vector3 direction {
        get { return (target.transform.position - owner.transform.position).normalized; }
    }
    public void OnBulletHit( ) {
        photonView.RPC("OnBulletHitRPC", PhotonTargets.All);
    }

	private void StartChant() {
        if (target == null) return;
        photonView.RPC("StartChantRPC", PhotonTargets.All, PhotonNetwork.time + 0.1);
    }
    private void FinishChant(SbiEvent e) {
        photonView.RPC(
			"FinishChantRPC", PhotonTargets.All, chantTimer.GetFinishTime() , position, direction
		);
    }
    private void CancelChant() {
        if (chantTimer.isTiming) {
            photonView.RPC("CancelChantRPC", PhotonTargets.All);
		}
    }

    [PunRPC]
    public void StartChantRPC(double startTime) {
        chantTimer.InitTiming(startTime, startTime + chantTime);
    }
    [PunRPC]
    public void FinishChantRPC(double createTime , Vector3 createPosition, Vector3 direction) {
        cooldownTimer.InitTiming(PhotonNetwork.time, createTime + cooldownTime);
        bullet = FireBallBullet.CreateInstance(createTime, createPosition, direction, faction, this);
        this.createTime = createTime;
        if (createTime > PhotonNetwork.time) {
            bullet.SetActive(false);
		}
    }
    [PunRPC]
    public void CancelChantRPC() { chantTimer.CancelTiming(); }
    [PunRPC]
    public void OnBulletHitRPC() {
        GameObject explodeGameObject = (GameObject)GameObject.Instantiate(
			Resources.Load("Prefab/Skill/Explosion"), bullet.transform.position, bullet.transform.rotation
		);
        Destroy(bullet);
        bullet = null;
    }

    protected override void SetSkillID() { skillID = 0; }
    protected override void SetSkillName() { skillName = "Fire Ball"; }
    protected override void SetSkillColor() { buttonColor = new Color(0.8f, 0.4f, 0.2f); }
    protected override void Init() {
        chantTimer = GetTimerByType("Chant");
        chantTimer.startEventName = "startChant";
        chantTimer.finishEventName = "finishChant";
        chantTimer.stopEventName = "stopChant";
        cooldownTimer = GetTimerByType("Cooldown");
        cooldownTimer.startEventName = null;
        cooldownTimer.finishEventName = null;
        cooldownTimer.stopEventName = null;

        if (isControllable) {
            EventManager eventManager = EventManager.Instance;
            eventManager.RegisterListener(chantTimer, "finishChant", this, FinishChant);
        }
    }
    protected override void OnSkillButtonDown(SbiEvent e) {
        SkillButtonEventData data = e.data as SkillButtonEventData;
        if (index != data.index) {
            CancelChant();
            return;
        }
        if (!cooldownTimer.isTiming) { StartChant(); }
    }
    protected override void OnSkillButtonUp(SbiEvent e) {
        SkillButtonEventData data = e.data as SkillButtonEventData;
        if (index != data.index) { return; }
        CancelChant();
    }
    protected override void OnPlayerMove(SbiEvent e) { CancelChant(); }
}
