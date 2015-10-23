#pragma strict
var playerModel: GameObject;
var playerView: GameObject;
var playerMoveTargetModel: GameObject;
var playerMoveTargetView: GameObject;
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js
private var playerViewActivated: boolean = false;
private var initPlayerPosition: Vector3 = Vector3(0, 0.5, -5);
private var playerTargetPosition: Vector3 = Vector3(0, 0.5, -5);
private var groundPlane: Plane = Plane(Vector3(0.0, 1.0, 0.0), Vector3(0, 0, 0));

function Awake () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	var appModel = app.GetModel();
	playerModel = appModel.playerModel.gameObject;
	var appView = app.GetView();
	playerView = appView.playerView.gameObject;
}
function Start () {
	eventCenter.RegisterListener(app.view.rootCanvas, 'mouseup', this, OnMouseUp);
}
function Update () {
	if(true == playerModel.activeSelf) {
		var targetDirection = playerTargetPosition - playerModel.gameObject.transform.position;
		var force = targetDirection*15;
		playerModel.GetComponent.<Rigidbody>().velocity = force;
		UpdateWizardView();
	} else {
		playerView.SetActive(false);
		playerViewActivated = false;
	}
}
var OnMouseUp = function(e: SbiEvent) {
	var ray: Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	var rayDistance: float;
	if(groundPlane.Raycast(ray, rayDistance)) {
		var playerTargetPosition = ray.GetPoint(rayDistance);
		playerTargetPosition.y = 0.5;
		SetPlayerTargetPosition(playerTargetPosition);
	}
};
function SetPlayerTargetPosition(pos: Vector3) {
	playerTargetPosition = pos;
	eventCenter.CastEvent(this, 'playerTargetPositionChanged', pos);
}
private function UpdateWizardView() {
	if(false == playerViewActivated) {
		playerView.SetActive(true);
		playerViewActivated = true;
	}
	playerView.transform.position = playerModel.transform.position;
	playerView.transform.rotation = playerModel.transform.rotation;
	playerView.transform.localScale = playerModel.transform.localScale;
}
