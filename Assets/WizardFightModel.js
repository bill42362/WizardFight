#pragma strict
var battleFieldModel: BattleFieldModel; // BattleFieldModel.js
var playerModel: WizardModel; // WizardModel.js
var enemies: Enemies; // Enemies.js
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js

function Awake () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	enemies = GetComponentInChildren(Enemies);
}
