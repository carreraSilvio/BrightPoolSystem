using UnityEngine;

namespace BrightLib.Pooling.Samples.BulletsSample
{
	public class PlayerInput : MonoBehaviour
	{
		public Player player;
		public CameraRig cameraRig;

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

			var forward = cameraRig.cameraAttached.transform.forward;
			var right = cameraRig.cameraAttached.transform.right;

			var desiredMoveDirection = forward.normalized * verticalAxis + right.normalized * horizontalAxis;
			desiredMoveDirection.y = 0;
		
			player.Move(desiredMoveDirection);
		}

		private void UpdateRigMovement()
		{
			cameraRig.Follow();
		}

		private void UpdateCameraRotation()
		{
			var vertical = Input.GetAxis("Mouse X");
			var horizontal = Input.GetAxis("Mouse Y");

			cameraRig.RotateCamera(vertical, horizontal);
		}
	}
}