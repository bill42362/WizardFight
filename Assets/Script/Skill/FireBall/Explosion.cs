using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {
	public GameObject owner;
	public double lifeTime = 5000;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double startTime;
	private Faction faction;

	// Use this for initialization
	void Start () {
		faction = GetComponent<Faction>();
		startTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}
	
	// Update is called once per frame
	void Update () {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		if(timestamp > (startTime + lifeTime)) { Destroy(gameObject, 5); }
	}
}
