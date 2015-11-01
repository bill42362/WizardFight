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
	var data: ChantingEventData = e.data as ChantingEventData;
	print(data.role);
}
function OnStopChanting(e: SbiEvent) {
	var data: ChantingEventData = e.data as ChantingEventData;
	print(data.role);
}
