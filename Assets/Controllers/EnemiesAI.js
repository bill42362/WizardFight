#pragma strict
function GetEnemieForce(ePos: Vector3, pPos: Vector3, pStat: String): Vector3 {
	var force: Vector3 = new Vector3(0, 0, -5);
	switch(pStat) {
		case 'chanting':
			force = pPos - ePos;
			break;
		case 'alerting':
			force = ePos - pPos;
			break;
		default: break;
	}
	return force;
}
