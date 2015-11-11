using UnityEngine;
public class ThunderNova : MonoBehaviour {
	public GameObject owner;
	public double damage = 10;
	public double lifeTime = 500;
	private PhotonView photonView;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double startTime;
	private bool doneDamage = false;
	private Faction faction;

	public void Start () {
		faction = GetComponent<Faction>();
		photonView = GetComponent<PhotonView>();
		startTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}
	public void Update() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		if((startTime + lifeTime) < timestamp) { Destroy(gameObject); }
	}
	public void OnTriggerStay(Collider other) {
		Role role = other.gameObject.GetComponent<Role>();
		Faction otherFaction = other.gameObject.GetComponent<Faction>();
		if(
			(null != role)
			&& (true == otherFaction.IsRival(faction))
			&& (false == doneDamage)
		) {
			role.TakeDamage(damage);
			doneDamage = true;
		}
	}
}
