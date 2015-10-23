#pragma strict
static private var app: WizardFightApplication; // WizardFightApplication.js
var components: WizardFightComponents; // WizardFightComponents.js
var eventCenter: EventCenter; // EventCenter.js
var controller: WizardFightController; // WizardFightController.js
var model: WizardFightModel; // WizardFightModel.js
var view: WizardFightView; // WizardFightView.js

function Awake () {
	app = GetComponent(WizardFightApplication);
	controller = GetComponentInChildren(WizardFightController);
	model = GetComponentInChildren(WizardFightModel);
	view = GetComponentInChildren(WizardFightView);
}
function Update () { }
function GetModel(): WizardFightModel {
	if(null == model) { model = GetComponentInChildren(WizardFightModel); }
	return model;
}
function GetView(): WizardFightView {
	if(null == view) { view = GetComponentInChildren(WizardFightView); }
	return view;
}
static function Shared(): WizardFightApplication {
	if(null == app) {
		app = FindObjectOfType(WizardFightApplication);
	}
	return app;
}
