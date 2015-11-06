using UnityEngine;

public class ChantTimer : MonoBehaviour {
	public double chantTime = 1000;
	public bool isChanting = false;
	public GameObject owner;

	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double timeStartChanting = 0;

	void Awake () {
	}

	public void StartChanting() {
		timeStartChanting = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		isChanting = true;
		ChantingEventData startData = new ChantingEventData("start", owner, this);
		EventManager.Instance.CastEvent(EventManager.Instance, "startChanting", startData);
	}
	public void StopChanting() {
		isChanting = false;
		ChantingEventData stopData = new ChantingEventData("stop", owner, this);
		EventManager.Instance.CastEvent(EventManager.Instance, "stopChanting", stopData);
	}
	public bool GetIsChantingFinished() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return (timeStartChanting + chantTime) < timestamp;
	}
	public double GetRemainChantTime() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return 0.001*(timeStartChanting + chantTime - timestamp);
	}
	public double GetChantingProgress() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		return (timestamp - timeStartChanting)/chantTime;
	}
}
