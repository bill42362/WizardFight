#pragma strict
private var hearings: Array = new Array();
function castEvent(caster: Object, type: String, data: Object) {
	for(var i = 0; i < hearings.length; ++i) {
		var h: Hearing = hearings[i];
		if((caster == h.target) && (type == h.type)) {
			var e = new SbiEvent();
			e.target = caster;
			e.listener = h.listener;
			e.type = h.type;
			e.data = data;
			h.method(e);
		}
	}
}
function registerListener(target: Object, type: String, listener: Object, method: Function): int {
	var eventId: int = -1;
	for(var i = 0; i < hearings.length; ++i) {
		var h: Hearing = hearings[i];
		if((target == h.target) && (type == h.type) && (listener == h.listener)) {
			eventId = i;
		}
	}
	if(-1 == eventId) {
		eventId = hearings.length;
		var hearing = new Hearing();
		hearing.target = target;
		hearing.listener = listener;
		hearing.type = type;
		hearing.method = method;
		hearings.push(hearing);
	}
	return eventId;
}

class Hearing {
	var target: Object;
	var listener: Object;
	var type: String;
	var method: Function;
}
class SbiEvent {
	var target: Object;
	var listener: Object;
	var type: String;
	var data: Object;
}
