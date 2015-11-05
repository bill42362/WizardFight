using UnityEngine;
public class Blizzard : MonoBehaviour {
	public GameObject owner;
	public double damage = 10;
	public double damageCycle = 1;
	private PhotonView photonView;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double lastDamageTime;

	public void Awake() {
		photonView = GetComponent<PhotonView>();
		lastDamageTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}

	public void OnTriggerStay(Collider other) {
		Role role = other.gameObject.GetComponent<Role>();
		if(
			(null != role)
			&& (owner != role.gameObject)
			&& (false == photonView.isMine)
		) {
			double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
			if(damageCycle <= (timestamp - lastDamageTime)) {
				role.TakeDamage(damage);
				lastDamageTime = timestamp;
			}
		}
	}
}
