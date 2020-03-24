using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour 
{
	[SerializeField]private Camera _camera;
	[SerializeField]private Transform _character;

	public float moveSpeed = 3f;
	public float rotateSpeed = 20f;

	public bool clampVerticalRotation;
	public float minX = -0.5f;
	public float maxX = 0.35f;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void RotateCamera(float vertical, float horizontal)
	{
		vertical  = rotateSpeed * vertical;
		horizontal  = rotateSpeed * horizontal;

		var characterRotation = _character.localRotation;
		var cameraRotation = _camera.transform.localRotation;
		characterRotation *= Quaternion.Euler (0f,  vertical, 0f);
		cameraRotation *= Quaternion.Euler (-horizontal, 0f, 0f);

		cameraRotation.x = clampVerticalRotation ?  Mathf.Clamp(cameraRotation.x, minX, maxX) : cameraRotation.x;

		_character.localRotation = Quaternion.Slerp (_character.localRotation, characterRotation,
			rotateSpeed * Time.deltaTime);
		_camera.transform.localRotation = Quaternion.Slerp (_camera.transform.localRotation, cameraRotation,
			rotateSpeed * Time.deltaTime);
	}

	private Vector3 _desiredPosition;
	private Vector3 _smoothedPosition;
	public void Follow()
	{
		_desiredPosition = _character.position;
		_smoothedPosition = Vector3.Lerp(transform.position, _desiredPosition, moveSpeed);
		transform.position = _smoothedPosition;
		transform.localRotation = _character.localRotation;
	}

	public Camera cameraAttached
	{
		get{return _camera;}
	}
}