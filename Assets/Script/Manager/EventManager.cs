using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {
    public string[] castedTypes = new string[0];
    public string[] hearingTypes = new string[0];

    private Hearing[] hearings = new Hearing[0];
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
    private static EventManager _instance = null;

    protected EventManager() { }
    public static EventManager Instance {
        get {
            if(_instance == null) {
                _instance = Object.FindObjectOfType(typeof(EventManager)) as EventManager;
                if(_instance == null) {
                    GameObject gm = new GameObject("EventManager");
                    DontDestroyOnLoad(gm);
                    _instance = gm.AddComponent<EventManager>();
                }
            }
            return _instance;
        }
    }

	public void CastEvent(Object caster, string type, SbiEventData data) {
		PushString(ref castedTypes, type);
		foreach(Hearing h in hearings) {
			if(((caster == h.target) || (this == h.target)) && (type == h.type)) {
				SbiEvent e = new SbiEvent();
				e.target = caster;
				e.listener = h.listener;
				e.type = h.type;
				e.data = data;
				e.time = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
				h.method(e);
			}
		}
	}
	public int RegisterListener(Object target, string type, Object listener, Hearing.EventCallback method) {
		int eventId = -1;
		for(int i = 0; i < hearings.Length; ++i) {
			Hearing h = hearings[i];
			if((target == h.target) && (type == h.type) && (listener == h.listener)) {
				eventId = i;
			}
		}
		if(-1 == eventId) {
			eventId = hearings.Length;
			Hearing hearing = new Hearing();
			hearing.target = target;
			hearing.listener = listener;
			hearing.type = type;
			hearing.method = method;
			Hearing.push(ref hearings, hearing);
			PushString(ref hearingTypes, type);
		}
		return eventId;
	}
	private static string[] PushString(ref string[] array, string item) {
		int index = array.Length;
		System.Array.Resize(ref array, array.Length + 1);
		array[index] = item;
		return array;
	}
}
