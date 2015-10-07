#pragma strict
static private var app: WizardFightApplication; // WizardFightApplication.js
var components: WizardFightComponents; // WizardFightComponents.js
var eventCenter: EventCenter; // EventCenter.js
var controller: WizardFightController; // WizardFightController.js
var model: WizardFightModel; // WizardFightModel.js
var view: WizardFightView; // WizardFightView.js

function Start () {
	app = GetComponent(WizardFightApplication);
	controller = components.WizardFightController;
	model = components.WizardFightModel;
	view = components.WizardFightView;
}
function Update () { }
static function Shared(): WizardFightApplication {
	if(null == app) {
		app = FindObjectOfType(WizardFightApplication);
	}
	return app;
}
