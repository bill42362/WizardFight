using UnityEngine;
public class FireBallBullet : MonoBehaviour {
	public GameObject owner;
	public double damage = 10;
	public double flyingSpeed = 10;
	public double lifeTime = 1000;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double startTime;

	public void Start () {
		GetComponent<Rigidbody>().velocity = transform.forward*(float)flyingSpeed;
		startTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}
	public void Update() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		if((startTime + lifeTime) < timestamp) { Destroy(gameObject); }
	}
}
