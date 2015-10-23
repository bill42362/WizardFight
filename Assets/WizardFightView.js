#pragma strict
import UnityEngine.UI;
var battleFieldView: BattleFieldView; // BattleFieldView.js
var playerView: WizardView; // WizardView.js
var enemiesView: EnemiesView; // EnemiesView.js
var rootCanvas: RootCanvas; // RootCanvas.js
var skillsPanel: Image;
private var app: WizardFightApplication; // WizardFightApplication.js

function Awake () {
	app = WizardFightApplication.Shared();
	battleFieldView = GetComponentInChildren(BattleFieldView);
	rootCanvas = GetComponentInChildren(RootCanvas);
	playerView = GetComponentInChildren(WizardView);
	enemiesView = GetComponentInChildren(EnemiesView);
}
