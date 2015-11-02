using UnityEngine;

public class ChantingEventData : SbiEventData {
	public string type;
	public GameObject role;
	public GameObject caster;
	public double chantTime;
	public ChantingEventData(string t, GameObject r, GameObject c) {
		type = t;
		role = r;
		caster = c;
	}
	public ChantingEventData(string t, GameObject r, GameObject c, double time) {
		type = t;
		role = r;
		caster = c;
		chantTime = time;
	}
}
