using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.SceneManagement;
using RPG.Control;

namespace RPG.Core
{
    public class DeathScreen : MonoBehaviour
    {
        int sceneToLoad = 1;
        [SerializeField] float fadeInTime = 5;
        [SerializeField] float fadeOutTime = 5;
        [SerializeField] float fadeWaitTime = 4;
        LoadFader fader;
        //bool hasBeenTriggered = false;
        GameObject player;

        IEnumerator Start() 
        {
            // player = GameObject.FindWithTag("Player");
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
            //hasBeenTriggered = true;

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
            
            player = GameObject.FindWithTag("Player");
            yield return DisableControl();

            yield return new WaitForSeconds(fadeWaitTime);

            yield return fader.FadeIn(fadeInTime);

            yield return EnableControl();
   
            Destroy(gameObject);
        }

        IEnumerator DisableControl()
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction(); 
            player.GetComponent<PlayerController>().enabled = false;
            yield return null;
        }

        IEnumerator EnableControl()
        {
            player.GetComponent<PlayerController>().enabled = true;
            yield return null;
        }
    }
}

