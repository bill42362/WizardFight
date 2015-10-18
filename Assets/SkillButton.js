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
	var transform = gameObject.transform;
	var boxCollider = gameObject.GetComponent(BoxCollider);
	var defaultSize = boxCollider.size;
	var width: float = Mathf.Min(MAX_WIDTH, Screen.width/totalSkillsAmount);
	var height: float = width;
	var newSize: Vector3 = new Vector3(width/defaultSize.x, height/defaultSize.y, 1);
	var newX: float = 0.5*(Screen.width - width);
	var newY: float = -0.5*(Screen.height - height);
	var newCenter: Vector3 = new Vector3(newX, newY, 0);
	gameObject.transform.localScale = newSize;
	gameObject.transform.localPosition = newCenter;

	uiNeedsLayout = false;
}
