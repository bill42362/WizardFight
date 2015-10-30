#pragma strict
var caster: GameObject;
private var eventCenter: EventCenter;
private var slider: Slider;

function Awake () {
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	eventCenter.RegisterListener(eventCenter, 'startChanting', gameObject, OnStartChanting);
	eventCenter.RegisterListener(eventCenter, 'stopChanting', gameObject, OnStopChanting);
	slider = GetComponent(Slider);
}

function Update () {
}
function OnStartChanting(e: SbiEvent) {
	print('start');
}
function OnStopChanting(e: SbiEvent) {
	print('stop');
}
