using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class CoreAudio : MonoBehaviour
    {
        int pluginKey = 1; //the value we set the "Instance Index" param to in the mixer
        AlkarraSoundHelper helper;
        public AudioClip rnboClip;
        System.UInt32 myInport;


        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            helper.SetParamValue(0, 0); // turning off karplusMetroOn
            helper.SetParamValue(1, 400); // setting initial metro
            //helper.SetParamValue(20, 1); // setting delayL to 1 (overriden by bulb)
            //helper.SetParamValue(22, 1); // setting delayR to 1 (overriden by bulb)
            helper.SetParamValue(4, 1); // stringsync
            helper.SetParamValue(11, 1); // rotate
            helper.SetParamValue(17, 1); // loop sample

            if (rnboClip)
            {
                float[] samples = new float[rnboClip.samples * rnboClip.channels];
                rnboClip.GetData(samples, 0);
                helper.LoadDataRef("sample", samples, rnboClip.channels, rnboClip.frequency);
            }

            myInport = AlkarraSoundHelper.Tag("playSample");
            helper.SendMessage(myInport, 1);


        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


