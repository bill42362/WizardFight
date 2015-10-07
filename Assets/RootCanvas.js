#pragma strict
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
}
function Update () {
	if(Input.GetMouseButtonUp(0)) {
		var mousePosition = Input.mousePosition;
		eventCenter.CastEvent(this, 'mouseup', mousePosition);
	}
}
