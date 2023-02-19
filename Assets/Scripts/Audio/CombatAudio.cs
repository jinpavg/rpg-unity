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
        //[SerializeField] AudioClip spellHitSound;
        [SerializeField] int whichInport = 2; // this should be an enum or something
        [SerializeField] float hitSoundGain = 0.5f;

        System.UInt32 myInport;

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
                bufferHelper.SetParamValue(8, playBackRate); // slow down sampleThree
                myInport = BufferTriggerHelper.Tag("playSampleThree");
                bufferHelper.SendMessage(myInport, 1);
            }
            
        }

    }

}
