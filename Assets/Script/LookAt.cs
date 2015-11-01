using UnityEngine;
public class LookAt : MonoBehaviour {
	public GameObject target;
	private GameObject me;
	private Vector3 up = new Vector3(0, 1, 0);
	public void Awake() { 
		target = GameObject.FindWithTag("Enemy");
	}
	public void Update () {
		if(null != target) { transform.LookAt(target.transform, up); }
	}
}
