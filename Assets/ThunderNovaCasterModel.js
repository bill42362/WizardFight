#pragma strict
var thunderNovaModel: ThunderNovaModel;
var thunderNovaView: ThunderNovaView;
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
	thunderNovaModel = Instantiate(components.ThunderNovaModel);
	thunderNovaView = Instantiate(components.ThunderNovaView);
	thunderNovaView.SetModel(thunderNovaModel);
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
	skillCaster.SetCastCallback(Cast);
}
var Cast = function(chantedTime: double) {
	var model = thunderNovaModel;
	model.SetAppearTime(chantedTime);
	model.gameObject.transform.position = transform.position;
	model.gameObject.SetActive(true);
};
