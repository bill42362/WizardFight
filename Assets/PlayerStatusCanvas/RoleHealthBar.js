#pragma strict
var roleGameObject: GameObject;
private var slider: Slider;

function Awake () { slider = GetComponent(Slider); }
function Update () {
	roleGameObject = GameObject.FindWithTag('Player');
	if(null != roleGameObject) {
		var role: Role = roleGameObject.GetComponent(Role);
		slider.value = role.health/role.maxHealth;
	}
}
