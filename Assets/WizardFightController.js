#pragma strict
private var epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js
private var eventCenter: EventCenter; // EventCenter.js
private var model: WizardFightModel; // WizardFightModel.js
private var view: WizardFightView; // WizardFightView.js
private var playerController: PlayerController; // PlayerController.js
private var skillsController: SkillsController; // SkillsController.js
private var playerSkillsPushed: boolean = false;

function Start () {
	app = WizardFightApplication.Shared();
	eventCenter = app.eventCenter;
	components = app.components;
	model = app.model;
	view = app.view;
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
	if(null == playerController) {
		playerController = Instantiate(components.PlayerController);
		playerController.gameObject.SetActive(true);
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
		MakeAndPushPlayerSkillCasters();
	}
}
private function MakeAndPushPlayerSkillCasters() {
	var timestamp: double = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	var thunderNovaCasterModel = Instantiate(components.ThunderNovaCasterModel);
	thunderNovaCasterModel.gameObject.AddComponent(SkillCaster);
	thunderNovaCasterModel.UpdateSkillCaster();
	thunderNovaCasterModel.GetComponent(SkillCaster).UpdateStartCastingTime(timestamp);
	skillsController.AddSkillCaster(thunderNovaCasterModel.gameObject, playerController.playerModel);
	thunderNovaCasterModel.gameObject.SetActive(true);
	var thunderNovaCasterView = Instantiate(components.ThunderNovaCasterView);
	thunderNovaCasterView.SetModel(thunderNovaCasterModel);
	thunderNovaCasterView.gameObject.SetActive(true);
	playerSkillsPushed = true;
}
