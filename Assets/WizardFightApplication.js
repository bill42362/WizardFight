#pragma strict
static private var app: WizardFightApplication;
var components: WizardFightComponents;
var controller: WizardFightController;
var model: WizardFightModel;
var view: WizardFightView;

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
