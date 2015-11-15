using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour {
	public int skillIndex = 0;
	public GameObject skillCaster;
	private SkillProperties skillProperties;
	private CoolDownTimer coolDownTimer;
	private Text coolDownTimeText;
	private RectTransform coolDownIndicatorRectTransform;

	public void Awake () {
        EventManager.Instance.RegisterListener(GameManager.Instance, "playerSkillsReady", gameObject, OnPlayerSkillsReady);
	}
	public void Update () {
		if(null != coolDownTimeText) {
			if(false == coolDownTimer.GetIsCoolDownFinished()) {
				float remainCoolDownTime = (float)coolDownTimer.GetRemainCoolDownTime();
				if(1.0 < remainCoolDownTime) {
					coolDownTimeText.text = remainCoolDownTime.ToString("#");
				} else {
					coolDownTimeText.text = remainCoolDownTime.ToString(".#");
				}
				float coolDownTime = (float)coolDownTimer.coolDownTime;
				Vector2 anchorMin = coolDownIndicatorRectTransform.anchorMin;
				anchorMin.x = (1 - 1000*remainCoolDownTime/coolDownTime);
				coolDownIndicatorRectTransform.anchorMin = anchorMin;
			} else {
				coolDownTimeText.text = "";
			}
		}
	}
    public void OnPlayerSkillsReady(SbiEvent e) {
        PlayerSkillsReadyEventData data = (PlayerSkillsReadyEventData)e.data;
		if(GameManager.Instance.GetPlayerCharacter() != data.player) { return; }
		GameObject[] playerSkillCasters = data.skillCasters;
		if(playerSkillCasters.Length > skillIndex) {
			skillCaster = playerSkillCasters[skillIndex];
			skillProperties = skillCaster.GetComponent<SkillProperties>();
			coolDownTimer = skillCaster.GetComponent<CoolDownTimer>();
		}
		if(null != skillProperties) {
			GetComponentInChildren<Text>().text = skillProperties.skillName;
			name = skillProperties.skillName + " Button";
			GetComponent<Image>().color = skillProperties.buttonColor;
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
