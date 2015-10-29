#pragma strict
var coolDownTime: double = 8000;
private var epochStart: System.DateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var timeStartCooling: double = 0;

function StartCoolDown() {
	timeStartCooling = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
}
function GetIsCoolDownFinished(): boolean {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	return (timeStartCooling + coolDownTime) < timestamp;
}
function GetRemainCoolDownTime(): double {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	return 0.001*(timeStartCooling + coolDownTime - timestamp);
}
