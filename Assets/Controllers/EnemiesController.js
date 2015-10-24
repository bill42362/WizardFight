#pragma strict
var skillsController: SkillsController; // SkillsController.js
var playerController: PlayerController; // PlayerController.js
var enemies: Enemies; // Enemies.js
var enemiesView: EnemiesView; // EnemiesView.js
var enemiesAI: EnemiesAI; // EnemiesAI.js
private var app: WizardFightApplication; // WizardFightApplication.js
private var bufferedEnemiesSkills = new Dictionary.<int, String>();

function Awake () {
	app = WizardFightApplication.Shared();
	var controller: WizardFightController = app.controller;
	if(null != controller) {
		skillsController = controller.skillsController;
		playerController = controller.playerController;
	}
	enemiesAI = GetComponent(EnemiesAI);
}
function Update () {
	if(null == skillsController) { Awake(); }
	if(null != skillsController) {
		var e = bufferedEnemiesSkills.GetEnumerator();
		while(e.MoveNext()) {
			skillsController.MakeAndPushSkillCasters(
				e.Current.Value, enemies.enemies[e.Current.Key]
			);
		}
		bufferedEnemiesSkills.Clear();
	}
	if(null != enemies) {
		UpdateEnemiesForces();
	}
}
function SetEnemiesModel(e: Enemies) { enemies = e; }
function SetEnemiesView(e: EnemiesView) {
	enemiesView = e;
	enemiesView.SetEnemies(enemies);
}
function PushEnemiesSkills(enemyIndex: int, skillName: String) {
	if(null != skillsController) {
		skillsController.MakeAndPushSkillCasters(skillName, enemies.enemies[enemyIndex]);
	} else {
		bufferedEnemiesSkills[enemyIndex] = skillName;
	}
}
private function UpdateEnemiesForces() {
	var playerPosition = playerController.GetPlayerPosition();
	var playerState = playerController.GetPlayerChantintState();
	var enemyGameObjects: GameObject[] = enemies.enemies;
	for(var i = 0; i < enemyGameObjects.Length; ++i) {
		var enemyForce = enemiesAI.GetEnemieForce(
			enemyGameObjects[i].transform.position, playerPosition, playerState
		);
		enemyGameObjects[i].GetComponent(Rigidbody).AddForce(enemyForce);
	}
}
