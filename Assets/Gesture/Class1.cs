using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Kinect;
using System.Text;

public class bla : MonoBehaviour
{
    // GameObject of BodyManager
    public GameObject BodyManager;

    // private variable for usage of BodyManager
    private BodyManager _bodyManager;

    // Kinect sensor for usage
    private KinectSensor _kinectSensor;

    public Text ConfidenceTextGameObject;
    public Text GestureTextGameObject;

    public Text TimeTextGameObject;
    public Text TimeTextGameObject2;
    public Text TimeTextGameObject3;

    public float _time;
    public float _time2;
    public float _time3;
    public float _time4;


    private byte[] _colorData;
    private Texture2D _colorTexture;

    private BodyFrameReader _bodyFrameReader;
    private int _bodyCount;
    private Body[] _bodies = null;

    private string _leanRightGestureName = "Lean_Right";
    private string _leanLeftGestureName = "Lean_Left";

    private UnityEngine.Color[] _bodyColors;

    private List<GestureDetector> _gestureDetectorList = null;



    // Use this for initialization
    void Start()
    {

        _bodyManager = BodyManager.GetComponent<BodyManager>();
        _kinectSensor = _bodyManager.GetSensor();

        if (_kinectSensor != null)
        {

            // # of bodies (which is max. 6 like here)
            _bodyCount = _kinectSensor.BodyFrameSource.BodyCount;
            Debug.Log(_bodyCount);

            _bodyFrameReader = _kinectSensor.BodyFrameSource.OpenReader();


            // initialize Body array with # of of bodies
            _bodies = new Body[_bodyCount];

            // Initialize new GestureDetector list
            _gestureDetectorList = new List<GestureDetector>();

            // For every body add a new GestureDetector instance to the list
            for (int bodyIndex = 0; bodyIndex < _bodyCount; bodyIndex++)
            {
                GestureTextGameObject.text = "none";
                _gestureDetectorList.Add(new GestureDetector(_kinectSensor));
            }

            // open KinectSensor for usage
            _kinectSensor.Open();
        }
        else
        {
            // kinect sensor not connected
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool newBodyData = false;
        using (BodyFrame bodyFrame = _bodyFrameReader.AcquireLatestFrame())
        {
            if (bodyFrame != null)
            {
                bodyFrame.GetAndRefreshBodyData(_bodies);
                newBodyData = true;
            }
        }

        if (newBodyData)
        {
            for (int bodyIndex = 0; bodyIndex < _bodyCount; bodyIndex++)
            {
                var body = _bodies[bodyIndex];
                if (body != null)
                {
                    var trackingId = body.TrackingId;

                    if (trackingId != _gestureDetectorList[bodyIndex].TrackingId)
                    {
                        GestureTextGameObject.text = "none";

                        _gestureDetectorList[bodyIndex].TrackingId = trackingId;

                        _gestureDetectorList[bodyIndex].IsPaused = (trackingId == 0);
                        _gestureDetectorList[bodyIndex].OnGestureDetected += CreateOnGestureHandler(bodyIndex);
                        Debug.Log(_gestureDetectorList[bodyIndex]);
                    }
                }
            }
        }

    }

    private EventHandler<GestureEventArgs> CreateOnGestureHandler(int bodyIndex)
    {
        Debug.Log(bodyIndex);
        return (object sender, GestureEventArgs e) => OnGestureDetected(sender, e, bodyIndex);
    }

    private void OnGestureDetected(object sender, GestureEventArgs e, int bodyIndex)
    {
        var isDetected = e.IsBodyTrackingIdValid && e.IsGestureDetected;
        //TimeTextGameObject.text = "FALSE";
        _time3 += Time.deltaTime;
        TimeTextGameObject3.text = _time3.ToString();

        Debug.Log(e.GestureID);
        if ((e.GestureID == _leanRightGestureName) || (e.GestureID == _leanLeftGestureName) )
        {
            Debug.Log("efg");
            GestureTextGameObject.text = "Gesture: " + isDetected;
            ConfidenceTextGameObject.text = "Confidence: " + e.DetectionConfidence;

            _time2 += Time.deltaTime;
            TimeTextGameObject2.text = _time2.ToString();

            if (e.DetectionConfidence > 0.4f)
            {
                ConfidenceTextGameObject.text += " WHOOP ";
                _time += Time.deltaTime;
                TimeTextGameObject.text = _time.ToString();
            }
            else
            {
                _time = 0;
            }

//            TimeTextGameObject.text = _time.ToString();

        }
    }

    void OnApplicationQuit()
    {
        if (this._bodyFrameReader != null)
        {
            this._bodyFrameReader.Dispose();
            this._bodyFrameReader = null;
        }

        if (this._kinectSensor != null)
        {
            if (this._kinectSensor.IsOpen)
            {
                this._kinectSensor.Close();
            }

            this._kinectSensor = null;
        }
    }

}