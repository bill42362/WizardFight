using UnityEngine;

public class CastingEventData : SbiEventData {
	public string type;
	public GameObject role;
	public int skillId;
	public CastingEventData(string t, GameObject r, int id) {
		type = t;
		role = r;
		skillId = id;
	}
}
