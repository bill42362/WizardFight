using UnityEngine;
public class Blizzard : MonoBehaviour {
	public GameObject owner;
	public double damage = 1;
	public double damageCycle = 100;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double lastDamageTime = 0;
	private Faction faction;

	public void Awake() {
		faction = GetComponent<Faction>();
		lastDamageTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}

	public void OnTriggerStay(Collider other) {
		Role role = other.gameObject.GetComponent<Role>();
		Faction otherFaction = other.gameObject.GetComponent<Faction>();
		if((null != role) && (true == otherFaction.IsRival(faction))) {
			double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
			if(damageCycle <= (timestamp - lastDamageTime)) {
				role.TakeDamageRPC(damage);
				lastDamageTime = timestamp;
			}
		}
	}
}
