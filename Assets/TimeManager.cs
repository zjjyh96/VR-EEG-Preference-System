using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.ProgressBars.Scripts;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using Looxid;
using System;

public class TimeManager : MonoBehaviour
{
    public enum Conditions { NoDisplay, SomeDisplay, FullDisplay };
    public enum Views { Description, Distribution, Review, Rating, Blank, Fixation };
    public enum ConditionOrders { Order123, Order132, Order213, Order231, Order312, Order321 };

    public GameObject infoBox, rating, distribution, reviews, phaseChange;
    public GameObject practice, praDes, praRat;
    public CircularTimer timer;
    public GameObject timerObj;
    public GameObject fix, rateManager;

    public int trailCount, totalTrail, breakingTime;
    public bool isPractice, isPhase1, isPhase2, isBreaking, isFixiation;

    public Conditions condition;
    public Views view;
    public ConditionOrders order;

    private Dictionary<ConditionOrders, Conditions[]> conditionMap;
    private Dictionary<Views, string> viewNameMap;

    private GuiProgressBar progressBar;
    private ImageLoad imageLoader;
    private JsonLoad jsonLoader;
    private Text phaseChangeText;
    private RectTransform timerTrans;

    private string sessionName, trailName;

    private LXVRManager lxvrManager;

    [Serializable]
    public class Event
    {
        public Conditions Condition;
        public Views View;
        public int ProductId;
    };

    void Start()
    {
        progressBar = GameObject.Find("ProgressBarBlock").GetComponentInChildren<GuiProgressBar>();
        imageLoader = GameObject.Find("LoadManager").GetComponent<ImageLoad>();
        jsonLoader = GameObject.Find("LoadManager").GetComponent<JsonLoad>();
        phaseChangeText = phaseChange.GetComponentInChildren<Text>();
        timerTrans = timerObj.GetComponent<RectTransform>();

        InitOrderMap();
        InitViewNameMap();
        InitViewShow();
        InitLooxid();
    }

    public void PracticeDescription()
    {
        Debug.Log("Call PracticeDes");
        view = Views.Description;
        Display(view);
        praDes.SetActive(true);
        praRat.SetActive(false);
        StartTimer();
    }

    public void PracticeRating()
    {
        Debug.Log("Call PracticeRating");
        view = Views.Rating;
        Display(view);
        praDes.SetActive(false);
        praRat.SetActive(true);
        StopTimer();
        StartTimer();
    }

