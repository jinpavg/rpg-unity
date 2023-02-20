using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.SceneManagement;

namespace RPG.Core
{
    public class TitleScreen : MonoBehaviour
    {
        int sceneToLoad = 1;
        [SerializeField] float fadeInTime = 5;
        [SerializeField] float fadeOutTime = 5;
        [SerializeField] float fadeWaitTime = 1;
        LoadFader fader;

        IEnumerator Start() 
        {
            fader = GetComponent<LoadFader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Transition());
            }
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

            yield return fader.FadeOut(fadeOutTime);

            //wrapper.Save(); // ensures there is a save file to delete
            wrapper.Delete();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // load current level
            wrapper.Load();
            
//            wrapper.Save();
            
            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }
    }
}

