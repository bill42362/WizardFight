using UnityEngine;

public class Timer : MonoBehaviour {
	public bool isTiming = false;
    private bool isEventShot = true;
    private double buffer = 0;
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
        if ( finishEventName != null && canShootFinishEvent)
        {
            EventManager.Instance.CastEvent(this, finishEventName, null);
            isEventShot = true;
        }
        if ( isTiming && !shouldTiming ) {
            FinishTiming();
        }
    }
	public double GetStartTime() { return startTime; }
	public double GetFinishTime() { return finishTime; }
    public void InitTiming(double startTime, double finishTime ,double bufferTime = 0) {
        this.startTime = startTime;
        this.finishTime = finishTime;
        this.buffer = bufferTime;
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
        StopTiming();
    }
	private void StartTiming() {
        isTiming = true;
        isEventShot = false;
        TimerEventData startData = new TimerEventData("start", owner, this);
        if ( startEventName != null )
		    EventManager.Instance.CastEvent(EventManager.Instance, startEventName, startData);
	}
	private void StopTiming() {
		if(isTiming) {
			isTiming = false;
            isEventShot = true;
			TimerEventData stopData = new TimerEventData("stop", owner, this);
            if (stopEventName != null)
                EventManager.Instance.CastEvent(EventManager.Instance, stopEventName, stopData);
		}
	}
    private bool canShootFinishEvent
    {
        get
        {
            if (isEventShot)
                return false;
            return  PhotonNetwork.time > finishTime - buffer ;
        }
    }
}
