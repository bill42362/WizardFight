#pragma strict
private var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var appearTime: double;
private var lifeTime: double = 1000.0;
private var damage: double = 1.0;

function Start () { }
function Update () {
	if(lifeTime < GetAge()) { gameObject.SetActive(false); }
}
function SetAppearTime(a: double) { appearTime = a; }
function GetAge(): double { 
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	var age: double = timestamp - appearTime;
	return age;
}
