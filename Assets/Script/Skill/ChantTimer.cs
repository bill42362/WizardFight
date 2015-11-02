using UnityEngine;

public class ChantTimer : MonoBehaviour {
	public double chantTime = 1000;
	public bool isChanting = false;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double timeStartChanting = 0;

	public void StartChanting() {
		timeStartChanting = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		isChanting = true;
	}
	public void StopChanting() { isChanting = false; }
	public bool GetIsChantingFinished() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return (timeStartChanting + chantTime) < timestamp;
	}
	public double GetRemainChantTime() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return 0.001*(timeStartChanting + chantTime - timestamp);
	}
}
