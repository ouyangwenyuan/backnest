using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonData
{
    public const float RADIUS_INSIDE = 0.25f;
    public const float RADIUS_OUTSIDE = 0.25f;

    public const int life_num = 4;
    public const int resurgence_num = 1000;

    public const float gold_fly_time = 1f;

    private static int[] gift_time = new int[] {300,600,1200,3600,10800,21600};
    private static int[] gift_min = new int[] {500,300,100,100,100,100};
    private static int[] gift_max = new int[] {500,300,300,300,300,300};

    public static void GetGiftTime(int id, out int time, out int gold)
    {
        time = 0;
        gold = 0;

        time = gift_time[id -1];

        int ran = Random.Range(1, 3);

        if(ran == 1){
            gold = gift_min[id -1];
        }else if(ran == 2){
            gold = gift_max[id - 1];
        }
    }

}
