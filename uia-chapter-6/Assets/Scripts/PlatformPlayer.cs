using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayer : MonoBehaviour {

	public float speed = 250.0f;

	private Rigidbody2D _body;
	private Animator _anim;

	void Start () {
		_body = GetComponent<Rigidbody2D>();
		_anim = GetComponent<Animator>();
	}
	
	void Update () {
		float deltaX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
		Vector2 movement = new Vector2(deltaX, _body.velocity.y);
		_body.velocity = movement;
		_anim.SetFloat("speed", Mathf.Abs(deltaX));
		if (!Mathf.Approximately(deltaX, 0)) {
			transform.localScale = new Vector3(Mathf.Sign(deltaX), 1, 1);
		}
	}
}
