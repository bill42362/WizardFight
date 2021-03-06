using UnityEngine;

public class Timer : MonoBehaviour {
	public bool isTiming = false;
    public string type = "";
	public string startEventName;
	public string finishEventName;
	public string stopEventName;
    public double duration { get { return finishTime - startTime; } }
    public GameObject owner { get {
		if ( _owner == null) { _owner = this.gameObject.transform.parent.parent.gameObject; }
		return _owner;
	}}

    private GameObject _owner = null;
	private double startTime = 0;
    private double finishTime = 0;
    private bool shouldTiming {
        get { return (PhotonNetwork.time > startTime && PhotonNetwork.time < finishTime); }
    }

    public void Update() {
        if ( !isTiming && shouldTiming ) {
            StartTiming();
            return;
        }
        if ( isTiming && !shouldTiming ) {
            FinishTiming();
        }
    }
	public double GetStartTime() { return startTime; }
	public double GetFinishTime() { return finishTime; }
    public void InitTiming(double startTime, double finishTime) {
        this.startTime = startTime;
        this.finishTime = finishTime;
    }
    public void CancelTiming() {
        finishTime = startTime;
        StopTiming();
    }
	public double GetRemainTime() {
		if (!isTiming) return 0;
        double timestamp = PhotonNetwork.time;
		return (finishTime - timestamp);
	}
	public double GetProgress() {
        if (!isTiming) return 0;
		double timestamp = PhotonNetwork.time;
		return (timestamp - startTime)/(finishTime - startTime);
	}

    private void FinishTiming() {
        if (finishEventName != null)
            EventManager.Instance.CastEvent(this, finishEventName, null);
        StopTiming();
    }
	private void StartTiming() {
        isTiming = true;
		TimerEventData startData = new TimerEventData("start", owner, this);
        if ( startEventName != null )
		    EventManager.Instance.CastEvent(EventManager.Instance, startEventName, startData);
	}
	private void StopTiming() {
		if(isTiming) {
			isTiming = false;
			TimerEventData stopData = new TimerEventData("stop", owner, this);
            if (stopEventName != null)
                EventManager.Instance.CastEvent(EventManager.Instance, stopEventName, stopData);
		}
	}
}
