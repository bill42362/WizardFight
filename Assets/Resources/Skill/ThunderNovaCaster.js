#pragma strict
var skillIndex: int = 2;
var skillName: String = 'Thunder Nova';
var coolDownTime: double = 5000;
var owner: GameObject;
private var epochStart: System.DateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var eventCenter: EventCenter;
private var isButtonPressed: boolean = false;
private var timeStartCooling: double = 0;

function Awake () {
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	eventCenter.RegisterListener(eventCenter, 'skillButtonDown', gameObject, OnSkillButtonDown);
	eventCenter.RegisterListener(eventCenter, 'skillButtonUp', gameObject, OnSkillButtonUp);
}
function Update () {
	if((true == isButtonPressed) && (true == GetIsCoolDownFinished())) {
		StartCoolDown();
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
private function StartCoolDown() {
	timeStartCooling = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
}
private function GetIsCoolDownFinished(): boolean {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	return (timeStartCooling + coolDownTime) < timestamp;
}
