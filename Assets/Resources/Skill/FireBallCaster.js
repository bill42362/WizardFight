#pragma strict
var skillIndex: int = 0;
var skillName: String = 'Fire Ball';
var chantTime: double = 1000;
var isChanting: boolean = false;
var owner: GameObject;
private var epochStart: System.DateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var eventCenter: EventCenter;
private var coolDownTimer: CoolDownTimer;
private var isButtonPressed: boolean = false;
private var timeStartChanting: double = 0;

function Awake () {
	coolDownTimer = GetComponent(CoolDownTimer);
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	eventCenter.RegisterListener(eventCenter, 'skillButtonDown', gameObject, OnSkillButtonDown);
	eventCenter.RegisterListener(eventCenter, 'skillButtonUp', gameObject, OnSkillButtonUp);
	eventCenter.RegisterListener(eventCenter, 'leftButtonPressed', gameObject, OnPlayerMove);
	eventCenter.RegisterListener(eventCenter, 'rightButtonPressed', gameObject, OnPlayerMove);
}
function Update () {
	if(false == isButtonPressed) {	
		isChanting = false;
		return;
	}
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	if(false == isChanting) {
		if(true == coolDownTimer.GetIsCoolDownFinished()) {
			isChanting = true;
			eventCenter.CastEvent(eventCenter, 'startChanting', gameObject);
			timeStartChanting = timestamp;
		}
	} else {
		if((timeStartChanting + chantTime) < timestamp) {
			coolDownTimer.StartCoolDown();
			isChanting = false;
			eventCenter.CastEvent(eventCenter, 'stopChanting', gameObject);
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
	) as GameObject;
}
