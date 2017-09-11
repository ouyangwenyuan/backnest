using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringUtil {
    public static string TimeFormat(float time) {
        string result = null;

        int iTime = (int)time;

        float ms = time - (float)iTime;

        int minute = iTime / 60;
        int second = iTime % 60;

        if (minute > 0) {
            result += minute.ToString() + ":";
        }

        result += ((float)second + ms).ToString("f2");

        return result;
    }
}
