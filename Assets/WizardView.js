#pragma strict
var wizardModel: WizardModel; // WizardModel.js

function Start () { }

function Update () {
	if(null != wizardModel) {
		UpdateView();
	}
}

private function UpdateView() {
	gameObject.transform.position = wizardModel.transform.position;
	gameObject.transform.rotation = wizardModel.transform.rotation;
	gameObject.transform.localScale = wizardModel.transform.localScale;
}
