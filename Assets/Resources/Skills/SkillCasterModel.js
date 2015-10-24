﻿#pragma strict
var skillName: String;
var castingTime: double = 4000.0;
var alertTime: double = 2000.0;
var skillState: String = SkillsController.SKILL_STATE_ALERTING;
var skillColor: Color = Color(0.2, 0.6, 0.8, 1);
var updatedByModel: boolean = false;
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js
private var skillsController: SkillsController; // SkillsController.js
private var startCastingTime: double = -1;
private var castCallback: Function;

function Awake () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	components = app.components;
	skillsController = app.controller.skillsController;
}
function Update () { }
function UpdateStartCastingTime(t: double) {
	startCastingTime = t;
}
function UpdateSkillStateByTime(t: double) {
	var offset = t - startCastingTime;
	var newSkillState: String = SkillsController.SKILL_STATE_CASTED;
	if(castingTime < offset) {
		if(skillState != SkillsController.SKILL_STATE_CASTED) {
			newSkillState = SkillsController.SKILL_STATE_CHANTED;
		}
	} else if((castingTime - alertTime) < offset) {
		newSkillState = SkillsController.SKILL_STATE_ALERTING;
	} else if(0 < offset) {
		newSkillState = SkillsController.SKILL_STATE_CHANTING;
	}
	if(skillState != newSkillState) {
		var data = new SkillStateChangeEventData();
		data.oldState = skillState;
		data.newState = newSkillState;
		skillState = newSkillState;
		data.time = t - GetStateOffsetByTime(t);
		data.caster = this.gameObject;
		eventCenter.CastEvent(gameObject, 'skillStateChanged', data as Object);
	}
}
function GetState(): String { return skillState; }
function GetChantedTimeByTime(t: double): double {
	return t - startCastingTime;
}
function GetStateOffsetByTime(t: double): double {
	var offset: double = 0;
	switch(skillState) {
		case SkillsController.SKILL_STATE_CHANTING:
			offset = t - startCastingTime;
			break;
		case SkillsController.SKILL_STATE_ALERTING:
			offset = t - (startCastingTime - alertTime);
			break;
		case SkillsController.SKILL_STATE_CHANTED:
			offset = t - (startCastingTime + castingTime);
			break;
		default: break;
	}
	return offset;
}
function SetCastingTime(c: double) { castingTime = c; }
function SetAlertTime(a: double) { alertTime = a; }
function SetSkillName(n: String) { skillName = n; }
function SetSkillColor(c: Color) { skillColor = c; }
function SetSkillUpdated() { updatedByModel = true; }
function SetCastCallback(c: Function) { castCallback = c; }
function CallCastCallbackByCastTime(t: double) {
	if((SkillsController.SKILL_STATE_CHANTED == skillState) && (null != castCallback)) {
		castCallback(t);
		skillState = SkillsController.SKILL_STATE_CASTED;
	}
}

class SkillStateChangeEventData {
	var caster: GameObject;
	var oldState: String;
	var newState: String;
	var time: double;
}