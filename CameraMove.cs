using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public Transform PlayerTransform;
    private Vector3 _cameraOffset;

    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;
    public float RotationsSpeed = 5.0f;

	void Start () {
		_cameraOffset = transform.position - PlayerTransform.position;	
	}
		
	void LateUpdate () {

		if(IsMouseButtonPressed())
        {	
            Quaternion camTurnAngle =
                Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed * 0.90f, Vector3.up);

			_cameraOffset = camTurnAngle * _cameraOffset;
        }

        Vector3 newPos = PlayerTransform.position + _cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

		if (IsMouseButtonPressed())
            transform.LookAt(PlayerTransform);
	}

	bool IsMouseButtonPressed()
	{
		return Input.GetMouseButton(1);
	}
}
