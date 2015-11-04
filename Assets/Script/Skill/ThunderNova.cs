using UnityEngine;
public class ThunderNova : MonoBehaviour {
	public GameObject owner;
	public double damage = 10;
	public double lifeTime = 500;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double startTime;
	public void Start () {
		startTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}
	public void Update() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		if((startTime + lifeTime) < timestamp) { Destroy(gameObject); }
	}
	public void OnTriggerStay(Collider other) {
		Role role = other.gameObject.GetComponent<Role>();
		if((null != role) && (owner != role.gameObject)) {
			print(role);
		}
	}
}
