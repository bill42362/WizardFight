using UnityEngine;

public class GuidingEventData : SbiEventData {
	public string type;
	public GameObject role;
	public GuideTimer guideTimer;
	public GuidingEventData(string t, GameObject r, GuideTimer c) {
		type = t;
		role = r;
		guideTimer = c;
	}
}
