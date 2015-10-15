#pragma strict
var thunderNovaModel: ThunderNovaModel;
var thunderNovaView: ThunderNovaView;
var castingTime: double = 4000.0;
var alertTime: double = 2000.0;
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
function Update () { }
function UpdateSkillCaster() {
	var skillCaster = gameObject.GetComponent(SkillCaster);
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
