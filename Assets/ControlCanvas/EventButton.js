#pragma strict
import UnityEngine.UI;
var eventName: String;
private var eventCenter: EventCenter;

function Awake () {
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	GetComponent(Button).onClick.AddListener(OnClick);
}
function OnClick() {
	if(null != eventName) {
		eventCenter.CastEvent(this, eventName, null);
	}
}
