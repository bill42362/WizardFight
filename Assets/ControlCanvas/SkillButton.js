#pragma strict
import UnityEngine.UI;
var skillIndex: int = 0;
var skillCaster: GameObject;
private var eventCenter: EventCenter;
private var skillProperties: SkillProperties;
private var coolDownTimer: CoolDownTimer;
private var coolDownTimeText: Text;
private var coolDownIndicatorRectTransform: RectTransform;

function Awake () {
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	var playerSkillCasters: GameObject = GameObject.FindWithTag('PlayerSkillCasters');
	if(playerSkillCasters.transform.childCount > skillIndex) {
		skillCaster = playerSkillCasters.transform.GetChild(skillIndex).gameObject;
		skillProperties = skillCaster.GetComponent(SkillProperties);
		coolDownTimer = skillCaster.GetComponent(CoolDownTimer);
	}
	if(null != skillProperties) {
		GetComponentInChildren(Text).text = skillProperties.skillName;
		name = skillProperties.skillName + ' Button';
		GetComponent(Image).color = skillProperties.buttonColor;
	}
	if(null != coolDownTimer) {
		coolDownTimeText = GetComponentsInChildren(Text)[1] as Text;
		coolDownIndicatorRectTransform = (GetComponentsInChildren(Image)[1] as Image).rectTransform;
	}
}
function Update () {
	if(null != coolDownTimeText) {
		if(false == coolDownTimer.GetIsCoolDownFinished()) {
			var remainCoolDownTime: double = coolDownTimer.GetRemainCoolDownTime();
			if(1.0 < remainCoolDownTime) {
				coolDownTimeText.text = remainCoolDownTime.ToString('#');
			} else {
				coolDownTimeText.text = remainCoolDownTime.ToString('.#');
			}
			var coolDownTime: double = coolDownTimer.coolDownTime;
			coolDownIndicatorRectTransform.anchorMin.x = (1 - 1000*remainCoolDownTime/coolDownTime);
		} else {
			coolDownTimeText.text = '';
		}
	}
}
function OnPointerDown() {
	eventCenter.CastEvent(this, 'skillButtonDown', skillIndex);
}
function OnPointerUp() {
	eventCenter.CastEvent(this, 'skillButtonUp', skillIndex);
}
