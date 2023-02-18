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
        // should make this more robust by having an array of params that all change with this flipped bool, rather than an if check in each method call
        [SerializeField] bool isPoison = false; 

        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            thisProjectile = GetComponent<Projectile>();
            spawnPosition = transform.position;

            if (isPoison)
            {
                helper.SetParamValue(0, 0); // turn off waveshape two metro for poison bombs
                helper.SetParamValue(2, startingFreq); // set washape two start freq
                helper.SetParamValue(5, 0.6f); // waveshape two gain    

            }
            else
            {
                helper.SetParamValue(9, startingFreq); // set waveshape start freq
                helper.SetParamValue(10, 0.6f); // waveshape gain     
            }
        }

        // Update is called once per frame
        void Update()
        {
            FireProjectile(thisProjectile.GetTarget().transform.position);
        }

        private void FireProjectile(Vector3 targetLocation)
        {
            // what i really need to do here is create multiple prefab variants so that the correct weapon instantiates the correct projectile and scale the distance based on range
            
            float distance = Vector3.Distance(targetLocation, spawnPosition);

            if (isPoison)
            {
                helper.SetParamValue(1, distance * 4.2f); // waveshape metro rate (really the Hz)
            }
            else
            {
                helper.SetParamValue(7, distance * 6.3f); // waveshape metro rate
            }
        }

        private void OnDestroy() {
            if (isPoison)
            {
                helper.SetParamValue(5, 0f); // lower waveshape two gain just in case 
            }
            else
            {
                helper.SetParamValue(10, 0f); // lower waveshape gain just in case 
            }
            
        }

        private void OnTriggerEnter(Collider other) 
            {
                if (other.GetComponent<Health>() != thisProjectile.GetTarget()) return;
                if (thisProjectile.GetTarget().IsDead()) return;

                if (isPoison)
                {
                    helper.SetParamValue(5, 0f); // lower waveshape gain 
                }
                else
                {
                    helper.SetParamValue(10, 0f); // lower waveshape gain
                }
                
            }

    }
}


