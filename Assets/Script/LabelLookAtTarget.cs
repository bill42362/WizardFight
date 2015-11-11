using UnityEngine;
using System.Collections;

public class LabelLookAtTarget : MonoBehaviour {
	public GameObject label;
	public LookAt lookAt;

	void Start () {
		label = Instantiate(
			Resources.Load("Prefab/RoleLabel"), Vector3.zero, Quaternion.identity
		) as GameObject;
		label.name = "RoleLabel";
		lookAt = GetComponent<LookAt>();
	}
	
	void Update () {
		if((null != lookAt.target) && (
			(null == label.transform.parent)
			|| (lookAt.target != label.transform.parent.gameObject)
		)) {
			label.transform.parent = lookAt.target.transform;
			label.transform.localPosition = Vector3.zero;
		}
	}
}
