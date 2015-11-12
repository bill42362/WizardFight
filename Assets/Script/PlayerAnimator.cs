﻿using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour {
    public Animator animator;
	public float velocityX;
	private Rigidbody rigidbody;
	
	void Start () {
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody>();
		EventManager.Instance.RegisterListener(EventManager.Instance, "startChanting", gameObject, OnStartChanting);
		EventManager.Instance.RegisterListener(EventManager.Instance, "stopChanting", gameObject, OnStopChanting);
		EventManager.Instance.RegisterListener(EventManager.Instance, "startGuiding", gameObject, OnStartGuiding);
		EventManager.Instance.RegisterListener(EventManager.Instance, "stopGuiding", gameObject, OnStopGuiding);
	}
	void Update () {
        animator.SetFloat("velocityX", rigidbody.velocity.x);
	}
	public void OnStartChanting(SbiEvent e) {
		ChantingEventData data = e.data as ChantingEventData;
		if(gameObject == data.role) {
			animator.SetBool("isChanting", true);
		}
	}
	public void OnStopChanting(SbiEvent e) {
		ChantingEventData data = e.data as ChantingEventData;
		if(gameObject == data.role) {
			animator.SetBool("isChanting", false);
		}
	}
	public void OnStartGuiding(SbiEvent e) {
		GuidingEventData data = e.data as GuidingEventData;
		if(gameObject == data.role) {
			animator.SetBool("isGuiding", true);
		}
	}
	public void OnStopGuiding(SbiEvent e) {
		GuidingEventData data = e.data as GuidingEventData;
		if(gameObject == data.role) {
			animator.SetBool("isGuiding", false);
		}
	}
}
