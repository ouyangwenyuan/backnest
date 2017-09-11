using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUtil //: MonoBehaviour 
{
    public static string ShowTime(long t) {
        string time = "";

        int hour = (int)(t / 3600);

        if (hour < 10)
        {
            time += "0";
        }

        time += hour.ToString() + " : ";

        int minute = (int)((t - hour * 3600)/ 60);

        if (minute < 10)
        {
            time += "0";
        }

        time += minute.ToString() + " : ";

        int second = (int)((t - hour * 3600)  % 60);

        if (second < 10)
        {
            time += "0";
        }

        time += second.ToString();

        return time;
    }
}
