#pragma strict
var battleFieldView: BattleFieldView; // BattleFieldView.js
var playerView: WizardView; // WizardView.js
var rootCanvas: RootCanvas; // RootCanvas.js
var nguiPanel: GameObject;
private var app: WizardFightApplication; // WizardFightApplication.js

function Start () {
	app = WizardFightApplication.Shared();
	rootCanvas = app.components.RootCanvas;
}
function Update () {
}
