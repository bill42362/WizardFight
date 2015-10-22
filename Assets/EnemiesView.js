#pragma strict
var enemies: Enemies; // Enemies.js
var enemyViews: GameObject[] = new GameObject[0];
var wizardViewPrefab: GameObject;

function Awake () { }
function Update () {
	if(null != enemies) {
		var models: GameObject[] = enemies.enemies;
		for(var modelCount = 0; modelCount < models.Length; ++modelCount) {
			// Create new views to meet models.
			if(!(enemyViews.Length > modelCount)) {
				var newEnemyViewGameObject: GameObject = Instantiate(wizardViewPrefab);
				enemyViews = PushGameObjectArray(enemyViews, newEnemyViewGameObject);
				newEnemyViewGameObject.SetActive(true);
			}
			enemyViews[modelCount].SetActive(models[modelCount].activeSelf);
			// Align views transform.
			enemyViews[modelCount].transform.position = models[modelCount].transform.position;
		}
		// Inactivate unused views.
		for(var viewsCount = models.Length; viewsCount < enemyViews.Length; ++viewsCount) {
			enemyViews[viewsCount].SetActive(false);
		}
	}
}
function SetEnemies(e: Enemies) { enemies = e; }
private function PushGameObjectArray(array: GameObject[], item: GameObject): GameObject[] {
	var index = array.Length;
	System.Array.Resize.<GameObject>(array, array.Length + 1);
	array[index] = item;
	return array;
}
