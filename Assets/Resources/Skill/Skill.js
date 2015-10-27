#pragma strict
var lastStartGuideTime: double;
var coolDownTime: double = 15000;
var damagePerMillisecond: double = 0.01;
var chantTime: double = 0;
var guideTime: double = 10000;
var player: GameObject;
var enemy: GameObject;

function Start () {
	transform.position = enemy.transform.position;
}
function SetLastStartGuideTime(t: double) { lastStartGuideTime = t; }
function OnTriggerStay(other: Collider) {
	if(enemy == other.gameObject) { print(other.gameObject); }
}
