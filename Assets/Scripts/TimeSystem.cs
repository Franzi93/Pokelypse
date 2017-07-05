using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour {

    private int day;

    [Range(0,60)]
    private float minuteCount;
    [Range(0, 24)]
    public int hourCount;

    private bool dayLightOn;

    public bool timeRunning = true;
    public Light sunLight;
    public Text timeText;
    public Text dayText;
    public Color dayColor;
    public Color nightColor;
    public int dayStartHour;
    public int dayEndHour;
    public float switchSpeed = 5;

    private Color toChange;
    private float t = 0;

    // Use this for initialization
    void Start () {
        day = PlayerPrefs.GetInt("day", 1);
        minuteCount = PlayerPrefs.GetFloat("minute", 0);
        hourCount = PlayerPrefs.GetInt("hour", 6);
        dayText.text = "day: "+day.ToString();
        timeText.text = hourCount + ":" + ((int)minuteCount);
        
        toChange = isDay()?dayColor:nightColor;
        dayLightOn = !isDay();
    }

    private bool isDay()
    {
        return (hourCount < dayEndHour) && (hourCount >= dayStartHour);
    }

    // Update is called once per frame
    void Update() {
        if (timeRunning) {
            minuteCount += Time.deltaTime * 5;
        }

        if (((int)minuteCount) % 10 == 0)
        {
            timeText.text = hourCount + ":" + ((int)minuteCount);
        }
        if (minuteCount >= 60)
        {
            hourCount++;
            minuteCount = 0;
        }
        if (hourCount >= 24) {
            hourCount = 0;
            ++day;
            dayText.text = "day: "+ day.ToString();
        }
        
        if (isDay() !=  dayLightOn)
        {
            toChange = isDay()?dayColor: nightColor;
            t = 0;
            dayLightOn = !dayLightOn;
        }

        sunLight.color = Color.Lerp(sunLight.color,toChange, t);
        if (t < .1) {
            t += Time.deltaTime / switchSpeed;
        }
    }

    public void skipTime(int _minutes,int _hours) {
        hourCount += _hours;
        minuteCount += _minutes;
    }
}
