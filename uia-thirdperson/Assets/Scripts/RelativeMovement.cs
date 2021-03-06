﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class RelativeMovement : MonoBehaviour {

	[SerializeField] private Transform target;

	public float rotSpeed = 15.0f;
	public float moveSpeed = 6.0f;
	public float jumpSpeed = 15.0f;
	public float gravity = -9.8f;
	public float terminalVelocity = -20.0f;
	public float minFall = -1.5f;
	public float pushForce = 3.0f;

	private float _vertSpeed;

	private CharacterController _charController;
	private Animator _animator;

	void Start() {
		_vertSpeed = minFall;
		_charController = GetComponent<CharacterController>();
		_animator = GetComponent<Animator>();
	}

	void Update () {
		Vector3 movement = Vector3.zero;

		float horInput = Input.GetAxis("Horizontal");
		float vertInput = Input.GetAxis("Vertical");
		if (horInput != 0 || vertInput != 0) {
			movement.x = horInput * moveSpeed;
			movement.z = vertInput * moveSpeed;
			movement = Vector3.ClampMagnitude(movement, moveSpeed);

			Quaternion tmp = target.rotation;
			target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
			movement = target.TransformDirection(movement);
			target.rotation = tmp;

			Quaternion direction = Quaternion.LookRotation(movement);
			transform.rotation = Quaternion.Lerp(
				transform.rotation, direction, rotSpeed * Time.deltaTime
			);
		}
		
		_animator.SetFloat("Speed", movement.sqrMagnitude);
		if (_charController.isGrounded) {
			if (Input.GetButtonDown("Jump")) {
				_vertSpeed = jumpSpeed;
			} else {
				_vertSpeed = minFall;
				_animator.SetBool("Jumping", false);
			}
		} else {
			_vertSpeed += gravity * 5 * Time.deltaTime;
			if (_vertSpeed < terminalVelocity) {
				_vertSpeed = terminalVelocity;
			}
			// Ideally, would need some sort of check here 
			// or find a setting to make sure this doesn't 
			// play when the game just starts if (_contact != null)
			_animator.SetBool("Jumping", true);
		}
		movement.y = _vertSpeed;

		movement *= Time.deltaTime;
		_charController.Move(movement);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		// _contact = hit;
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body != null && !body.isKinematic) {
			body.velocity = hit.moveDirection * pushForce;
		}
	}
}