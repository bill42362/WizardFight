using UnityEngine;
using System.Collections;
public class FireBallBullet : MonoBehaviour {
    public static GameObject CreateInstance( double time , Vector3 pos, Vector3 dir , Faction faction, FireBallCaster caster)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(
			Resources.Load("Prefab/Skill/FireBallBullet"), pos, Quaternion.LookRotation(dir)
		);
        obj.GetComponent<FireBallBullet>().SetCreationParameters(pos, dir, time, caster);
        obj.GetComponent<Faction>().SetFaction(faction);
        return obj;
    }
	public double damage = 10;
	public double flyingSpeed = 10;
	public double lifeTime = 4;
    public Vector3 createPosition;
    public Vector3 createForward;
    public double createTime;

	private bool doneDamage = false;
	private Faction faction;
    private FireBallCaster caster;

    public void SetCreationParameters(Vector3 pCreatePosition,Vector3 pForward, double pTime, FireBallCaster pCaster) {
        this.createPosition = pCreatePosition;
        this.createTime = pTime;
        this.createForward = pForward;
        this.caster = pCaster;
    }
	public void Start () {
		faction = GetComponent<Faction>();
        //GetComponent<Rigidbody>().velocity = transform.forward*(float)flyingSpeed;
	}
	public void Update() {
		double timestamp = PhotonNetwork.time;
		if((createTime + lifeTime) < timestamp) { Destroy(gameObject); }
        float diffTime = (float)(PhotonNetwork.time - createTime);
        transform.position = createPosition + createForward * (float)(flyingSpeed * diffTime);
	}
	public void OnTriggerStay(Collider other) {
		Role role = other.gameObject.GetComponent<Role>();
		Faction otherFaction = other.gameObject.GetComponent<Faction>();
		if(
			(null != role)
			&& (true == otherFaction.IsRival(faction))
			&& (false == doneDamage)
            && ( role.playerId == GameManager.Instance.PlayerId )
		) {
            caster.OnBulletHit();
            role.TakeDamageRPC(damage);
            doneDamage = true;
        }
	}
}
