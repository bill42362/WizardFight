using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {
	public int skillIndex = 0;
	private SkillCasterBase skillCasterBase;
	private Timer coolDownTimer;
	private Text coolDownTimeText;
	private RectTransform coolDownIndicatorRectTransform;

	public void Awake () {
        EventManager.Instance.RegisterListener(EventManager.Instance, "casterReady", gameObject, OnCasterReady);
	}
	public void Update () {
		if( null != coolDownTimeText) {
			if( coolDownTimer != null && !coolDownTimer.isTiming ) {
				float remainCoolDownTime = (float)coolDownTimer.GetRemainTime();
				if(1.0 < remainCoolDownTime) {
					coolDownTimeText.text = remainCoolDownTime.ToString("#");
				} else {
					coolDownTimeText.text = remainCoolDownTime.ToString(".#");
				}
				float coolDownTime = (float)coolDownTimer.duration;
				Vector2 anchorMin = coolDownIndicatorRectTransform.anchorMin;
				anchorMin.x = (1 - 1000*remainCoolDownTime/coolDownTime);
				coolDownIndicatorRectTransform.anchorMin = anchorMin;
			} else {
				coolDownTimeText.text = "";
			}
		}
	}
    public void OnCasterReady(SbiEvent e) {
        CasterReadyEventData data = (CasterReadyEventData)e.data;
		if(GameManager.Instance.GetPlayer() != data.player) { return; }
		if(skillIndex == data.skillIndex) {
			skillCasterBase = data.skillCaster.GetComponent<SkillCasterBase>();
            coolDownTimer = skillCasterBase.GetTimerByType("cooldown");
		}
		if(null != skillCasterBase) {
			GetComponentInChildren<Text>().text = skillCasterBase.skillName;
			name = skillCasterBase.skillName + " Button";
			GetComponent<Image>().color = skillCasterBase.buttonColor;
		}
		if(null != coolDownTimer) {
			coolDownTimeText = GetComponentsInChildren<Text>()[1] as Text;
			coolDownIndicatorRectTransform = (GetComponentsInChildren<Image>()[1] as Image).rectTransform;
		}
    }
	public void OnPointerDown() {
		EventManager.Instance.CastEvent(
			this, "skillButtonDown", new SkillButtonEventData("down", skillIndex)
		);
	}
	public void OnPointerUp() {
		EventManager.Instance.CastEvent(
			this, "skillButtonUp", new SkillButtonEventData("up", skillIndex)
		);
	}
}
