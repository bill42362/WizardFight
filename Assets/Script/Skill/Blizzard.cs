using UnityEngine;
public class Blizzard : MonoBehaviour {
	public GameObject owner;
	public double damage = 10;
	public void OnTriggerStay(Collider other) {
		Role role = other.gameObject.GetComponent<Role>();
		if((null != role) && (owner != role.gameObject)) {
			print(role);
		}
	}
}
