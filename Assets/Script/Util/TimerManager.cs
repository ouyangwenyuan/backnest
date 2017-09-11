using System;
using System.Collections.Generic;
using UnityEngine;

//namespace UnityEngine.GameFramework
//{
    public enum TimerType
    {
        /// <summary>
        /// 受时间缩放影响
        /// </summary>
        UnityTime,
        /// <summary>
        /// 不受时间缩放影响
        /// </summary>
        RealTime
    }

    /// <summary>
    /// 定时器类
    /// </summary>
    public class TimerManager
    {
        static TimerManager time_manager;

        static int index = 0;

        public static TimerManager GetInstance() {
            if (time_manager == null)
            {
                time_manager = new TimerManager();
            }

            return time_manager;
        }

        private Dictionary<int, Timer> timerDic = new Dictionary<int, Timer>();//有效的计时器
        //private List<Timer> timerList = new List<Timer>();                     //失效的计时器，准备清楚
        private List<int> timerList = new List<int>();

        public void OnUpdate()
        {
            /*
            int count = timerList.Count;
            if (count <= 0) return;

            for (int i = count - 1; i >= 0; i--)
            {
                Timer time = timerList[i];

                bool delete = time.Update();
                if (delete)
                {
                    timerList.RemoveAt(i);
                    time = null;
                }
            }
            */

            foreach (KeyValuePair<int, Timer> kv in timerDic)
            {
                bool delete = kv.Value.Update();

                if (delete)
                {
                    timerList.Add(kv.Key);
                }
            }

            foreach(int key in timerList){
                if (timerDic.ContainsKey(key)) {
                    Timer timer = timerDic[key];
                    timerDic.Remove(key);

                    timer = null;
                }
            }

            timerList.Clear();
        }

        public void OnDestroy()
        {
            timerDic.Clear();
        }

        public Timer CreateTimer(
            out int id,
            float time, 
            Action<object> callback, 
            object customParam = null, 
            TimerType timeType = TimerType.UnityTime,
            bool isLoop = false , bool in_turn = false, float time2 = 0 ,float time3 = 0)
        {
            Timer timer = new Timer(this, time, callback, customParam, timeType, isLoop , in_turn , time2 , time3);

            id = timer.index = index++;

            timerDic.Add(timer.index , timer);
            return timer;
        }

        /*
        public void CreateTimer(Timer timer)
        {
            if (!timerList.Contains(timer))
            {
                timerList.Add(timer);
            }
        }
        */

        public void StopTimer(int index) {
            if (timerDic.ContainsKey(index))
            {
                Timer timer = timerDic[index];
                timer.Stop();
            }
        }

        public class Timer
        {
            private enum TimerStatus
            {
                UnStart,
                Running,
                Paused,
                WaitForRemove,
            }

            public int index = 0;

            private float time1 = 0;
            private Action<object> callback = null;
            private object customParam = null;
            private TimerType timeType = TimerType.UnityTime;
            private bool isLoop = false;

            private float lastTime = 0;
            private TimerStatus status = TimerStatus.UnStart;
            //private TimerManager manager = null;

            private bool time_in_true = false;
            private bool time_in_true_first = false;
            private float time2 = 0;
            private float time3 = 0;

            private Timer() { }

            public Timer(TimerManager manager, float time1, Action<object> callback, object customParam = null, TimerType timeType = TimerType.UnityTime, bool isLoop = false,bool in_true = false, float time2 = 0 , float time3 = 0)
            {
                //this.manager = manager;
                this.time1 = time1;
                this.callback = callback;
                this.customParam = customParam;
                this.timeType = timeType;
                this.isLoop = isLoop;

                if (in_true) {
                    this.time_in_true = true;
                    this.time_in_true_first = true;
                    this.time2 = time2;
                    this.time3 = time3;

                    this.time1 = this.time2;
                }
            }

            public bool Update()
            {
                if (status == TimerStatus.WaitForRemove) return true;
                if (status != TimerStatus.Running) return false;

                switch (timeType)
                {
                    case TimerType.UnityTime:
                        lastTime += Time.deltaTime;
                        break;
                    case TimerType.RealTime:
                        lastTime += Time.unscaledDeltaTime;
                        break;
                    default:
                        break;
                }

                // 时间到了
                if (lastTime >= time1)
                {
                    if (isLoop)

                        if (time_in_true)
                        {
                            lastTime = 0;

                            time_in_true_first = !time_in_true_first;
                            time1 = time_in_true_first ? time2 : time3;

                        }else{
                            lastTime %= time1;
                        }

                    else
                        status = TimerStatus.WaitForRemove;

                    if (callback != null)
                        callback(customParam);
                }

                return status == TimerStatus.WaitForRemove;
            }

            public void Start()
            {
                status = TimerStatus.Running;
            }

            /// <summary>
            /// 暂停
            /// </summary>
            public void Pause()
            {
                status = TimerStatus.Paused;
            }

            /// <summary>
            /// 删除
            /// </summary>
            public void Stop()
            {
                status = TimerStatus.WaitForRemove;
            }

            /*
            /// <summary>
            /// 重置时间
            /// </summary> 
            public void Reset()
            {
                if (status == TimerStatus.WaitForRemove)
                {
                    // 已经删除或者正准备删除
                    manager.CreateTimer(this);
                }
                lastTime = 0;
                status = TimerStatus.Running;
            }
            */
            
            /*
            public override string ToString()
            {
                return time - lastTime + "s loop:" + isLoop +
                    " : " + callback.Target.GetType().Name + "." + callback.Method.Name;
            }
            */
        }
    }
//}