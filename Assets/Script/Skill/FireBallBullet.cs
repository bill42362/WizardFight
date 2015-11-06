using UnityEngine;
public class FireBallBullet : MonoBehaviour {
	public GameObject owner;
	public double damage = 10;
	public double flyingSpeed = 10;
	public double lifeTime = 3000;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double startTime;
	private bool doneDamage = false;
	private PhotonView photonView;

	public void Start () {
		photonView = GetComponent<PhotonView>();
		GetComponent<Rigidbody>().velocity = transform.forward*(float)flyingSpeed;
		startTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}
	public void Update() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		if((startTime + lifeTime) < timestamp) { Destroy(gameObject); }
	}
	public void OnTriggerStay(Collider other) {
		Role role = other.gameObject.GetComponent<Role>();
		if(null != role) {
			Debug.Log("FireBallBullet Hit!!!!");
		}
		if((null != role) && (owner != role.gameObject)
		) {
			Debug.Log("FireBallBullet Enemy!!!!");
		}
		if(
			(null != role)
			&& (owner != role.gameObject)
			&& (false == photonView.isMine)
			&& (false == doneDamage)
		) {
			role.TakeDamage(damage);
			doneDamage = true;
		}
	}
}
