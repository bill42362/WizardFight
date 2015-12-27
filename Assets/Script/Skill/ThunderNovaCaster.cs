using UnityEngine;

public class ThunderNovaCaster : SkillCasterBase
{

    private double cooldownTime = 12;
    private double emitTime = 0.5;

    private float dashSpeed = 50;
    private Timer cooldownTimer;
    private Timer emitTimer;
    protected override void SetSkillID() { skillID = 2; }
    protected override void SetSkillName() { skillName = "Thunder Nova"; }
    protected override void SetSkillColor() { buttonColor = new Color(0.6f, 0.6f, 0.2f); }
    protected override void Init()
    {
        emitTimer = GetTimerByType("Emit");
        emitTimer.startEventName = "onThunderDash";
        emitTimer.finishEventName = "onNovaEmit";
        emitTimer.stopEventName = null;
        cooldownTimer = GetTimerByType("Cooldown");
        cooldownTimer.startEventName = null;
        cooldownTimer.finishEventName = null;
        cooldownTimer.stopEventName = null;

        EventManager eventManager = EventManager.Instance;
        if (isControllable)
        {
            
            eventManager.RegisterListener(emitTimer, "onThunderDash", this, OnThunderDash);
        }
        eventManager.RegisterListener(emitTimer, "onNovaEmit", this, OnNovaEmit);

    }
    [PunRPC]
    public void ThunderDashRPC( double createTime )
    {
        emitTimer.InitTiming(PhotonNetwork.time, PhotonNetwork.time + emitTime);
        cooldownTimer.InitTiming(PhotonNetwork.time, PhotonNetwork.time + cooldownTime);
    }
    protected override void OnSkillButtonDown(SbiEvent e)
    {
        SkillButtonEventData data = e.data as SkillButtonEventData;
        if (index != data.index)
        {
            return;
        }
        if (!cooldownTimer.isTiming) {
            this.photonView.RPC("ThunderDashRPC", PhotonTargets.All, PhotonNetwork.time);
        }
    }
    protected override void OnSkillButtonUp(SbiEvent e)
    {

    }
    protected override void OnPlayerMove(SbiEvent e) {
    }

    private void OnThunderDash(SbiEvent e) {
        owner.transform.GetComponent<Rigidbody>().velocity += dashSpeed * owner.transform.forward;
    }
    private void OnNovaEmit(SbiEvent e)
    {
        Debug.Log("OnNovaEmit");
        GameObject nova = ThunderNova.CreateInstance(PhotonNetwork.time, position, faction, this);
    }
}
