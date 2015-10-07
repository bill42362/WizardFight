﻿#pragma strict
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js
private var model: WizardFightModel; // WizardFightModel.js
private var view: WizardFightView; // WizardFightView.js
private var playerController: PlayerController; // PlayerController.js
private var rootCanvasMouseUpHearingId: int = -1;
private var groundPlane = Plane(Vector3(0.0, 1.0, 0.0), Vector3(0, 0, 0));

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
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
	if(null == playerController) {
		playerController = Instantiate(components.PlayerController);
		playerController.gameObject.SetActive(true);
	}
	if(null == playerController.playerModel) {
		playerController.SetPlayerModel(components.WizardModel.gameObject);
	}
	if(null == playerController.playerView) {
		playerController.SetPlayerView(components.WizardView.gameObject);
	}
	if(-1 == rootCanvasMouseUpHearingId) {
		rootCanvasMouseUpHearingId = eventCenter.RegisterListener(
			view.rootCanvas, 'mouseup', this, OnMouseUp
		);
	}
}
var OnMouseUp = function(e: SbiEvent) {
	var ray: Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	var rayDistance: float;
	if(groundPlane.Raycast(ray, rayDistance)) {
		var playerTargetPosition = ray.GetPoint(rayDistance);
		playerTargetPosition.y = 0.5;
		playerController.SetPlayerTargetPosition(playerTargetPosition);
	}
};
