using BrightLib.Pooling.Runtime;
using UnityEngine;

namespace BrightLib.Pooling.Samples.BulletsSample
{
    public class Weapon : MonoBehaviour
    {
        public GameObject visuals;
        public Transform emissionSpot;
        public float fireRate = 0.1f;
        public int clipSize = 12;
        public float reloadSpeed = 0.1f;
        public int range = 30;
        public float shootForce = 500f;

        private float _lastTimeShot;
        private int _bulletsInClip;

        private float _reloadStartTime;

        public enum WeaponState { Idle, Shooting, Reloading};
        private WeaponState _state;

       
        void Awake()
        {
            _bulletsInClip = clipSize;
        }

        private void Update()
        {
            if(_state == WeaponState.Reloading)
            {
                if(IsReloadComplete)
                {
                    _bulletsInClip = clipSize;
                    _state = WeaponState.Idle;
                }
            }
            else if (_state == WeaponState.Shooting)
            {
                if (IsShootingComplete)
                {
                    _state = WeaponState.Idle;
                }
            }
        }

        public void Reload()
        {
            if (IsClipFull || _state == WeaponState.Reloading) return;
            
            _reloadStartTime = Time.time;
            _state = WeaponState.Reloading;
        }

        public void PullTrigger()
        {
            if(_bulletsInClip == 0)
            {
                Debug.Log("Bullet clip empty. Reload!");
            }
            else
            {
                if(IsShootingComplete) Shoot();
            }
        }

        private void Shoot()
        {
            if (PoolSystem.FetchAvailable("Bullet", out Bullet bullet))
            {
                bullet.transform.position = emissionSpot.position;
                bullet.Fire(emissionSpot.forward * shootForce, range);

                _lastTimeShot = Time.time;
                _bulletsInClip--;
                _state = WeaponState.Shooting; 
            }
            else
            {
                Debug.Log("no more bullets available in pool");
            }
        }

        public bool IsShootingComplete
        {
            get
            {
                return Time.time - _lastTimeShot >= fireRate;
            }
        }

        public bool IsClipEmpty
        {
            get
            {
                return _bulletsInClip == 0;
            }
        }

        public bool IsClipFull
        {
            get
            {
                return _bulletsInClip == clipSize;
            }
        }

        public bool IsReloadComplete
        {
            get
            {
                return Time.time - _reloadStartTime > reloadSpeed;
            }
        }

        public int BulletsInClip { get => _bulletsInClip; }
        public string State { get => _state.ToString(); }
    }
}