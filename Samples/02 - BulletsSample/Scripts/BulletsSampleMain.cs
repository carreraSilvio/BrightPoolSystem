using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrightLib.Pooling.Samples.BulletsSample
{
    public class BulletsSampleMain : MonoBehaviour
    {
        public Weapon weapon;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if(weapon.CanShoot && Input.GetKeyDown(KeyCode.Space))
            {
                weapon.Shoot();
            }
        }
    }
}