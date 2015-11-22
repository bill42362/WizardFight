using UnityEngine;

public class CastingEventData : SbiEventData {
	public string type;
	public GameObject role;
	public int skillId;
    public Vector3 pos;
    public Vector3 forward;
    public double time;
	public CastingEventData(string t, GameObject r, int id, Vector3 createPosition, Vector3 pForward, double createTime) {
		type = t;
		role = r;
		skillId = id;
        pos = createPosition;
        time = createTime;
        this.forward = pForward;

	}
}
