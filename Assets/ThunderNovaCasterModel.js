﻿#pragma strict
var thunderNovaGameObject: GameObject;
var castingTime: double = 4.0;
var alertTime: double = 2.0;
var skillState: String = SkillsController.SKILL_STATE_WAITING;
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js
private var skillsController: SkillsController; // SkillsController.js
private var startCastingTime: double = -1.0;

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	components = app.components;
	skillsController = components.SkillsController;
	thunderNovaGameObject = components.ThunderNovaModel.gameObject;
}
function Update () { }
function UpdateStartCastingTime(t: double) {
	startCastingTime = t;
}
function UpdateSkillStateByTime(t: double) {
	var offset = t - startCastingTime;
	var newSkillState: String = SkillsController.SKILL_STATE_WAITING;
	if((castingTime < offset) && (skillState != SkillsController.SKILL_STATE_WAITING)) {
		newSkillState = SkillsController.SKILL_STATE_CASTED;
	} else if((castingTime - alertTime) < offset) {
		newSkillState = SkillsController.SKILL_STATE_ALERTING;
	} else if(0 < offset) {
		newSkillState = SkillsController.SKILL_STATE_CASTING;
	}
	if(skillState != newSkillState) {
		eventCenter.CastEvent(this, 'skillStateChanged', newSkillState);
		skillState = newSkillState;
	}
}
function GetStateOffsetByTime(t: double): double {
	var offset = 0;
	switch(skillState) {
		case SkillsController.SKILL_STATE_CASTING:
			offset = t - startCastingTime;
			break;
		case SkillsController.SKILL_STATE_ALERTING:
			offset = t - (startCastingTime - alertTime);
			break;
		case SkillsController.SKILL_STATE_CASTED:
			offset = t - (startCastingTime + castingTime);
			break;
		default: break;
	}
	return offset;
}
function GetSkillGameObject(): GameObject { return thunderNovaGameObject; }

