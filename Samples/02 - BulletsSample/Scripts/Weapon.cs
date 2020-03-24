using BrightLib.Pooling.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Samples.BulletsSample
{
    public class Weapon : MonoBehaviour
    {
        public float fireRate = 0.1f;
        public int clipSize = 30;

        private float lastTimeShot;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public void Shoot()
        {
            if(PoolSystem.TryFetchAvailable("Bullet", out GameObject bullet))
            {
                bullet.transform.position = transform.position;
                lastTimeShot = Time.time;
            }
            else
            {
                Debug.Log("no more bullets available in pool");
            }
            
        }

        public bool CanShoot
        {
            get
            {
                return Time.time - lastTimeShot >= fireRate;
            }
        }
    }
}