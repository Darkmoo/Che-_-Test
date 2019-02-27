using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class SpotPulse : MonoBehaviour {

	public Light lt;
	public float duration = 20;
	public float min_angle = 25f;
	public float max_angle = 35f;

	void Start () {
		lt = GetComponent<Light>();
	}

	void Update () {
		float min = Time.time * duration; 
		float cur_angle = Mathf.PingPong(min, max_angle);
		cur_angle = cur_angle <= min_angle ? min_angle : cur_angle;
		lt.spotAngle = cur_angle;
	}
}
