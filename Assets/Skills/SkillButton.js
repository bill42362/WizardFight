#pragma strict
import UnityEngine.UI;
var skillName: String;
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js
private var rectTransform: RectTransform;
private var uiNeedsLayout: boolean = false;
private var totalSkillsAmount: int = 1;
private var skillIndex: int = 0;

function Awake () {
	rectTransform = GetComponent(RectTransform);
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
}
function Start () { }
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
	var text: Text = GetComponentInChildren(Text);
	text.text = skillName;
	var parentRect: Rect = rectTransform.parent.GetComponent(RectTransform).rect;
	var parentScale: Vector3 = rectTransform.parent.localScale;
	var height: float = parentRect.height*parentScale.y;
	rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, height);
	rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, height);
	uiNeedsLayout = false;
}
