using UnityEngine;
using System.Collections;
public class FireBallBullet : MonoBehaviour {
    public static GameObject CreateInstance( CastingEventData data, Faction faction)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(
    Resources.Load("Prefab/Skill/FireBallBullet"), data.pos, Quaternion.LookRotation(data.forward));
        obj.GetComponent<FireBallBullet>().SetCreationParameters(data.pos, data.forward, data.time);
        obj.GetComponent<Faction>().SetFaction(faction);
        return obj;
    }
	public double damage = 10;
	public double flyingSpeed = 10;
	public double lifeTime = 3000;
    public Vector3 createPosition;
    public Vector3 createForward;
    public double createTime;
	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private double startTime;
	private bool doneDamage = false;
	private Faction faction;

    public void SetCreationParameters(Vector3 pCreatePosition,Vector3 pForward, double pTime)
    {
        this.createPosition = pCreatePosition;
        this.createTime = pTime;
        this.createForward = pForward;
    }
	public void Start () {
		faction = GetComponent<Faction>();
		//GetComponent<Rigidbody>().velocity = transform.forward*(float)flyingSpeed;
		startTime = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
	}
	public void Update() {
		double timestamp = (System.DateTime.UtcNow - epochStart).TotalMilliseconds;
		if((startTime + lifeTime) < timestamp) { Destroy(gameObject); }
        float diffTime = (float)(PhotonNetwork.time - createTime);
        transform.position = createPosition + createForward * (float)flyingSpeed * diffTime;


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
			GameObject explodeGameObject = (GameObject)GameObject.Instantiate(
				Resources.Load("Prefab/Skill/Explosion"), gameObject.transform.position, transform.rotation
			);
		}
	}
}
