#pragma strict
static var CAST_STATE_STOPPED: String = 'stopped';
static var CAST_STATE_CHANTING: String = 'chanting';
static var CAST_STATE_GUIDING: String = 'guiding';
var player: GameObject;
var enemy: GameObject;
var skills: GameObject[] = new GameObject[0];
var castState: String = CAST_STATE_STOPPED;
private var epochStart: System.DateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var eventCenter: EventCenter;
private var startChantingTime: double;
private var castingSkill: Skill;

function Awake () {
	eventCenter = GameObject.FindWithTag('EventCenter').GetComponent(EventCenter);
	eventCenter.RegisterListener(eventCenter, 'skillButtonDown', gameObject, OnSkillButtonDown);
	eventCenter.RegisterListener(eventCenter, 'skillButtonUp', gameObject, OnSkillButtonUp);
	eventCenter.RegisterListener(eventCenter, 'leftButtonPressed', gameObject, OnPlayerMove);
	eventCenter.RegisterListener(eventCenter, 'rightButtonPressed', gameObject, OnPlayerMove);
	player = GameObject.FindWithTag('Player');
	enemy = GameObject.FindWithTag('Enemy');
	var blizzard: GameObject =  Instantiate(Resources.Load('Skill/Blizzard'));
	blizzard.name = 'Blizzard';
	var blizzardSkill = blizzard.GetComponent(Skill);
	blizzard.SetActive(false);
	blizzardSkill.player = player;
	blizzardSkill.enemy = enemy;
	AddSkill(blizzard);
}
function Update() {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	var offset: double = timestamp - startChantingTime;
	switch(castState) {
		case CAST_STATE_CHANTING:
			if(castingSkill.chantTime < offset) { StartGuiding(); }
			break;
		case CAST_STATE_GUIDING:
			if((castingSkill.chantTime + castingSkill.guideTime) < offset) {
				StopCasting();
			}
			break;
		case CAST_STATE_STOPPED: break;
		default: break;
	}
}
function AddSkill(s: GameObject) {
	var skillIndex = System.Array.IndexOf(skills, s);
	if(-1 == skillIndex) { skills = PushGameObjectArray(skills, s); }
}
var OnSkillButtonDown = function(e: SbiEvent) {
	var index: int = System.Convert.ToInt32(e.data);
	var skill: Skill;
	if(skills.Length > index) { skill = skills[index].GetComponent(Skill); }
	if((null != skill) && ((skill.lastStartGuideTime + skill.coolDownTime) < e.time)) {
		StartChanting(skill, e.time);
	}
};
var OnSkillButtonUp = function(e: SbiEvent) { StopCasting(); };
var OnPlayerMove = function(e: SbiEvent) { StopCasting(); };
private function StartChanting(s: Skill, time: double) {
	castingSkill = s;
	startChantingTime = time;
	castState = CAST_STATE_CHANTING;
}
private function StartGuiding() {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	castingSkill.SetLastStartGuideTime(timestamp);
	castingSkill.gameObject.SetActive(true);
	castState = CAST_STATE_GUIDING;
}
private function StopCasting() {
	if(null != castingSkill) {
		castingSkill.gameObject.SetActive(false);
		castingSkill = null;
	}
	castState = CAST_STATE_STOPPED;
}
private function PushGameObjectArray(array: GameObject[], item: GameObject): GameObject[] {
	var index = array.Length;
	System.Array.Resize.<GameObject>(array, array.Length + 1);
	array[index] = item;
	return array;
}
