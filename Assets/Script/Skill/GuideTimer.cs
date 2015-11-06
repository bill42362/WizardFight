using UnityEngine;

public class GuideTimer : MonoBehaviour {
	public double guideTime = 10000;
	public bool isGuiding = false;
	public GameObject owner;

	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double timeStartGuiding = 0;

	void Awake () {
	}

	public void StartGuiding() {
		timeStartGuiding = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		isGuiding = true;
		GuidingEventData startData = new GuidingEventData("start", owner, this);
		EventManager.Instance.CastEvent(EventManager.Instance, "startGuiding", startData);
	}
	public void StopGuiding() {
		isGuiding = false;
		GuidingEventData stopData = new GuidingEventData("stop", owner, this);
		EventManager.Instance.CastEvent(EventManager.Instance, "stopGuiding", stopData);
	}
	public bool GetIsGuidingFinished() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return (timeStartGuiding + guideTime) < timestamp;
	}
	public double GetRemainGuidTime() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return 0.001*(timeStartGuiding + guideTime - timestamp);
	}
	public double GetGuidingProgress() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return (timestamp - timeStartGuiding)/guideTime;
	}
}
