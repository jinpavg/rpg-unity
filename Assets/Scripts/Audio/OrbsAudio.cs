using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Audio
{
    public class OrbsAudio : MonoBehaviour
    {
        const int pluginKey = 1;
        const int orbsKey = 2;
        AlkarraSoundHelper helper;
        AlkarraOrbsHelper orbsHelper;
        System.UInt32 sampleThreeInport;

        GameObject player;
        float orbsGain = 0;
        [SerializeField] float fadeInTime = 3;
        [SerializeField] float fadeOutTime = 3;

        double harmonicsLFOValue;
        double cutoffValue;
        double leftDelayValue;
        double rightDelayValue;
        double overblowValue;

        Transform bottomCube;
        Transform secondFromBottomCube;
        Transform thirdFromBottomCube;
        Transform fourthFromBottomCube;
        Transform topCube;
        //Transform travelSphere;
        //float rotationSpeed = 0.1f;


        // Start is called before the first frame update
        void Start()
        {
            orbsHelper = AlkarraOrbsHelper.FindById(orbsKey);
            helper = AlkarraSoundHelper.FindById(pluginKey);
            player = GameObject.FindWithTag("Player");
            bottomCube = gameObject.transform.GetChild(0);
            secondFromBottomCube = gameObject.transform.GetChild(1);
            thirdFromBottomCube = gameObject.transform.GetChild(2);
            fourthFromBottomCube = gameObject.transform.GetChild(3);
            topCube = gameObject.transform.GetChild(4);
            //travelSphere = gameObject.transform.GetChild(5);

            //lfoLFO is off. i like it, but it causes glitches, need to change the scale from 0 1 3 10 to 0 1 0.01 0.1 or something (see RNBO patch)
            orbsHelper.SetParamValue(3, 0);
            orbsHelper.SetParamValue(9, 0.1); // harmonicLFO


        }

        void Update()
        {
            // i could extract a method here
            orbsHelper.GetParamValueNormalized(9, out harmonicsLFOValue);
            bottomCube.Rotate(new Vector3(0, 360 * (float)harmonicsLFOValue, 0));

            orbsHelper.GetParamValueNormalized(17, out leftDelayValue);
            secondFromBottomCube.Rotate(new Vector3(0, 360 * (float)leftDelayValue, 0));

            thirdFromBottomCube.Rotate(new Vector3(0, -360 * (float)leftDelayValue, 0));

            orbsHelper.GetParamValueNormalized(19, out rightDelayValue);
            fourthFromBottomCube.Rotate(new Vector3(0, 360 * (float)rightDelayValue, 0));

            //orbsHelper.GetParamValueNormalized(2, out overblowValue);
            topCube.Rotate(new Vector3(0, -360 * (float)harmonicsLFOValue, 0));

            //orbsHelper.GetParamValueNormalized(0, out cutoffValue);
            //float scaledHarmonic = (float)harmonicsLFOValue * 2f - 1f;
            //Vector3 travelSpherePosition = new Vector3 (0, (float)cutoffValue * 2.7f, 0);
            //travelSphere.Translate(Vector3.up * scaledHarmonic / 1000f);

        }

        // requires that a sample has already been loaded into the sampleThree buffer elsewhere
        public void AddHealthSound()
        {
            helper.SetParamValue(27, 0.4); // slow down sampleThree
            sampleThreeInport = AlkarraSoundHelper.Tag("playSampleThree");
            helper.SendMessage(sampleThreeInport, 1);
        }

        private IEnumerator RaiseBulbGain(float time)
        {
            while (orbsGain < 0.7f)
            {
                orbsGain += Time.deltaTime / time;
                orbsHelper.SetParamValue(5, orbsGain);
                yield return null;
            }
        }
        private IEnumerator LowerBulbGain(float time)
        {
            while (orbsGain > 0f)
            {
                orbsGain -= Time.deltaTime / time;
                orbsHelper.SetParamValue(5, orbsGain);
                yield return null;
            }
        }


        void OnTriggerEnter(Collider other)
        {

            if (other.gameObject == player)
            {
                StartCoroutine(RaiseBulbGain(fadeInTime));

                //orbsHelper.SetParamValue(5, 0.4); // orbs sound
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject == player)
            {
                StartCoroutine(LowerBulbGain(fadeOutTime));
                //orbsHelper.SetParamValue(5, 0); // no orbs sound
            }
        }
    }
}

