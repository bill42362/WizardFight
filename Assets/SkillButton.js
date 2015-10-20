#pragma strict
private var MAX_WIDTH: int = 100;
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js
private var uiNeedsLayout: boolean = false;
private var totalSkillsAmount: int = 1;
private var skillIndex: int = 0;

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
}
function Update () {
	if(true == uiNeedsLayout) {
		LayoutUI();
	}
}
function SetSkillIndex(index: int, skillsLength: int) {
	totalSkillsAmount = skillsLength;
	skillIndex = index;
	uiNeedsLayout = true;
}
private function LayoutUI() {
	uiNeedsLayout = false;
}
