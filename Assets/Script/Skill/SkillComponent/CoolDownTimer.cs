using UnityEngine;
using System.Collections;

public class CoolDownTimer : MonoBehaviour {
	public double coolDownTime = 8000;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double timeStartCooling = 0;

	public void StartCoolDown() {
		timeStartCooling = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}
	public bool GetIsCoolDownFinished() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return (timeStartCooling + coolDownTime) < timestamp;
	}
	public double GetRemainCoolDownTime() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return 0.001*(timeStartCooling + coolDownTime - timestamp);
	}
}
