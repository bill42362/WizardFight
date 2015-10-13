#pragma strict
import System.Collections.Generic;
static var SKILL_STATE_CASTED: String = 'casted';
static var SKILL_STATE_CHANTING: String = 'chanting';
static var SKILL_STATE_ALERTING: String = 'alerting';
static var SKILL_STATE_CHANTED: String = 'chanted';
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js
private var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var ownerDictionary = new Dictionary.<GameObject, GameObject>();
private var skillCastersDictionary = new Dictionary.<GameObject, GameObject[]>();
private var skillSwitchesDictionary = new Dictionary.<GameObject, boolean[]>();

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	components = app.components;
}
function Update () {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	MoveCastersToOwners();
	UpdateSkillCastersStateByTime(timestamp);
}
var OnSkillStateChanged = function(e: SbiEvent) {
	if('chanted' == e.data) {
		var caster = e.target as GameObject;
		caster.SetActive(false);
		var owner = ownerDictionary[caster];
		var casterArray = skillCastersDictionary[owner];
		var casterIndex: int = System.Array.IndexOf(casterArray, caster);
		var nextCasterIndex: int = casterIndex + 1;
		if(casterArray.Length == nextCasterIndex) {
			nextCasterIndex = 0;
		}
	}
};
function AddSkillCaster(caster: GameObject, owner: GameObject): int {
	if(null == eventCenter) { Start(); }
	var casterIndex: int = -1;
	var ownerExist = skillCastersDictionary.ContainsKey(owner);
	if(false == ownerExist) {
		skillCastersDictionary[owner] = new GameObject[0];
		skillSwitchesDictionary[owner] = new boolean[0];
	}
	var skillRepeated = false;
	var casterArray = skillCastersDictionary[owner];
	for(var casterCounter = 0; casterCounter < casterArray.length; ++casterCounter) {
		if(casterArray[casterCounter].name == caster.name) {
			skillRepeated = true;
			casterIndex = casterCounter;
		}
	}
	if(false == skillRepeated) {
		ownerDictionary[caster] = owner;
		casterIndex = skillCastersDictionary[owner].Length;
		skillCastersDictionary[owner] = PushGameObjectArray(
			skillCastersDictionary[owner], caster
		);
		skillSwitchesDictionary[owner] = PushBooleanArray(
			skillSwitchesDictionary[owner], true
		);
		eventCenter.RegisterListener(caster, 'skillStateChanged', this, OnSkillStateChanged);
	}
	return casterIndex;
}
private function MoveCastersToOwners() {
	var e = ownerDictionary.GetEnumerator();
	while(e.MoveNext()) {
		var caster = e.Current.Key;
		if(caster.activeSelf) {
			var owner = e.Current.Value;
			caster.transform.position = owner.transform.position;
		}
	}
}
private function UpdateSkillCastersStateByTime(t: double) {
	var e = ownerDictionary.GetEnumerator();
	while(e.MoveNext()) {
		var caster = e.Current.Key;
		if(caster.activeSelf) {
			caster.GetComponent(SkillCaster).UpdateSkillStateByTime(t);
		}
	}
}
private function PushGameObjectArray(array: GameObject[], item: GameObject): GameObject[] {
	var index = array.Length;
	System.Array.Resize.<GameObject>(array, array.Length + 1);
	array[index] = item;
	return array;
}
private function PushBooleanArray(array: boolean[], item: boolean): boolean[] {
	var index = array.Length;
	System.Array.Resize.<boolean>(array, array.Length + 1);
	array[index] = item;
	return array;
}
