#pragma strict
private var model: ThunderNovaCasterModel; // ThunderNovaCasterModel.js
private var rendererObject: Renderer;

function Start () {
	rendererObject = this.GetComponent.<Renderer>();
}
function Update () {
	if(null == model) {
		model = GetComponent(SkillCasterView).model.GetComponent(ThunderNovaCasterModel);
	}
	if(null != model) {
		rendererObject.enabled = model.gameObject.activeSelf;
		var newPosition = model.transform.position;
		newPosition.y = 0;
		transform.position = newPosition;
		var renderColor: Color = Color(0.0, 0.0, 0.0, 0.0);
		switch(model.GetComponent(SkillCasterModel).GetState()) {
			case SkillsController.SKILL_STATE_CHANTING:
				renderColor = Color(0.2, 0.6, 0.8, 0.5);
				break;
			case SkillsController.SKILL_STATE_ALERTING:
				renderColor = Color(0.8, 0.2, 0.6, 0.5);
				break;
			default:
				break;
		}
		rendererObject.material.color = renderColor;
	}
}
