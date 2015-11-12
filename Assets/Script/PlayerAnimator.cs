using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {
    public Animator animator;
	public float velocityX;
	private Rigidbody rigidbody;
	
	void Start () {
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody>();
	}
	void Update () {
        animator.SetFloat("velocityX", rigidbody.velocity.x);
	}
}
