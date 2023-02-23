using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Audio;
using RPG.Core; // only need this for UItext... probably needs refactor
using RPG.Story; // only need this for UItext... probably needs refactor

namespace RPG.Combat
{

    public class SpellPickup : MonoBehaviour
    {
        [SerializeField] Weapon weaponToEquip = null;
        [SerializeField] float respawnTime = 5;
        [SerializeField] GameObject spellHelpPrefab;
        [SerializeField] GameObject spellEffect = null;
        [NonSerialized] public bool isHidden = false;
        bool alreadyTriggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !alreadyTriggered)
            {
                other.GetComponent<Fighter>().EquipWeapon(weaponToEquip);
                GetComponent<SpellPickupAudio>().SpellPickupSound();
                Instantiate(spellEffect, other.transform.position, other.transform.rotation);
                StartCoroutine(DisplayHelpText(0, spellHelpPrefab));
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
            isHidden = !shouldShow;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(shouldShow);
            }
        }

        IEnumerator DisplayHelpText(float waitTime, GameObject uiTextPrefab)
        {
            yield return new WaitForSeconds(waitTime);
            GameObject textPromptInstance = Instantiate(uiTextPrefab);
            //StartCoroutine(StoryDialogueFade(textPromptInstance));
            yield return StoryDialogueFade(textPromptInstance, 7);
            alreadyTriggered = true;
        }

        private IEnumerator StoryDialogueFade(GameObject textPrompt, float storyDisplayTime)
        {
            StoryFader fader = textPrompt.GetComponent<StoryFader>();

            // this is a little backwards bc alpha = 1 is considered "out"
            yield return fader.FadeOut(1f);
            yield return new WaitForSeconds(storyDisplayTime);
            yield return fader.FadeIn(1f);
        }

        public object CaptureState()
        {
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            alreadyTriggered = (bool)state;
        }

    }
}

