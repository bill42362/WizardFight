#pragma strict
private var hearings: Array = new Array();
private var epochStart: System.DateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
function CastEvent(caster: Object, type: String, data: Object) {
	for(var i = 0; i < hearings.length; ++i) {
		var h: Hearing = hearings[i] as Hearing;
		if((caster == h.target) && (type == h.type)) {
			var e = new SbiEvent();
			e.target = caster;
			e.listener = h.listener;
			e.type = h.type;
			e.data = data;
			e.time = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
			h.method(e);
		}
	}
}
function RegisterListener(target: Object, type: String, listener: Object, method: Function): int {
	var eventId: int = -1;
	for(var i = 0; i < hearings.length; ++i) {
		var h: Hearing = hearings[i] as Hearing;
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
	var time: double;
}
