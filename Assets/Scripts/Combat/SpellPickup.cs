using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Audio;

namespace RPG.Combat
{

    public class SpellPickup : MonoBehaviour
    {
        [SerializeField] Weapon weaponToEquip = null;
        [SerializeField] float respawnTime = 5;

        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player")
            {
                other.GetComponent<Fighter>().EquipWeapon(weaponToEquip);
                GetComponent<SpellPickupAudio>().SpellPickupSound();
                StartCoroutine(HideForSeconds(respawnTime));
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

