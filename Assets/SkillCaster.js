#pragma strict
var castingTime: double = 4.0;
var alertTime: double = 2.0;
var skillState: String = SkillsController.SKILL_STATE_ALERTING;
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js
private var skillsController: SkillsController; // SkillsController.js
private var startCastingTime: double = Mathf.NegativeInfinity;
private var castCallback: Function;

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	components = app.components;
	skillsController = components.SkillsController;
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
		eventCenter.CastEvent(gameObject, 'skillStateChanged', newSkillState);
		skillState = newSkillState;
		if((SkillsController.SKILL_STATE_CHANTED) && (null != castCallback)) {
			castCallback(t - GetStateOffsetByTime(t));
			skillState = SkillsController.SKILL_STATE_CASTED;
		}
	}
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
function SetCastCallback(c: Function) { castCallback = c; }
