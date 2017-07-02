using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour {

    private int day;
    private float secondsCount = 0;
    private float minuteCount;
    private int hourCount;

    public Text timeText;
    public Text dayText;

    // Use this for initialization
    void Start () {
        day = PlayerPrefs.GetInt("day", 1);
        minuteCount = PlayerPrefs.GetFloat("minute", 0);
        hourCount = PlayerPrefs.GetInt("hour", 6);
        dayText.text = "day: "+day.ToString();
        timeText.text = hourCount + ":" + ((int)minuteCount);
    }

    // Update is called once per frame
    void Update() {
        minuteCount += Time.deltaTime * 5;

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
        
       
    }

    public void skipTime(int _time) {

    }
}
