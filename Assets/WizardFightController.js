#pragma strict
var skillsController: SkillsController; // SkillsController.js
var playerController: PlayerController; // PlayerController.js
var enemiesController: EnemiesController; // EnemiesController.js
private var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js
private var model: WizardFightModel; // WizardFightModel.js
private var view: WizardFightView; // WizardFightView.js
private var playerSkillsPushed: boolean = false;

function Awake () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	components = app.components;
	model = app.GetComponentInChildren(WizardFightModel);
	view = app.GetComponentInChildren(WizardFightView);
	skillsController = GetComponentInChildren(SkillsController);
	playerController = GetComponentInChildren(PlayerController);
	enemiesController = GetComponentInChildren(EnemiesController);
}
function Update () {
	if(false == playerSkillsPushed) {
		skillsController.MakeAndPushSkillCasters(
			'ThunderNova', playerController.playerModel
		);
		playerSkillsPushed = true;
	}
	if(null == enemiesController.enemies) {
		enemiesController.SetEnemiesModel(model.enemies);
		enemiesController.PushEnemiesSkills(0, 'ThunderNova');
	}
	if(null == enemiesController.enemiesView) {
		enemiesController.SetEnemiesView(view.enemiesView);
	}
}
function GetPlayerController(): PlayerController {
	if(null == playerController) { playerController = GetComponentInChildren(PlayerController); }
	return playerController;
}
