#pragma strict
var type: String;
var role: GameObject;
var caster: GameObject;
class ChantingEventData {
	function ChantingEventData(t: String, r: GameObject, c: GameObject) {
		type = t;
		role = r;
		caster = c;
	}
}
