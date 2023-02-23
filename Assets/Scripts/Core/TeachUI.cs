using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Story;
using RPG.Saving;

namespace RPG.Core
{
    public class TeachUI : MonoBehaviour, ISaveable
    {
        [SerializeField] GameObject uiTextPrefab;
        [SerializeField] float storyDisplayTime = 30;
        [SerializeField] float waitTime = 0;
        bool alreadyTriggered = false;
        
        // Start is called before the first frame update
        IEnumerator Start()
        {
            yield return new WaitForSeconds(waitTime);
            GameObject textPromptInstance = Instantiate(uiTextPrefab);
            //StartCoroutine(StoryDialogueFade(textPromptInstance));
            yield return StoryDialogueFade(textPromptInstance);
            alreadyTriggered = true;
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
            return alreadyTriggered;
        }

        public void RestoreState(object state)
        {
            alreadyTriggered = (bool) state;
        }
    }
}

