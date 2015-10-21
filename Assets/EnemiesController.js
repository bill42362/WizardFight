#pragma strict
var enemies: Enemies; // Enemies.js
var enemiesView: EnemiesView; // EnemiesView.js

function Awake () { }
function Update () { }
function SetEnemiesModel(e: Enemies) { enemies = e; }
function SetEnemiesView(e: EnemiesView) {
	enemiesView = e;
	enemiesView.SetEnemies(enemies);
}
