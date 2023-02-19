using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class CoreAudio : MonoBehaviour
    {
        const int pluginKey = 1; //the value we set the "Instance Index" param to in the mixer
        AlkarraSoundHelper helper;
        public AudioClip background;
        System.UInt32 myInport;


        // Start is called before the first frame update
        void Start()
        {
            helper = AlkarraSoundHelper.FindById(pluginKey);
            helper.SetParamValue(3, 0); // turning off karplusMetroOn
            helper.SetParamValue(4, 400); // setting initial metro
            //helper.SetParamValue(20, 1); // setting delayL to 1 (overriden by bulb)
            //helper.SetParamValue(22, 1); // setting delayR to 1 (overriden by bulb)
            helper.SetParamValue(8, 1); // stringsync
            helper.SetParamValue(15, 1); // rotate
            helper.SetParamValue(21, 1); // loop sample
            helper.SetParamValue(19, 1); // start sample playback rate at 1

            if (background)
            {
                float[] samples = new float[background.samples * background.channels];
                background.GetData(samples, 0);
                helper.LoadDataRef("sample", samples, background.channels, background.frequency);
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


