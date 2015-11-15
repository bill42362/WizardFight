using UnityEngine;

public class GuidingEventData : SbiEventData {
	public string type;
	public GameObject role;
	public int skillId;
	public GuideTimer guideTimer;
	public GuidingEventData(string t, GameObject r, GuideTimer c) {
		type = t;
		role = r;
		guideTimer = c;
	}
	public GuidingEventData(string t, GameObject r, int id) {
		type = t;
		role = r;
		skillId = id;
	}
}
