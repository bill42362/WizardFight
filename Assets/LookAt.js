﻿#pragma strict
var target: GameObject;
var up: Vector3 = new Vector3(0, 1, 0);
function Awake() { target = FindWithTag('Enemy'); }
function Update () {
	if(null != target) { transform.LookAt(target.transform, up); }
}
