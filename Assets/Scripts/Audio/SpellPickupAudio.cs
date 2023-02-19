using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        float waveshapeGain = 0;
        [SerializeField] float fadeInTime = 3;
        [SerializeField] float fadeOutTime = 3;
        [SerializeField] AudioClip spellPickup;
        System.UInt32 sampleOneInport;



        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            bufferHelper = BufferTriggerHelper.FindById(bufferKey);
            player = GameObject.FindWithTag("Player");

            helper.SetParamValue(9, startingFreq); // set waveshape start freq
            //helper.SetParamValue(10, 0f); // waveshape gain

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

        }

        public void SpellPickupSound()
        {
            bufferHelper.SetParamValue(0, 0.6); // slow down sampleOne
            sampleOneInport = BufferTriggerHelper.Tag("playSampleOne");
            bufferHelper.SendMessage(sampleOneInport, 1);
        }

        private IEnumerator RaiseWaveshapeGain(float time)
        {
            while (waveshapeGain < 0.4f)
            {
                waveshapeGain += Time.deltaTime / time;
                helper.SetParamValue(10, waveshapeGain);
                yield return null;
            }
        }
        private IEnumerator LowerWaveshapeGain(float time)
        {
            while (waveshapeGain > 0f)
            {
                waveshapeGain -= Time.deltaTime / time;
                helper.SetParamValue(10, waveshapeGain);
                yield return null;
            }
        }


        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == player)
            {
                shouldInteractWithPlayer = true;
                StartCoroutine(RaiseWaveshapeGain(fadeInTime));
            }

        }
        void OnCollisionExit(Collision other)
        {
            if (other.gameObject == player)
            {
                shouldInteractWithPlayer = false;
                StartCoroutine(LowerWaveshapeGain(fadeOutTime));
            }


        }
    }
}


