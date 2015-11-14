using UnityEngine;
using System.Collections;
public class FireBallBullet : MonoBehaviour {
	public GameObject owner;
	public double damage = 10;
	public double flyingSpeed = 10;
	public double lifeTime = 3000;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double startTime;
	private bool doneDamage = false;
	private Faction faction;

	public void Start () {
		faction = GetComponent<Faction>();
		GetComponent<Rigidbody>().velocity = transform.forward*(float)flyingSpeed;
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
			role.TakeDamageRPC(damage);
			doneDamage = true;
			Destroy(gameObject);
			GameObject explodeGameObject = Instantiate(
				Resources.Load("Prefab/Skill/Explosion"),
				gameObject.transform.position,
				transform.rotation) as GameObject;
		}
	}
}
