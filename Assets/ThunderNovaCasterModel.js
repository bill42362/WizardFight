#pragma strict
var thunderNovaGameObject: GameObject;
var skillCaster: SkillCaster; // SkillCaster.js
var castingTime: double = 4.0;
var alertTime: double = 2.0;
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	components = app.components;
	thunderNovaGameObject = components.ThunderNovaModel.gameObject;
}
function Update () {
	if(null != skillCaster) {
		transform.position = skillCaster.transform.position;
	}
}
function SetSkillCaster(s: SkillCaster) {
	skillCaster = s;
	skillCaster.SetCastingTime(castingTime);
	skillCaster.SetAlertTime(alertTime);
}
function GetSkillGameObject(): GameObject { return thunderNovaGameObject; }
