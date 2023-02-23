using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using System;

namespace RPG.Story
{
    
    // I can probably render this class unnecessary by making RPG.SceneManagement.Fader more flexible
    public class StoryTriggerSphere : MonoBehaviour, ISaveable
    {

        bool entryAlreadyTriggered = false;
        bool exitAlreadyTriggered = false;

        [SerializeField] GameObject entryTextPromptPrefab = null;
        [SerializeField] GameObject textPromptPrefab = null;
        [SerializeField] float storyDisplayTime = 3;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player" && entryTextPromptPrefab != null && !entryAlreadyTriggered)
            {
                GameObject entryTextPromptInstance = Instantiate(entryTextPromptPrefab);
                StartCoroutine(StoryDialogueFade(entryTextPromptInstance));
                entryAlreadyTriggered = true;
            }

        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.tag == "Player" && !exitAlreadyTriggered)
            {
                GameObject textPromptInstance = Instantiate(textPromptPrefab);
                StartCoroutine(StoryDialogueFade(textPromptInstance));
                exitAlreadyTriggered = true;
            }
            
        }

        private IEnumerator StoryDialogueFade(GameObject textPrompt)
        {
            StoryFader fader = textPrompt.GetComponent<StoryFader>();

            // this is a little backwards bc alpha = 1 is considered "out"
            yield return fader.FadeOut(1f);
            yield return new WaitForSeconds(storyDisplayTime);
            yield return fader.FadeIn(1f);
        }

        public object CaptureState()
        {
            return exitAlreadyTriggered;
        }

        public void RestoreState(object state)
        {
            exitAlreadyTriggered = (bool) state;
        }
    }
}


