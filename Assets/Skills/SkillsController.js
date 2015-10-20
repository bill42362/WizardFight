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
private var playerGameObject: GameObject;
private var playerSkillCasterButtons: GameObject[] = new GameObject[0];

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
	var data: SkillStateChangeEventData = e.data as SkillStateChangeEventData;
	if(SkillsController.SKILL_STATE_CHANTED == data.newState) {
		var caster = e.target as GameObject;
		caster.GetComponent(SkillCaster).CallCastCallbackByCastTime(data.time);
		caster.SetActive(false);
		var nextCaster = PickNextCaster(caster);
		if(null != nextCaster) {
			nextCaster.GetComponent(SkillCaster).UpdateStartCastingTime(data.time);
			nextCaster.SetActive(true);
		}
	}
};
function AddSkillCaster(caster: GameObject, owner: GameObject): int {
	if(null == eventCenter) { Start(); }
	var casterIndex: int = -1;
	if((null == playerGameObject) && ('player' == owner.name)) {
		playerGameObject = owner;
	}
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
		if(playerGameObject == owner) {
			AddPlayerSkillCasterButton(caster);
		}
	}
	return casterIndex;
}
private function AddPlayerSkillCasterButton(caster: GameObject) {
	var allButtons: GameObject[] = playerSkillCasterButtons;
	var skillIndex: int = allButtons.Length;
	var newButton: SkillButton = Instantiate(components.SkillButton);
	var skillsPanel: Image = app.view.skillsPanel;
	newButton.transform.SetParent(skillsPanel.transform);
	newButton.skillName = caster.GetComponent(SkillCaster).skillName;
	allButtons = PushGameObjectArray(allButtons, newButton.gameObject);
	for(var i = 0; i < allButtons.Length; ++i) {
		allButtons[i].GetComponent(SkillButton).SetSkillIndex(i, allButtons.Length);
	}
	newButton.gameObject.SetActive(true);
	playerSkillCasterButtons = allButtons;
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
private function PickNextCaster(caster: GameObject): GameObject {
	var nextCaster: GameObject;
	caster.SetActive(false);
	var owner = ownerDictionary[caster];
	var casterArray: GameObject[] = skillCastersDictionary[owner];
	var switchArray: boolean[] = skillSwitchesDictionary[owner];
	var nextCasterIndex: int = 1 + System.Array.IndexOf(casterArray, caster);
	if(switchArray.Length == nextCasterIndex) {
		nextCasterIndex = 0;
	}
	var pickTimes: int = switchArray.Length;
	while((false == switchArray[nextCasterIndex]) && (0 < pickTimes)) {
		nextCasterIndex++;
		if(switchArray.Length == nextCasterIndex) {
			nextCasterIndex = 0;
		}
		pickTimes--;
	}
	if(true == switchArray[nextCasterIndex]) {
		nextCaster = casterArray[nextCasterIndex];
	}
	return nextCaster;
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
