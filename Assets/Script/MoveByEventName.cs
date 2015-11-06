using UnityEngine;
public class MoveByEventName : MonoBehaviour {
	public string eventName;
	public Vector3 direction;
	public void Awake () {
		if(null != eventName) {
			EventManager.Instance.RegisterListener(EventManager.Instance, eventName, gameObject, OnEventTriggered);
		}
	}
	public void OnEventTriggered(SbiEvent e) {
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		float speed = 10;
		Role role = GetComponent<Role>();
		if(null != role) { speed = (float)role.speed; }
		Vector3 velocity = transform.localToWorldMatrix.MultiplyVector(direction*speed);
		if(null != rigidbody) {
			rigidbody.velocity = velocity;
		}
	}
}
