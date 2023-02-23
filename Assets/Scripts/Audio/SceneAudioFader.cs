using System.Collections;
using UnityEngine;

namespace RPG.Audio
{
    public class SceneAudioFader : MonoBehaviour 
    {
        [SerializeField] int sampleToPlay = 1;
        [SerializeField] float fadeTime = 3;
        double backgroundGain;
        double loadingScreenGain;
        const int pluginKey = 1;
        AlkarraSoundHelper helper;

        private void Start() 
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);

            helper.GetParamValueNormalized(22, out backgroundGain);
            helper.GetParamValueNormalized(26, out loadingScreenGain);

            StartCoroutine(FadeIn(sampleToPlay, fadeTime));
            StartCoroutine(FadeOut(sampleToPlay, fadeTime));
        }
        
        
        private IEnumerator FadeIn(int sampleIndex, float time) 
        {
            if (sampleIndex > 1 || sampleIndex < 0) yield break;
            if (sampleIndex == 0)
            {
                while (loadingScreenGain < 0.7f)
                {
                    loadingScreenGain += Time.deltaTime / time;
                    helper.SetParamValue(26, loadingScreenGain);
                    yield return null;
                }
            }
            else if (sampleIndex == 1)
            {
                while (backgroundGain < 0.5f)
                {
                    backgroundGain += Time.deltaTime / time;
                    helper.SetParamValue(22, backgroundGain);
                    yield return null;
                }
            }
        }

        private IEnumerator FadeOut(int sampleIndex, float time) 
        {
            if (sampleIndex > 1 || sampleIndex < 0) yield break;
            if (sampleIndex == 0)
            {
                while (backgroundGain > 0)
                {
                    backgroundGain -= Time.deltaTime / time;
                    helper.SetParamValue(22, backgroundGain);
                    yield return null;
                }
            }
            else if (sampleIndex == 1)
            {
                while (loadingScreenGain > 0)
                {
                    loadingScreenGain -= Time.deltaTime / time;
                    helper.SetParamValue(26, loadingScreenGain);
                    yield return null;
                }
            }
        }


    }
}