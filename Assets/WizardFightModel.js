#pragma strict
var battleFieldModel: BattleFieldModel; // BattleFieldModel.js
var playerModel: WizardModel; // WizardModel.js
var enemies: Enemies; // Enemies.js
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js

function Awake () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	battleFieldModel = GetComponentInChildren(BattleFieldModel);
	playerModel = GetComponentInChildren(WizardModel);
	enemies = GetComponentInChildren(Enemies);
}
function GetPlayerModel(): WizardModel {
	if(null == playerModel) { playerModel = GetComponentInChildren(WizardModel); }
	return playerModel;
}
function GetEnemies(): Enemies {
	if(null == enemies) { enemies = GetComponentInChildren(Enemies); }
	return enemies;
}
