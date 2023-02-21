using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Audio;

namespace RPG.Combat
{
    
    public class OrbHealing : MonoBehaviour {
        [SerializeField] float healthToRestore = 200;
        //[SerializeField] float respawnTime = 10;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player")
            {
                other.GetComponent<Health>().Heal(healthToRestore);
                //GetComponent<BulbAudio>().AddHealthSound();
                //StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool shouldShow)
        {
            GetComponent<Collider>().enabled = shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }
        
    }
}