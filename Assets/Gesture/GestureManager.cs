using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Kinect;
using System.Text;

public class GestureManager : MonoBehaviour
{
    // GameObject of BodyManager
    public GameObject BodyManager;

    // private variable for usage of BodyManager
    private BodyManager _bodyManager;

    // Kinect sensor for usage
    private KinectSensor _kinectSensor;

    public GameObject  CanvasExerciseManager;
    private CanvasExerciseManager _canvasExerciseManager;

    // Text GameObjects
    public Text ConfidenceTextGameObject;
    public Text GestureTextGameObject;
    public Text TimeTextGameObject;

    public float _time;
    public float _time2;

    public Image TimeImage;

    private byte[] _colorData;
    private Texture2D _colorTexture;

    private BodyFrameReader _bodyFrameReader;
    private Body[] _bodies = null;

    private string _leanRightGestureName = "Lean_Right";
    private string _leanLeftGestureName = "Lean_Left";

    private UnityEngine.Color[] _bodyColors;

    private List<GestureDetector> _gestureDetectorList = null;

    // Use this for initialization
    void Start()
    {

        if (BodyManager == null)
        {
            return;
        }

        _bodyManager = BodyManager.GetComponent<BodyManager>();
        if (_bodyManager == null)
        {
            return;
        }
        _canvasExerciseManager = CanvasExerciseManager.GetComponent<CanvasExerciseManager>();

        _kinectSensor = _bodyManager.GetSensor();

        // initialize Body array with # of maximum bodies
        _bodies = _bodyManager.GetBodies();
        Debug.Log(_bodyManager + " | " + _bodies);
        //_bodyFrameReader = _kinectSensor.BodyFrameSource.OpenReader();

        // Initialize new GestureDetector list
        _gestureDetectorList = new List<GestureDetector>();
        // For every body add a new GestureDetector instance to the list

        for (int bodyIndex = 0; bodyIndex < _bodies.Length; bodyIndex++)
        {
            GestureTextGameObject.text = "none";
            _gestureDetectorList.Add(new GestureDetector(_kinectSensor));
        }
    }

    // Update is called once per frame
    void Update()
    {
    /*
        bool newBodyData = false;
        using (BodyFrame bodyFrame = _bodyFrameReader.AcquireLatestFrame())
        {
            if (bodyFrame != null)
            {
                bodyFrame.GetAndRefreshBodyData(_bodies);
                newBodyData = true;
            }
        }
    */

        //if (newBodyData)
        if (_bodyManager.NewBodyData())
        {
            for (int bodyIndex = 0; bodyIndex < _bodies.Length; bodyIndex++)
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
                    }
                }
            }
        }
    }// Update

    private EventHandler<GestureEventArgs> CreateOnGestureHandler(int bodyIndex)
    {
        return (object sender, GestureEventArgs e) => OnGestureDetected(sender, e, bodyIndex);
    }

    private void OnGestureDetected(object sender, GestureEventArgs e, int bodyIndex)
    {
        var isDetected = e.IsBodyTrackingIdValid && e.IsGestureDetected;

       // Debug.Log(_canvasExerciseManager.IsReady());

        if ((e.GestureID == _leanRightGestureName || e.GestureID == _leanLeftGestureName) && _canvasExerciseManager.GetReadyState() == "true" )
        {

            GestureTextGameObject.text = "Gesture: " + isDetected;
            ConfidenceTextGameObject.text = "Confidence: " + e.DetectionConfidence;

            _time2 += Time.deltaTime;

            if (e.DetectionConfidence > 0.4f)
            {

                // Fill the bar for duration
                _canvasExerciseManager.StartTime();

/*
                ConfidenceTextGameObject.text += " WHOOP ";

                _time += Time.deltaTime;
                TimeTextGameObject.text = _time.ToString();

                TimeImage.fillAmount = _time/5.0f;

                if (TimeImage.fillAmount >= 1.0f)
                {
                    readyState = false;
                    //AttemptSuccess;

            */
            }
        }

    }// OnGestureDetected
}// Class