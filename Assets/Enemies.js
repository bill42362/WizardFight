#pragma strict
var enemyCount: int = 1;
var enemies: GameObject[] = new GameObject[0];
var wizardModelPrefab: WizardModel; // WizardModel.js
private var app: WizardFightApplication; // WizardFightApplication.js
private var components: WizardFightComponents; // WizardFightComponents.js

function Awake () {
	app = WizardFightApplication.Shared();
	wizardModelPrefab = Instantiate(Resources.Load("Wizard/WizardModel", WizardModel));
	wizardModelPrefab.gameObject.SetActive(false);
	for(var i = 0; i < enemyCount; ++i) {
		var newEnemyGameObject = Instantiate(wizardModelPrefab);
		newEnemyGameObject.transform.position = new Vector3(0, 0, 5);
		newEnemyGameObject.name = 'EnemyWizard';
		newEnemyGameObject.tag = 'Enemy';
		newEnemyGameObject.transform.parent = gameObject.transform;
		enemies = PushGameObjectArray(enemies, newEnemyGameObject.gameObject);
		newEnemyGameObject.gameObject.SetActive(true);
	}
}
function Update () {
	if(enemyCount != enemies.Length) { Awake(); }
}
private function PushGameObjectArray(array: GameObject[], item: GameObject): GameObject[] {
	var index = array.Length;
	System.Array.Resize.<GameObject>(array, array.Length + 1);
	array[index] = item;
	return array;
}
