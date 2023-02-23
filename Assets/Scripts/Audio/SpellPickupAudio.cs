using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;

namespace RPG.Audio
{
    // this is really specifically a health crystal script
    public class SpellPickupAudio : MonoBehaviour
    {
        const int pluginKey = 1; //the value we set the "Instance Index" param to in the mixer
        const int bufferKey = 3; //the value we set the "Instance Index" param to in the mixer

        AlkarraSoundHelper helper;
        BufferTriggerHelper bufferHelper;
        GameObject player;
        bool shouldInteractWithPlayer = false;
        [SerializeField] float startingFreq = 1000;
        double waveshapeGain;
        [SerializeField] float fadeInTime = 3;
        [SerializeField] float fadeOutTime = 3;
        [SerializeField] float targetGainHigh = 0.3f;
        [SerializeField] AudioClip spellPickup;
        System.UInt32 sampleOneInport;

        Coroutine currentActiveFade = null;

        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            bufferHelper = BufferTriggerHelper.FindById(bufferKey);
            player = GameObject.FindWithTag("Player");

            helper.SetParamValue(9, startingFreq); // set waveshape start freq
            helper.SetParamValue(10, 0f); // waveshape gain starts off, for sure
            helper.GetParamValue(10, out waveshapeGain);

            if (spellPickup)
            {
                float[] samples = new float[spellPickup.samples * spellPickup.channels];
                spellPickup.GetData(samples, 0);
                bufferHelper.LoadDataRef("sampleOne", samples, spellPickup.channels, spellPickup.frequency);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (!shouldInteractWithPlayer) return;
            float distanceToBulb = Vector3.Distance(player.transform.position, transform.position);
            float bulbRadius = GetComponent<CapsuleCollider>().radius;
            if (distanceToBulb < bulbRadius)
            {
                helper.SetParamValue(7, distanceToBulb * (126 / bulbRadius)); // waveshape metro rate
            }
            if (GetComponent<SpellPickup>().isHidden)
            {
                helper.SetParamValue(10, 0); //drop the waveshape once the crystal is picked up
            }

        }

        public void SpellPickupSound()
        {
            bufferHelper.SetParamValue(0, 0.6); // slow down sampleOne
            sampleOneInport = BufferTriggerHelper.Tag("playSampleOne");
            bufferHelper.SendMessage(sampleOneInport, 1);
        }



        public IEnumerator FadeGainIn(float time)
        {
            return Fade(targetGainHigh, time);
        }

        public IEnumerator FadeGainOut(float time)
        {
            return Fade(0, time);
        }


        public IEnumerator Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            yield return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately((float)waveshapeGain, target))
            {
                waveshapeGain = Mathf.MoveTowards((float)waveshapeGain, target, Time.deltaTime / time);
                helper.SetParamValue(10, waveshapeGain);
                yield return null;
            }
        }


        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == player)
            {
                shouldInteractWithPlayer = true;
                StartCoroutine(FadeGainIn(fadeInTime));
            }

        }
        void OnCollisionExit(Collision other)
        {
            if (other.gameObject == player)
            {
                shouldInteractWithPlayer = false;
                StartCoroutine(FadeGainOut(fadeOutTime));
            }


        }
    }
}


