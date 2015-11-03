using UnityEngine;

public class ChantingEventData : SbiEventData {
	public string type;
	public GameObject role;
	public ChantTimer chantTimer;
	public ChantingEventData(string t, GameObject r, ChantTimer c) {
		type = t;
		role = r;
		chantTimer = c;
	}
}
