using UnityEngine;

public class SkillButtonEventData : SbiEventData {
	public string type;
	public int index;
	public SkillButtonEventData(string t, int i) {
		type = t;
		index = i;
	}
}
