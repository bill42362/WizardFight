#pragma strict
var eventName: String;
var direction: Vector3;
function Awake () {
	if(null != eventName) {
		var eventCenter: EventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
		eventCenter.RegisterListener(eventCenter, eventName, gameObject, OnEventTriggered);
	}
}
var OnEventTriggered = function(e: SbiEvent) {
	var rigidbody: Rigidbody = GetComponent(Rigidbody);
	var speed: double = 10;
	var role: Role = GetComponent(Role);
	if(null != role) { speed = role.speed; }
	var velocity: Vector3 = transform.localToWorldMatrix.MultiplyVector(direction*speed);
	if((null != direction) && (null != rigidbody)) {
		rigidbody.velocity = velocity;
	}
};
