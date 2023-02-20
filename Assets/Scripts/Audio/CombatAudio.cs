using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Audio
{
    public class CombatAudio : MonoBehaviour
    {
        const int bufferKey = 3; //the value we set the "Instance Index" param to in the mixer
        BufferTriggerHelper bufferHelper;
        [SerializeField] AudioClip hitSound;
        [SerializeField] AudioClip deathSound;
        //[SerializeField] AudioClip spellHitSound;
        [SerializeField] int whichInport = 2; // this should be an enum or something, and also controls the deathInport, which is hacky
        [SerializeField] float hitSoundGain = 0.5f;
        [SerializeField] float deathSoundGain = 0.5f;
        [SerializeField] float pitchDownFactor = 1f;

        System.UInt32 myInport;
        System.UInt32 deathInport; // do I need a seperate one?

        private void Start()
        {
            bufferHelper = BufferTriggerHelper.FindById(bufferKey);

            if (hitSound)
            {
                float[] samples = new float[hitSound.samples * hitSound.channels];
                hitSound.GetData(samples, 0);
                if (whichInport == 2)
                {
                    bufferHelper.LoadDataRef("sampleTwo", samples, hitSound.channels, hitSound.frequency);
                } else if (whichInport == 3)
                {
                    bufferHelper.LoadDataRef("sampleThree", samples, hitSound.channels, hitSound.frequency);
                }
            }

            if (deathSound)
            {
                float[] samples = new float[deathSound.samples * deathSound.channels];
                deathSound.GetData(samples, 0);
                if (whichInport == 2)
                {
                    bufferHelper.LoadDataRef("sampleFour", samples, deathSound.channels, deathSound.frequency);
                } else if (whichInport == 3)
                {
                    bufferHelper.LoadDataRef("sampleFive", samples, deathSound.channels, deathSound.frequency);
                }
            }

            // helper.ParameterChangedEvent += (object sender, AlkarraSoundHelper.ParameterChangedEventArgs args) => {
            //     Debug.LogFormat("got param change {0} {1} {2}", args.Index, args.Value, args.Time);
            // };
            // helper.MessageEvent += (object sender, AlkarraSoundHelper.MessageEventArgs args) => {
            //     Debug.LogFormat("got message {0} [{1}] {2}", args.Tag, string.Join(", ", args.Values), args.Time);
            // };
        }

        public void PlayImpactSound()
        {
            if (whichInport == 2)
            {
                float playBackRate = Random.Range(0.7f, 1.1f);
                bufferHelper.SetParamValue(4, playBackRate); // slow down sampleTwo
                bufferHelper.SetParamValue(7, hitSoundGain); // set sampleTwo gain
                myInport = BufferTriggerHelper.Tag("playSampleTwo");
                bufferHelper.SendMessage(myInport, 1);
            } else if (whichInport == 3)
            {
                float playBackRate = Random.Range(0.4f, 1.1f);
                bufferHelper.SetParamValue(8, playBackRate * pitchDownFactor); // slow down sampleThree
                myInport = BufferTriggerHelper.Tag("playSampleThree");
                bufferHelper.SendMessage(myInport, 1);
            }
            
        }

        public void PlayDeathSound()
        {
            if (whichInport == 2)
            {
                bufferHelper.SetParamValue(15, deathSoundGain); // sampleFour gain
                bufferHelper.SetParamValue(12, pitchDownFactor); // slow down sampleFour
                deathInport = BufferTriggerHelper.Tag("playSampleFour");
                bufferHelper.SendMessage(deathInport, 1); 
            } else if (whichInport == 3)
            {
                bufferHelper.SetParamValue(19, deathSoundGain); // sampleFive gain
                bufferHelper.SetParamValue(16, pitchDownFactor); // slow down sampleFive
                deathInport = BufferTriggerHelper.Tag("playSampleFive");
                bufferHelper.SendMessage(deathInport, 1); 
            }

        }

    }

}