    public void StartPhase()
    {
        trailCount = 0;
        view = Views.Fixation;
        condition = GetCondition(order, 0);
        timerObj.SetActive(true);
        TimerTransfrom();
        Display(view);
        StopTimer();
        StartTimer();
        if (isPhase1)
        {
            sessionName = "Phase1";
            WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "Phase 1 Calibration Start.");

        }
        if (isPhase2)
        {
            sessionName = "Phase2";
            WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "Phase 2 Calibration Start.");
        }
        trailName = "Calibration";
        LXVRManager.Instance.StartRecording(sessionName, trailName, true, startRecordingResultCallback);
        progressBar.Value = trailCount / (float)totalTrail;
    }

    public void DidFinishedTimer()
    {
        //bool isOpened = LXVRManager.Instance.OpenImpedancePanel(impedancePanelClosed);
        //Debug.Log("OpenImpedancePanel : " + isOpened);

        //Debug.Log("View = " + view);
        //Debug.Log("Condition = " + condition);
        //Debug.Log("trailCount = " + trailCount);

        if (!isPractice && view != Views.Blank)
        {
            WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "View: " + viewNameMap[view] + "; Condition: " + condition.ToString() + "; Finished");
            if (view == Views.Rating)
            {
                int rate = rateManager.GetComponent<RatePreference>().rate;
                WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "Rate for product " + trailCount.ToString() + " is " + rate.ToString());

            }
        }

        if (view == Views.Fixation)
        {
            LXVRManager.Instance.StopRecording(stopRecordingResultCallback);
            WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "Calibration Finished.");
            WriteString("-----------------------------------------------------");

            if (isPhase1)
            {
                WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "Phase 1 Start.");
                LXVRManager.Instance.StartRecording("Phase1", "Experiment", true, startRecordingResultCallback);
            }
            if (isPhase2)
            {
                WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "Phase 2 Start.");
                LXVRManager.Instance.StartRecording("Phase2", "Experiment", true, startRecordingResultCallback);
            }

            var curEvent = new Event
            {
                Condition = condition,
                View = view,
                ProductId = trailCount
            };
            string curEventDetail = JsonUtility.ToJson(curEvent);
            LXVRManager.Instance.RecordEvent(curEventDetail, recordEventResultCallback);


        }

        if (isPhase1)
        {
            // Stop the timer
            StopTimer();

            if (view == Views.Rating)
            {
                trailCount++;
            }

            if (view == Views.Blank)
            {
                view = Views.Description;
            }
            // when the last scene of phase 1 is finished, show the phaseChangeMsg
            else if (view == Views.Rating && totalTrail == trailCount)
            {
                isPhase1 = false;
                isBreaking = true;
                view = GetNextView(condition, view);
                Phase1EndMsg();
                LXVRManager.Instance.StopRecording(stopRecordingResultCallback);

                TimerTransfrom();
                StartTimer();
            }
            else
            {
                view = GetNextView(condition, view);
                Display(view);
                StartTimer();
                progressBar.Value = trailCount / (float)totalTrail;
                var curEvent = new Event
                {
                    Condition = condition,
                    View = view,
                    ProductId = trailCount
                };
                string curEventDetail = JsonUtility.ToJson(curEvent);
                LXVRManager.Instance.RecordEvent(curEventDetail, recordEventResultCallback);

            }
        }
        else if (isPhase2)
        {
            // Stop the timer
            StopTimer();

            // when the last scene of a product (Rating) is finished, trailCount++, switch into another condition
            if (view == Views.Rating)
            {
                trailCount++;
                if (trailCount == (int)(totalTrail/3)) condition = GetCondition(order, 1);
                if (trailCount == (int)(totalTrail/3*2)) condition = GetCondition(order, 2);
            }

            if (view == Views.Rating && totalTrail == trailCount)
            {
                isPhase1 = false;
                isPhase2 = false;
                view = GetNextView(condition, view);
                Phase2EndMsg();
                LXVRManager.Instance.StopRecording(stopRecordingResultCallback);
            }
            else
            {
                view = GetNextView(condition, view);
                Display(view);
                StartTimer();
                progressBar.Value = trailCount / (float)totalTrail;
                var curEvent = new Event
                {
                    Condition = condition,
                    View = view,
                    ProductId = trailCount
                };
                string curEventDetail = JsonUtility.ToJson(curEvent);
                LXVRManager.Instance.RecordEvent(curEventDetail, recordEventResultCallback);

            }
        }
        else if (isBreaking)
        {
            isBreaking = false;
            isPhase2 = true;
            BreakingEndMsg();
        }
        else if (isPractice)
        {
            Debug.Log("Timer finished");
            Debug.Log(view);


            if (view == Views.Description) view = Views.Rating;
            else if (view == Views.Rating) view = Views.Blank;
        }

        trailName = viewNameMap[view] + trailCount.ToString();
        //LXVRManager.Instance.StopRecording(stopRecordingResultCallback);
        //LXVRManager.Instance.StartRecording(sessionName, trailName, true, startRecordingResultCallback);

    }

    LXVRDelegate<LXVRResult> stopRecordingResultCallback = (LXVRResult result) =>
    {
        Debug.Log("StopRecording ResultCallback: " + result);
    };

    LXVRDelegate<LXVRResult> startRecordingResultCallback = (LXVRResult result) =>
    {
        Debug.Log("StartRecording ResultCallback: " + result);
    };

    LXVRDelegate<LXVRResult> recordEventResultCallback = (LXVRResult result) =>
    {
        Debug.Log("RecordEvent ResultCallback : " + result);
    };

    LXVRDelegate impedancePanelClosed = () =>
    {
        Debug.Log("impedancePanelClosed");
    };

    public void Phase1BeginMsg()
    {
        view = Views.Blank;
        Display(view);
        timer.duration = 3;
        praDes.SetActive(false);
        praRat.SetActive(false);
        isPractice = false;
        isPhase1 = true;
        phaseChange.SetActive(true);
        phaseChangeText.text = "Welcome to Phase 1! When you are ready, pull the trigger on the back of your controller to start!";
        StopTimer();
        StartTimer();
    }

    private void Phase1EndMsg()
    {
        infoBox.SetActive(false);
        distribution.SetActive(false);
        reviews.SetActive(false);
        rating.SetActive(false);

        phaseChange.SetActive(true);
        RectTransform pos = phaseChange.GetComponent<RectTransform>();
        pos.localPosition = new Vector3(80, 30, 0);
        phaseChangeText.text = "Phase 1 ended. Take a Break!";
        WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "End Phase 1");
        WriteString("-----------------------------------------------------");
    }

    private void BreakingEndMsg()
    {
        RectTransform pos = phaseChange.GetComponent<RectTransform>();
        pos.localPosition = new Vector3(10, -40, 0);
        timerObj.SetActive(false);
        phaseChangeText.text = "Time up! Please pull the trigger to start Phase 2";
        WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "End Breaking");
        WriteString("-----------------------------------------------------");
    }

    private void Phase2EndMsg()
    {
        infoBox.SetActive(false);
        distribution.SetActive(false);
        reviews.SetActive(false);
        rating.SetActive(false);

        phaseChange.SetActive(true);
        phaseChangeText.text = "Phase 2 ended. Thank you for your patience. Please wait for further instructions.";
        WriteString("[" + System.DateTime.Now + "]" + "[" + Time.time + "]" + "End Phase 2");
        WriteString("-----------------------------------------------------");
    }

    private void TimerTransfrom()
    {
        if (isBreaking)
        {
            timerTrans.localPosition = new Vector3(0, -40, 0);
            timerTrans.localScale = new Vector3(2, 2, 0.5f);
            timer.duration = breakingTime; // 120
            //timer.fillSettings.color = new Color(165/255, 233/255, 19/255);
        }
        else
        {
            timerTrans.localPosition = new Vector3(161, 132, 0);
            timerTrans.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            timer.duration = 3;
        }
    }


    // get the next condition, later it should be randomlized
    private Conditions GetCondition(ConditionOrders order, int index)
    {
        return conditionMap[order][index];
    }
    private Views GetNextView(Conditions condition, Views curView )
    {
        if (isPhase1)
        {
            if (curView == Views.Fixation) return Views.Description;
            else if (curView == Views.Description) return Views.Rating;
            else return Views.Description;
        }
        else if (isPhase2)
        {
            if (condition == Conditions.NoDisplay)
            {
                if (curView == Views.Description) return Views.Rating;
                else return Views.Description;
            }
            else if (condition == Conditions.SomeDisplay)
            {
                if (curView == Views.Description) return Views.Distribution;
                else if (curView == Views.Distribution) return Views.Rating;
                else return Views.Description;
            }
            else if (condition == Conditions.FullDisplay)
            {
                if (curView == Views.Description) return Views.Distribution;
                else if (curView == Views.Distribution) return Views.Review;
                else if (curView == Views.Review) return Views.Rating;
                else return Views.Description;
            }
        }
        return Views.Description;
    }

    private void Display(Views view)
    {
        //Debug.Log("Current View is " + view);
        if (view == Views.Description)
        {
            timer.duration = 3;
            jsonLoader.UpdateImage();
            jsonLoader.UpdateTitle();

            infoBox.SetActive(true);
            distribution.SetActive(false);
            reviews.SetActive(false);
            rating.SetActive(false);
            fix.SetActive(false);

        }
        else if (view == Views.Distribution)
        {
            timer.duration = 3;
            jsonLoader.UpdateDistribution();

            infoBox.SetActive(false);
            distribution.SetActive(true);
            reviews.SetActive(false);
            rating.SetActive(false);
        }
        else if (view == Views.Review)
        {
            timer.duration = 4;
            jsonLoader.UpdateDistribution();
            jsonLoader.UpdateReviews();

            infoBox.SetActive(false);
            distribution.SetActive(false);
            reviews.SetActive(true);
            rating.SetActive(false);
        }
        else if (view == Views.Rating)
        {
            timer.duration = 5;
            infoBox.SetActive(false);
            distribution.SetActive(false);
            reviews.SetActive(false);
            rating.SetActive(true);
        }
        else if (view == Views.Blank)
        {
            if (condition == Conditions.SomeDisplay) timer.duration = 4;
            else if (condition == Conditions.NoDisplay) timer.duration = 7;
            infoBox.SetActive(false);
            distribution.SetActive(false);
            reviews.SetActive(false);
            rating.SetActive(false);
        }
        else if (view == Views.Fixation)
        {
            timer.duration = 5;
            infoBox.SetActive(false);
            distribution.SetActive(false);
            reviews.SetActive(false);
            rating.SetActive(false);
            fix.SetActive(true);
        }
    }

    public void StartTimer()
    {
        timer.StartTimer();
    }

    public void PauseTimer()
    {
        timer.PauseTimer();
    }

    public void StopTimer()
    {
        timer.StopTimer();
    }

    [MenuItem("Tools/Write file")]
    static void WriteString(string message)
    {
        string path = "Assets/RatingLog.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(message);
        writer.Close();
    }

    private void InitOrderMap()
    {
        conditionMap = new Dictionary<ConditionOrders, Conditions[]>();
        conditionMap.Add(ConditionOrders.Order123, new Conditions[] { Conditions.NoDisplay, Conditions.SomeDisplay, Conditions.FullDisplay });
        conditionMap.Add(ConditionOrders.Order132, new Conditions[] { Conditions.NoDisplay, Conditions.FullDisplay, Conditions.SomeDisplay });
        conditionMap.Add(ConditionOrders.Order213, new Conditions[] { Conditions.SomeDisplay, Conditions.NoDisplay, Conditions.FullDisplay });
        conditionMap.Add(ConditionOrders.Order231, new Conditions[] { Conditions.SomeDisplay, Conditions.FullDisplay, Conditions.NoDisplay });
        conditionMap.Add(ConditionOrders.Order312, new Conditions[] { Conditions.FullDisplay, Conditions.NoDisplay, Conditions.SomeDisplay });
        conditionMap.Add(ConditionOrders.Order321, new Conditions[] { Conditions.FullDisplay, Conditions.SomeDisplay, Conditions.NoDisplay });
    }

    private void InitViewNameMap()
    {
        viewNameMap = new Dictionary<Views, string>();
        viewNameMap.Add(Views.Description, "Description");
        viewNameMap.Add(Views.Distribution, "Distribution");
        viewNameMap.Add(Views.Review, "Review");
        viewNameMap.Add(Views.Rating, "Rating");
        viewNameMap.Add(Views.Blank, "Blank");
        viewNameMap.Add(Views.Fixation, "Fixation");
    }

    private void InitViewShow()
    {
        // at the begining, only phaseChange msg is shown, everything else is not active
        phaseChange.SetActive(true);
        infoBox.SetActive(false);
        distribution.SetActive(false);
        reviews.SetActive(false);
        rating.SetActive(false);
        if (isPractice)
        {
            practice.SetActive(true);
            praDes.SetActive(false);
            praRat.SetActive(false);
        }

        // make sure our phaseChange msg is in the right place
        RectTransform pos = phaseChange.GetComponent<RectTransform>();
        pos.localPosition = new Vector3(10, -40, 0);

        // init views into Description
        view = Views.Description;

        // init condition into the first condition
        condition = GetCondition(order, 0);

        // init the progress bar into 0
        progressBar.Value = 0;

        // init the Msg we want to show in different phases
        if (isPractice)
        {
            phaseChangeText.text = "Welcome to our VR/EEG Project. Let's do some practice first! Pull the trigger on the back of your controller when you are ready!";
        }
        else if (isPhase1)
        {
            phaseChangeText.text = "Welcome to Phase 1! In this phase, you will see images and titles of 90 different products. You will be asked to give a rating for each of them. When you are ready, pull the trigger on the back of your controller to start!";
        }
        else if (isPhase2)
        {
            phaseChangeText.text = "Welcome to Phase 2! When you are ready, pull the trigger on the back of your controller to start!";
        }
    }

    private void InitLooxid()
    {
        lxvrManager = LXVRManager.Instance;
        lxvrManager.connectionStatusCallback = OnConnectStatusChanged;
        lxvrManager.eyeTrackerStatusCallback = OnEyeTrackerStatusChanged;
        lxvrManager.eegSensorStatusCallback = OnEEGSensorStatusChanged;
        lxvrManager.recordingStatusCallback = OnRecordingStatusChanged;
        //lxvrManager.StartBroadcasting();
    }

    public void OnConnectStatusChanged(ConnectionStatus connectionStatus)
    {
        Debug.Log("--> OnConnectStatusChanged : " + connectionStatus);
    }

    public void OnEyeTrackerStatusChanged(EyeTrackerStatus eyeTrackerStatus)
    {
        Debug.Log("--> OnEyeTrackerStatusChanged : " + eyeTrackerStatus);
    }

    public void OnEEGSensorStatusChanged(EEGSensorStatus eegSensorStatus)
    {
        Debug.Log("--> OnEEGSensorStatusChanged : " + eegSensorStatus);
    }

    public void OnRecordingStatusChanged(RecordingStatus recordingStatus)
    {
        Debug.Log("--> OnRecordingStatusChanged : " + recordingStatus);
    }



}
