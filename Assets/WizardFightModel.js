﻿#pragma strict
var battleFieldModel: BattleFieldModel; // BattleFieldModel.js
var playerModel: WizardModel; // WizardModel.js
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
}
function Update () {
}
