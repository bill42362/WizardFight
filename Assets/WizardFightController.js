#pragma strict
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var model: WizardFightModel; // WizardFightModel.js
private var view: WizardFightView; // WizardFightView.js
private var groundPlane = Plane(Vector3(0.0, 1.0, 0.0), Vector3(0, 0, 0));
private var playerTargetPosition: Vector3 = Vector3(0, 0.5, -5);

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
		playerModel.gameObject.transform.position = Vector3(0, 0.5, -5);
		model.playerModel = playerModel;
		playerModel.gameObject.name = 'player';
		playerModel.gameObject.SetActive(true);
		var playerView = Instantiate(components.WizardView);
		playerView.wizardModel = playerModel;
		view.playerView = playerView;
		playerView.gameObject.SetActive(true);
	}
	if(Input.GetMouseButtonUp(0)) {
		var ray: Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var rayDistance: float;
		if(groundPlane.Raycast(ray, rayDistance)) {
			playerTargetPosition = ray.GetPoint(rayDistance);
			playerTargetPosition.y = 0.5;
		}
	}
	if(null != playerTargetPosition) {
		var targetDirection = playerTargetPosition - model.playerModel.gameObject.transform.position;
		var force = targetDirection*15;
		model.playerModel.GetComponent.<Rigidbody>().velocity = force;
	}
}
