using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Story
{
public class StoryFader : MonoBehaviour
{
    CanvasGroup canvasGroup;
        //GameObject sceneFader;

        private void Awake()
        {
            //GameObject sceneFader = GameObject.FindWithTag("SceneFader");
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }

        public IEnumerator FadeOut(float time)
        {
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
}
}

