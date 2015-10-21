#pragma strict
import UnityEngine.UI;
var skillName: String;
var slider: Slider;
private var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var app: WizardFightApplication; // WizardFightApplication.js
private var eventCenter: EventCenter; // EventCenter.js
private var rectTransform: RectTransform;
private var uiNeedsLayout: boolean = false;
private var totalSkillsAmount: int = 1;
private var skillIndex: int = 0;
private var skillCaster: SkillCaster; // SkillCaster.js
private var image: Image;

function Awake () {
	rectTransform = GetComponent(RectTransform);
	image = GetComponent(Image);
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	GetComponent(Button).onClick.AddListener(OnClick);
}
function Update () {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	if(true == uiNeedsLayout) { LayoutUI(); }
	slider.value = skillCaster.GetChantedTimeByTime(timestamp)/skillCaster.castingTime;
	image.color = skillCaster.skillColor;
	if(false == skillCaster.updatedByModel) { uiNeedsLayout = true; }
}
function SetSkillIndex(index: int, skillsLength: int) {
	totalSkillsAmount = skillsLength;
	skillIndex = index;
	uiNeedsLayout = true;
}
function SetSkillCaster(c: SkillCaster) { skillCaster = c; }
function OnClick() {
	eventCenter.CastEvent(this, 'skillbuttonclicked', skillIndex);
}
private function LayoutUI() {
	var text: Text = GetComponentInChildren(Text);
	text.text = skillCaster.skillName;
	skillName = skillCaster.skillName;
	var parentRect: Rect = rectTransform.parent.GetComponent(RectTransform).rect;
	var parentScale: Vector3 = rectTransform.parent.localScale;
	var height: float = parentRect.height*parentScale.y;
	rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, height);
	rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, height);
	uiNeedsLayout = false;
}
