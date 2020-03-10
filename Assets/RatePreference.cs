using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class RatePreference : MonoBehaviour
{
    public Image[] starImages;
    public int rate;
    public GameObject rating;
    private TimeManager timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = GameObject.Find("TimeManager").GetComponentInChildren<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            rate++;
            if (rate > 5) rate = 5;
        }

        if (Input.GetKeyDown("down"))
        {
            rate--;
            if (rate < 1) rate = 1;
        }

        for (int i = 0; i < rate; i++)
        {
            starImages[i].enabled = true;
        }

        for (int i = rate; i < 5; i++)
        {
            starImages[i].enabled = false;
        }

        if (!rating.activeSelf) ResetRating();

    }

    public void ResetRating()
    {
        if (rate != 0)
        {
            string strRate = rate.ToString();
        }

        rate = 0;
        for (int i = 0; i < 5; i++)
        {
            starImages[i].enabled = false;
        }

    }
}
