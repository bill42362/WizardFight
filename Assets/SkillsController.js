#pragma strict
static var SKILL_STATE_WAITING: String = 'waiting';
static var SKILL_STATE_CASTING: String = 'casting';
static var SKILL_STATE_ALERTING: String = 'alerting';
static var SKILL_STATE_CASTED: String = 'casted';
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js
private var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var skillCasters: Array = new Array();
private var skillCasterOwners: Array = new Array();
private var wizardObjectList: Array = new Array();
private var wizardSkillCasterLists: Array = new Array();
private var wizardSkillSwitchLists: Array = new Array();

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	components = app.components;
}
function Update () {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	MoveCastersToOwners();
}
function MoveCastersToOwners() {
	for(var i = 0; i < skillCasters.length; ++i) {
		(skillCasters[i] as GameObject).transform.position
		= (skillCasterOwners[i] as GameObject).transform.position;
	}
}
function AddSkillCaster(sc: GameObject, owner: GameObject): int {
	var casterIndex = skillCasters.length;
	var caster = Instantiate(sc);
	skillCasters.push(caster);
	skillCasterOwners.push(owner);
	//var wizardIndex = System.Array.IndexOf(wizardObjectList as GameObject[], owner);
	var wizardIndex = -1;
	for(var wizardCounter = 0; wizardCounter < wizardObjectList.length; ++wizardCounter) {
		if(owner == wizardObjectList[wizardCounter]) {
			wizardIndex = wizardCounter;
		}
	}
	if(-1 == wizardIndex) {
		wizardIndex = wizardObjectList.length;
		wizardObjectList.Push(owner);
		var skillList = new Array();
		wizardSkillCasterLists.Push(skillList);
		var skillSwitchList = new Array();
		wizardSkillSwitchLists.Push(skillSwitchList);
	}
	var skillRepeated = false;
	var tempList = wizardSkillCasterLists[wizardIndex] as Array;
	for(var casterCounter = 0; casterCounter < tempList.length; ++casterCounter) {
		if((tempList[casterCounter] as GameObject).name == caster.name) {
			skillRepeated = true;
		}
	}
	if(false == skillRepeated) {
		(wizardSkillCasterLists[wizardIndex] as Array).Push(caster);
		(wizardSkillSwitchLists[wizardIndex] as Array).Push(true);
	}
	return casterIndex;
}
