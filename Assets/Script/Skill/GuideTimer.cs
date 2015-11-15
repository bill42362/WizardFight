using UnityEngine;

public class GuideTimer : MonoBehaviour {
	public double guideTime = 10000;
	public bool isGuiding = false;
	public GameObject owner;

	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double timeStartGuiding = 0;

	public void StartGuiding() {
		timeStartGuiding = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		isGuiding = true;
	}
	public void StopGuiding() {
		isGuiding = false;
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
