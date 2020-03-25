using UnityEngine;

namespace BrightLib.Pooling.Samples.BulletsSample
{
	public class PlayerInput : MonoBehaviour
	{
		public Player player;
		public CameraRig _cameraRig;


		public void Update()
		{
			UpdateMovement();
			if (Input.GetButtonDown("Fire1"))
			{
				player.Weapon.PullTrigger();
			}
			else if(Input.GetKeyDown(KeyCode.R))
			{
				player.Weapon.Reload();
			}
		}

		public void LateUpdate()
		{
			UpdateRigMovement();
			UpdateCameraRotation();
		}

		private void UpdateMovement()
		{
			var horizontalAxis = Input.GetAxis("Horizontal");
			var verticalAxis = Input.GetAxis("Vertical");

			var forward = _cameraRig.cameraAttached.transform.forward;
			var right = _cameraRig.cameraAttached.transform.right;

			var desiredMoveDirection = forward.normalized * verticalAxis + right.normalized * horizontalAxis;
			desiredMoveDirection.y = 0;

			player.Move(desiredMoveDirection);
		}

		private void UpdateRigMovement()
		{
			_cameraRig.Follow();
		}

		private void UpdateCameraRotation()
		{
			var vertical = Input.GetAxis("Mouse X");
			var horizontal = Input.GetAxis("Mouse Y");
			_cameraRig.RotateCamera(vertical, horizontal);
		}



	}
}