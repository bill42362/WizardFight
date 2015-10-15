#pragma strict
private var model: ThunderNovaModel; // ThunderNovaModel.js

function Start () { }
function Update () {
	if(null != model) {
		gameObject.GetComponent(Renderer).enabled = model.gameObject.activeSelf;
		transform.position = model.transform.position;
	}
}
function SetModel(m: ThunderNovaModel) { model = m; }
