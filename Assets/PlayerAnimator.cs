using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {

    public Animator animator;
	// Use this for initialization
	void Start () {
		animator = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		// 把角色的速度值傳給 animator 中的 velocity
        animator.SetFloat ("velocity", this.GetComponent<Rigidbody>().velocity.magnitude);
	}
}
