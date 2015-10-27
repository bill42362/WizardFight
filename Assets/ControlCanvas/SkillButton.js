#pragma strict
import UnityEngine.UI;
var skillIndex: int = 0;
private var eventCenter: EventCenter;

function Awake () {
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	GetComponent(Button).onClick.AddListener(OnClick);
}
function OnClick() { }
function OnPointerDown() {
	eventCenter.CastEvent(this, 'skillButtonDown', skillIndex);
}
function OnPointerUp() {
	eventCenter.CastEvent(this, 'skillButtonUp', skillIndex);
}
