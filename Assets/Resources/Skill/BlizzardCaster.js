#pragma strict
var skillIndex: int = 1;
var skillName: String = 'Blizzard';
var coolDownTime: double = 15000;
var guidingTime: double = 10000;
var isGuiding: boolean = false;
var owner: GameObject;
var enemy: GameObject;
private var epochStart: System.DateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var eventCenter: EventCenter;
private var isButtonPressed: boolean = false;
private var timeStartCooling: double = 0;
private var timeStartGuiding: double = 0;
private var blizzard: GameObject;

function Awake () {
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	eventCenter.RegisterListener(eventCenter, 'skillButtonDown', gameObject, OnSkillButtonDown);
	eventCenter.RegisterListener(eventCenter, 'skillButtonUp', gameObject, OnSkillButtonUp);
	eventCenter.RegisterListener(eventCenter, 'leftButtonPressed', gameObject, OnPlayerMove);
	eventCenter.RegisterListener(eventCenter, 'rightButtonPressed', gameObject, OnPlayerMove);
}
function Update () {
	if(false == isButtonPressed) {	
		if(true == isGuiding) {
			StopGuiding();
			isGuiding = false;
		}
		return;
	}
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	if(false == isGuiding) {
		if(true == GetIsCoolDownFinished()) {
			isGuiding = true;
			timeStartGuiding = timestamp;
			StartGuiding();
			StartCoolDown();
		}
	} else {
		if((timeStartGuiding + guidingTime) < timestamp) {
			isGuiding = false;
			StopGuiding();
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
	isGuiding = false;
	isButtonPressed = false;
};
private function StartGuiding() {
	var targetPosition: Vector3 = transform.position;
	if(null != enemy) { targetPosition = enemy.transform.position; }
	if(null == blizzard) {
		blizzard = Instantiate(
			Resources.Load('Skill/Blizzard'), targetPosition, transform.rotation
		) as GameObject;
	} else {
		blizzard.transform.position = targetPosition;
	}
	blizzard.SetActive(true);
}
private function StopGuiding() { blizzard.SetActive(false); }
private function StartCoolDown() {
	timeStartCooling = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
}
private function GetIsCoolDownFinished(): boolean {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	return (timeStartCooling + coolDownTime) < timestamp;
}
