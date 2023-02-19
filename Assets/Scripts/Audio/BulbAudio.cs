using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    // this is really specifically a health crystal script
    public class BulbAudio : MonoBehaviour
    {
        const int pluginKey = 1; //the value we set the "Instance Index" param to in the mixer
        AlkarraSoundHelper helper;
        GameObject player;
        bool shouldInteractWithPlayer = false;
        [SerializeField] AudioClip healthAdded;
        System.UInt32 sampleThreeInport;



        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            player = GameObject.FindWithTag("Player");

            if (healthAdded)
            {
                float[] samples = new float[healthAdded.samples * healthAdded.channels];
                healthAdded.GetData(samples, 0);
                helper.LoadDataRef("sampleThree", samples, healthAdded.channels, healthAdded.frequency);
            }

            // helper.SetParamValue(27, 0.6); // slow down sampleThree

        }

        // Update is called once per frame
        void Update()
        {
            if (!shouldInteractWithPlayer) return;
            float distanceToBulb = Vector3.Distance(player.transform.position, transform.position);
            float bulbRadius = GetComponent<CapsuleCollider>().radius;
            if (distanceToBulb < bulbRadius)
            {
                float playbackSpeedMultiplicationFactor = (bulbRadius - distanceToBulb) * 0.01f;
                helper.SetParamValue(19, 1 + playbackSpeedMultiplicationFactor); // playback speed for "sample"
            }

        }

        public void AddHealthSound ()
        {
            helper.SetParamValue(27, 0.6); // slow down sampleThree
            sampleThreeInport = AlkarraSoundHelper.Tag("playSampleThree");
            helper.SendMessage(sampleThreeInport, 1);
        }


        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == player)
            {
                shouldInteractWithPlayer = true;
            }

        }
        void OnCollisionExit(Collision other)
        {
            if (other.gameObject == player)
            {
                shouldInteractWithPlayer = false;
                helper.SetParamValue(19, 1); // playback speed for "sample"
            }


        }
    }
}


