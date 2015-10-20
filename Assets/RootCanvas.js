#pragma strict
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js

function Awake () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	GetComponent(Button).onClick.AddListener(OnClick);
}
function OnClick() {
	eventCenter.CastEvent(this, 'mouseup', Input.mousePosition);
}
