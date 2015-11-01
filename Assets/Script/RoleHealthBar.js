#pragma strict
var role: Role;
private var eventCenter: EventCenter;
private var slider: Slider;

function Awake () {
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	eventCenter.RegisterListener(eventCenter, 'playerChanged', gameObject, OnPlayerChanged);
	slider = GetComponent(Slider);
}
function Update () {
	if(null != role) {
		slider.value = role.health/role.maxHealth;
	}
}
function OnPlayerChanged(e: SbiEvent) {
	var roleGameObject = e.data as GameObject;
	role = roleGameObject.GetComponent(Role);
}
