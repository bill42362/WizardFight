#pragma strict
var skillIndex: int = 2;
var skillName: String = 'Thunder Nova';
var owner: GameObject;
private var epochStart: System.DateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var eventCenter: EventCenter;
private var coolDownTimer: CoolDownTimer;
private var isButtonPressed: boolean = false;
private var timeStartCooling: double = 0;

function Awake () {
	coolDownTimer = GetComponent(CoolDownTimer);
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	eventCenter.RegisterListener(eventCenter, 'skillButtonDown', gameObject, OnSkillButtonDown);
	eventCenter.RegisterListener(eventCenter, 'skillButtonUp', gameObject, OnSkillButtonUp);
}
function Update () {
	if(
		(true == isButtonPressed)
		&& (true == coolDownTimer.GetIsCoolDownFinished())
	) {
		coolDownTimer.StartCoolDown();
		Cast();
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
private function Cast() {
	var nova: GameObject = Instantiate(
		Resources.Load('Skill/ThunderNova'), transform.position, transform.rotation
	) as GameObject;
}
