#pragma strict
var enemyCount: int = 1;
var enemies: GameObject[] = new GameObject[0];

function Awake () {
	for(var i = 0; i < enemyCount; ++i) {
		var newEnemyGameObject = new GameObject();
		newEnemyGameObject.transform.position = new Vector3(0, 0, 5);
		newEnemyGameObject.AddComponent(WizardModel);
		newEnemyGameObject.name = 'EnemyWizard';
		newEnemyGameObject.transform.parent = gameObject.transform;
		enemies = PushGameObjectArray(enemies, newEnemyGameObject);
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
