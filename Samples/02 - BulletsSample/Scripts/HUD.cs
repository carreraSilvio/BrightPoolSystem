using UnityEngine;
using UnityEngine.UI;

namespace BrightLib.Pooling.Samples.BulletsSample
{
	public class HUD : MonoBehaviour
	{
		public Weapon weapon;

		[SerializeField]
		private Text _bulletsField = default;

		[SerializeField]
		private Text _stateField = default;


		public void LateUpdate()
		{
			if (weapon == null) return;

			_bulletsField.color = weapon.IsClipEmpty ? Color.yellow : Color.white;
			_bulletsField.text = weapon.BulletsInClip + "/" + weapon.clipSize;

			_stateField.text = weapon.State;
		}

	}
}