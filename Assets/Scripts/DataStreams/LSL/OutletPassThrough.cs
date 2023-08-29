using UnityEngine;
using UnityEngine.Events;
using static LSL.liblsl;

namespace LSL
{
    [System.Serializable]
    public class m_LSL_Event : UnityEvent<string>
    {
    }
    public enum Marker { game_start = 0, ball_release = 1, ball_good_hit = 2, ball_bad_hit = 3, score = 4, test = 5, end_game = 6, LSL_test = 7, caregiver_in_view = 8, caregiver_starts_feedback = 9, caregiver_stops_feedback = 10 };

    public class OutletPassThrough : MonoBehaviour
    {
        /*
         * This is a simple example of an LSL Outlet to stream out irregular events occurring in Unity.
         * This uses only LSL.cs and is intentionally simple. For a more robust version, see another sample.
         * 
         * We stream out the trigger event during OnTriggerEnter which is, in our opinison, the closest
         * time to when the trigger actually occurs (i.e., independent of its rendering).
         * A simple way to print the events is with pylsl: `python -m pylsl.examples.ReceiveStringMarkers`
         *
         * If you are instead trying to log a stimulus event then there are better options. Please see the 
         * LSL4Unity SimpleStimulusEvent Sample for such a design.
         */

        #region SingletonPattern
        private static OutletPassThrough _instance;
        public static OutletPassThrough Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("The OutletPassThrough is Null");
                }
                return _instance;
            }
        }
        private void Awake()
        {
            _instance = this;
        }
        #endregion

        [SerializeField] private string StreamName_1 = "DataSyncMarker_emotibit";
        [SerializeField] private string StreamName_2 = "DataSyncMarker_eeg";
        [SerializeField] private string ContentType_1 = "Markers";
        [SerializeField] private string ContentType_2 = "Markers";
        [SerializeField] private string StreamId_1 = "LSL1";
        [SerializeField] private string StreamId_2 = "LSL2";
        [SerializeField] private channel_format_t channel_format_type = channel_format_t.cf_string;

        private StreamOutlet outlet_1;
        private StreamOutlet outlet_2;
        private string[] sample = { "" };
        //private float i = 0.0f;

        void Start()
        {
            StreamInfo streamInfo1 = new(StreamName_1, ContentType_1, 1, IRREGULAR_RATE,
                channel_format_type, StreamId_1); //int8 should be more than enough for coding the differing signals
            outlet_1 = new StreamOutlet(streamInfo1);
            StreamInfo streamInfo2 = new(StreamName_2, ContentType_2, 1, IRREGULAR_RATE,
                channel_format_type, StreamId_2); //int8 should be more than enough for coding the differing signals
            outlet_2 = new StreamOutlet(streamInfo2);
        }
        public void SendMarker(Marker marker)
        {
            //Debug.Log(Time.time + (int)marker);
            //switch (marker)
            //{
            //    case Marker.ball_release:
            //    case Marker.ball_good_hit:
            //    case Marker.ball_bad_hit:
            //    case Marker.score:
            //        SendMarker((int)marker);
            //        break;
            //    case Marker.test:
            //        SendMarker((int)marker);
            //        break;
            //    default:
            //        Debug.Log("unknown command");
            //        break;
            //}
            SendMarker((int)marker);
        }

        private void SendMarker(int marker)
        {
            sample[0] = "" + marker;
            if (outlet_1 != null)
            {
                outlet_1.push_sample(sample);
                //Debug.Log("push sample outlet 1");

            }
            if (outlet_2 != null)
            {
                outlet_2.push_sample(sample);
                //Debug.Log("push sample outlet 2");

            }
            //Debug.Log("LSL stream: " + Time.time + " " + sample[0]);

        }

        public void LSLTest()
        {
            Debug.Log("Sending LSL test");
            SendMarker(Marker.LSL_test);
        }
    }
}