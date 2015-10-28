#pragma strict
var owner: GameObject;
var damage: double = 10;
var flyingSpeed: double = 10;
var lifeTime: double = 1000;
private var epochStart: System.DateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var startTime: double;
function Start () {
	GetComponent(Rigidbody).velocity = transform.forward*flyingSpeed;
	startTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
}
function Update() {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	if((startTime + lifeTime) < timestamp) { Destroy(gameObject); }
}

