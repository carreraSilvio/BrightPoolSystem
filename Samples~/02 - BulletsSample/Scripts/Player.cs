using UnityEngine;

namespace BrightLib.Pooling.Samples.BulletsSample
{
	public class Player : MonoBehaviour
	{
		public float speed = 5f;
		private CharacterController _characterController;
		private Weapon _weapon;

		public Weapon Weapon { get => _weapon; }

		public void Awake()
		{
			_characterController = GetComponentInParent<CharacterController>();
			_weapon = GetComponentInChildren<Weapon>();
		}

		public void Move(Vector3 desiredMoveDirection)
		{
			desiredMoveDirection *= speed;

			_characterController.Move(desiredMoveDirection * Time.deltaTime);
		}

		public bool IsMoving()
		{
			return _characterController.velocity.magnitude > 0.1f ||
				_characterController.velocity.magnitude > 0.1f;
		}
	}
}