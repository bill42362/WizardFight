using UnityEngine;

public class ChantingEventData : Object {
	public string type;
	public GameObject role;
	public GameObject caster;
	public ChantingEventData(string t, GameObject r, GameObject c) {
		type = t;
		role = r;
		caster = c;
	}
}
