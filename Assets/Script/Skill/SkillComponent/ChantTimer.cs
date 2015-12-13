using UnityEngine;

public class ChantTimer : MonoBehaviour {
	public bool isChanting = false;
	public GameObject owner;

	private double epochStart = 0.0;
	private double timeStartChanting = 0;
    private double timeFinishChanting = 0;
    public bool shouldChanting
    {
        get
        {
            return (PhotonNetwork.time > timeStartChanting && PhotonNetwork.time < timeFinishChanting);
        }
    }
    public void Update()
    {
        if ( !isChanting && shouldChanting )
        {
            StartChanting();
            return;
        }
        if ( isChanting && !shouldChanting )
        {
            FinishChanting();
        }
    }
    public void InitChanting(double startTime, double endTime)
    {
        timeStartChanting = startTime;
        timeFinishChanting = endTime;
    }
	private void StartChanting() {

        isChanting = true;
		ChantingEventData startData = new ChantingEventData("start", owner, this);
		EventManager.Instance.CastEvent(EventManager.Instance, "startChanting", startData);
	}
	private void StopChanting() {
		isChanting = false;
		ChantingEventData stopData = new ChantingEventData("stop", owner, this);
		EventManager.Instance.CastEvent(EventManager.Instance, "stopChanting", stopData);
	}
    public void FinishChanting()
    {
        EventManager.Instance.CastEvent(this, "finishChanting", null);
        StopChanting();
    }
    public void CancelChanting()
    {
        timeFinishChanting = timeStartChanting;
        StopChanting();
    }
	public double GetRemainChantTime() {
        if (!isChanting)
            return 0;
        double timestamp = PhotonNetwork.time;
		return (timeFinishChanting - timestamp);
	}
	public double GetChantingProgress() {
        if (!isChanting)
            return 0;
		double timestamp = PhotonNetwork.time;
		return (timestamp - timeStartChanting)/(timeFinishChanting - timeStartChanting);
	}
}
