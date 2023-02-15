using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;

namespace RPG.Audio
{
    public class ProjectileAudio : MonoBehaviour
    {
        const int pluginKey = 1; //the value we set the "Instance Index" param to in the mixer
        AlkarraSoundHelper helper;
        Projectile thisProjectile;
        Vector3 spawnPosition;
        [SerializeField] float startingFreq = 1000;

        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            thisProjectile = GetComponent<Projectile>();
            spawnPosition = transform.position;
            helper.SetParamValue(6, 0.6f); // waveshape gain      
            helper.SetParamValue(5, startingFreq); // set waveshape start freq  
        }

        // Update is called once per frame
        void Update()
        {
            FireProjectile(thisProjectile.GetTarget().transform.position);
        }

        private void FireProjectile(Vector3 targetLocation)
        {
            float distance = Vector3.Distance(targetLocation, spawnPosition);
            helper.SetParamValue(3, distance * 8f); // waveshape metro rate
        }

        private void OnDestroy() {
            helper.SetParamValue(6, 0f); // lower waveshape gain just in case
        }

        private void OnTriggerEnter(Collider other) 
            {
                if (other.GetComponent<Health>() != thisProjectile.GetTarget()) return;
                if (thisProjectile.GetTarget().IsDead()) return;
                Debug.Log("projaudio hit");
                helper.SetParamValue(6, 0f); // lower waveshape gain
            }

    }
}


