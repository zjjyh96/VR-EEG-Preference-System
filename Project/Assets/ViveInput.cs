using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class ViveInput : MonoBehaviour
{
    public SteamVR_Action_Single squeezeAction;
    public SteamVR_Action_Vector2 touchPadAction;

    public GameObject infoBox;
    public GameObject phaseChange;

    private Text phaseChangeText;
    private RatePreference rate;
    private TimeManager timer;

    // Start is called before the first frame update
    void Start()
    {
        rate = GameObject.Find("RatingManager").GetComponent<RatePreference>();
        timer = GameObject.Find("TimeManager").GetComponent<TimeManager>();
        phaseChangeText = phaseChange.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = squeezeAction.GetAxis(SteamVR_Input_Sources.Any);

        if (timer.isPractice && triggerValue > 0.99f)
        {
            phaseChange.SetActive(false);
            if (timer.view == TimeManager.Views.Description)
            {
                timer.PracticeDescription();
            }
            if (timer.view == TimeManager.Views.Rating)
            {
                timer.PracticeRating();
            }
            if (timer.view == TimeManager.Views.Blank)
            {
                timer.Phase1BeginMsg();
            }
        }
        
        if (phaseChange.activeSelf && triggerValue > 0.99f && (timer.isPhase1 || timer.isPhase2) && timer.view != TimeManager.Views.Blank)
        {
            phaseChange.SetActive(false);
            timer.StartPhase();
        }

        if (!timer.isPractice && timer.rating.activeSelf && triggerValue > 0.99f)
        {
            timer.DidFinishedTimer();
        }

        Vector2 touchPadValue = touchPadAction.GetAxis(SteamVR_Input_Sources.Any);
        if(touchPadValue != Vector2.zero)
        {
            double value = touchPadValue.x + 1.0;
            rate.rate = (int)(value / 0.4) + 1;
        }
    }
}
