#pragma strict
private var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js
private var model: WizardFightModel; // WizardFightModel.js
private var view: WizardFightView; // WizardFightView.js
private var playerController: PlayerController; // PlayerController.js
private var enemiesController: EnemiesController; // EnemiesController.js
private var skillsController: SkillsController; // SkillsController.js
private var playerSkillsPushed: boolean = false;

function Awake () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	components = app.components;
	model = app.model;
	view = app.view;
	playerController = GetComponentInChildren(PlayerController);
	enemiesController = GetComponentInChildren(EnemiesController);
}
function Update () {
	if(null == model) { model = app.model; }
	if(null == view) { view = app.view; }
	if(null == model.battleFieldModel) {
		var battleFieldModel = Instantiate(components.BattleFieldModel);
		model.battleFieldModel = battleFieldModel;
		battleFieldModel.gameObject.SetActive(true);
		var battleFieldView = Instantiate(components.BattleFieldView);
		battleFieldView.gameObject.SetActive(true);
		view.battleFieldView = battleFieldView;
	}
	if(null == playerController.playerModel) {
		playerController.SetPlayerModel(components.WizardModel.gameObject);
	}
	if(null == playerController.playerView) {
		playerController.SetPlayerView(components.WizardView.gameObject);
	}
	if(null == skillsController) {
		skillsController = Instantiate(components.SkillsController);
		skillsController.gameObject.SetActive(true);
	}
	if(false == playerSkillsPushed) {
		skillsController.MakeAndPushSkillCasters(
			'ThunderNova', playerController.playerModel
		);
		playerSkillsPushed = true;
	}
	if(null == enemiesController.enemies) {
		enemiesController.SetEnemiesModel(model.enemies);
	}
	if(null == enemiesController.enemiesView) {
		enemiesController.SetEnemiesView(view.enemiesView);
	}
}
