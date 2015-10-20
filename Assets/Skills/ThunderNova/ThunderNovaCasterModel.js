#pragma strict
var thunderNovaModel: ThunderNovaModel; // ThunderNovaModel.js
var thunderNovaView: ThunderNovaView; // ThunderNovaView.js
var skillName: String = "Thunder Nova";
var skillColor: Color = Color(0.8, 0.6, 0.2, 1.0);
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
	skillCaster.SetSkillName(skillName);
	skillCaster.SetSkillColor(skillColor);
}
var Cast = function(chantedTime: double) {
	var model = thunderNovaModel;
	model.SetAppearTime(chantedTime);
	var novaPosition = transform.position;
	novaPosition.y = 0;
	model.gameObject.transform.position = novaPosition;
	model.gameObject.SetActive(true);
	var view = thunderNovaView;
	view.gameObject.transform.position = novaPosition;
	view.gameObject.SetActive(true);
};
