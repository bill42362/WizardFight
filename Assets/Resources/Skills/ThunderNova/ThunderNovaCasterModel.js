#pragma strict
var thunderNovaModel: ThunderNovaModel; // ThunderNovaModel.js
var thunderNovaView: ThunderNovaView; // ThunderNovaView.js
var skillName: String = "Thunder Nova";
var skillColor: Color = Color(0.8, 0.6, 0.2, 1.0);
var castingTime: double = 4000.0;
var alertTime: double = 2000.0;
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js
private var casterUpdated: boolean = false;

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	thunderNovaModel = Instantiate(Resources.Load(
		'Skills/ThunderNova/ThunderNovaModel', ThunderNovaModel
	));
	thunderNovaView = Instantiate(Resources.Load(
		'Skills/ThunderNova/ThunderNovaView', ThunderNovaView
	));
	thunderNovaView.SetModel(thunderNovaModel);
}
function Update () {
	var skillCaster = gameObject.GetComponent(SkillCasterModel);
	if((false == casterUpdated) && (null != skillCaster)) {
		UpdateSkillCaster();
		casterUpdated = true;
	}
}
function UpdateSkillCaster() {
	var skillCaster = gameObject.GetComponent(SkillCasterModel);
	skillCaster.SetCastingTime(castingTime);
	skillCaster.SetAlertTime(alertTime);
	skillCaster.SetCastCallback(Cast);
	skillCaster.SetSkillName(skillName);
	skillCaster.SetSkillColor(skillColor);
	skillCaster.SetSkillUpdated();
}
var Cast = function(chantedTime: double) {
	var model = thunderNovaModel;
	model.SetAppearTime(chantedTime);
	var novaPosition = transform.position;
	novaPosition.y = 0;
	model.gameObject.tag = gameObject.tag;
	model.gameObject.transform.position = novaPosition;
	model.gameObject.SetActive(true);
	var view = thunderNovaView;
	view.gameObject.transform.position = novaPosition;
	view.gameObject.SetActive(true);
};
