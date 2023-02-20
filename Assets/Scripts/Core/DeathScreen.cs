using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.SceneManagement;

namespace RPG.Core
{
    public class DeathScreen : MonoBehaviour
    {
        int sceneToLoad = 1;
        [SerializeField] float fadeInTime = 5;
        [SerializeField] float fadeOutTime = 5;
        [SerializeField] float fadeWaitTime = 1;
        LoadFader fader;

        IEnumerator Start() 
        {
            fader = GetComponentInChildren<LoadFader>();
            yield return fader.FadeOut(fadeOutTime);
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Transition());
            }
        }

        public void SetDeathLocation(int whereYouDied)
        {
            sceneToLoad = whereYouDied;
        }

        private IEnumerator Transition()
        {
            if (sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            // LoadFader fader = FindObjectOfType<LoadFader>();
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();


            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // load current level
            wrapper.Load();
            
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }
    }
}

