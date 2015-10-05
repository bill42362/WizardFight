#pragma strict
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var model: WizardFightModel; // WizardFightModel.js
private var view: WizardFightView; // WizardFightView.js

function Start () {
	app = WizardFightApplication.Shared();
	components = app.components;
	model = app.model;
	view = app.view;
}

function Update () {
	if(null == model) { model = app.model; }
	if(null == view) { view = app.view; }
	if(null == model.battleFieldModel) {
		var battleFieldModel = Instantiate(components.BattleFieldModel);
		model.battleFieldModel = battleFieldModel;
		battleFieldModel.gameObject.SetActive(true);
		var battleFieldView = Instantiate(components.BattleFieldView);
		battleFieldView.gameObject.SetActive(true);
		view.battleFieldView = battleFieldView;
	}
	if(null == model.playerModel) {
		var playerModel = Instantiate(components.WizardModel);
		playerModel.gameObject.transform.position = Vector3(0, 0, -5);
		model.playerModel = playerModel;
		playerModel.gameObject.name = 'player';
		playerModel.gameObject.SetActive(true);
		var playerView = Instantiate(components.WizardView);
		playerView.wizardModel = playerModel;
		view.playerView = playerView;
		playerView.gameObject.SetActive(true);
	}
}
