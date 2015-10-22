#pragma strict
var health: double = 100;

function Start () { }
function Update () { }
function TakeDamage(d: double) {
	health -= d;
	if(0 >= health) { gameObject.SetActive(false); }
}
