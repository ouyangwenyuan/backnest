using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathExtend{
    public static float CalculateCircumference(float raduis)
    {
        return 2 * Mathf.PI * raduis;
    }

    //angle speed line speed
    public static float CalculateAngleSpeed(float raduis , float line_speed)
    {
        return line_speed / raduis;
    }
}
