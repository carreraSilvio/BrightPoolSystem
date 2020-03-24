using UnityEngine;

namespace BrightLib.Pooling.Samples.BulletsSample
{
	public class Player : MonoBehaviour
	{
		public float speed = 5f;
		private CharacterController _characterController;

		public void Awake()
		{
			_characterController = GetComponentInParent<CharacterController>();
		}

		public void Move(Vector3 desiredMoveDirection)
		{
			desiredMoveDirection = desiredMoveDirection * speed;

			_characterController.Move(desiredMoveDirection * Time.deltaTime);
		}

		public bool IsMoving()
		{
			return _characterController.velocity.magnitude > 0.1f ||
				_characterController.velocity.magnitude > 0.1f;
		}
	}
}