#pragma strict
var skillIndex: int = 0;
var skillName: String = 'Fire Ball';
var coolDownTime: double = 8000;
var chantTime: double = 1000;
var isChanting: boolean = false;
var owner: GameObject;
private var epochStart: System.DateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var eventCenter: EventCenter;
private var isButtonPressed: boolean = false;
private var timeStartCooling: double = 0;
private var timeStartChanting: double = 0;

function Awake () {
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	eventCenter.RegisterListener(eventCenter, 'skillButtonDown', gameObject, OnSkillButtonDown);
	eventCenter.RegisterListener(eventCenter, 'skillButtonUp', gameObject, OnSkillButtonUp);
	eventCenter.RegisterListener(eventCenter, 'leftButtonPressed', gameObject, OnPlayerMove);
	eventCenter.RegisterListener(eventCenter, 'rightButtonPressed', gameObject, OnPlayerMove);
}
function Update () {
	if(false == isButtonPressed) return;
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	if(false == isChanting) {
		if(true == GetIsCoolDownFinished()) {
			isChanting = true;
			timeStartChanting = timestamp;
		}
	} else {
		if((timeStartChanting + chantTime) < timestamp) {
			StartCoolDown();
			isChanting = false;
			Cast();
		}
	}
}
var OnSkillButtonDown = function(e: SbiEvent) {
	var index: int = System.Convert.ToInt32(e.data);
	if(skillIndex != index) return;
	isButtonPressed = true;
};
var OnSkillButtonUp = function(e: SbiEvent) {
	var index: int = System.Convert.ToInt32(e.data);
	if(skillIndex != index) return;
	isButtonPressed = false;
};
var OnPlayerMove = function(e: SbiEvent) {
	isChanting = false;
	isButtonPressed = false;
};
private function Cast() {
	var fireBall: GameObject = Instantiate(
		Resources.Load('Skill/FireBallBullet'), transform.position, transform.rotation
	);
}
private function StartCoolDown() {
	timeStartCooling = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
}
private function GetIsCoolDownFinished(): boolean {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	return (timeStartCooling + coolDownTime) < timestamp;
}
