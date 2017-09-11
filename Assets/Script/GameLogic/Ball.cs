using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

    public enum BallStatus
    {
        INIT,
        INSIDE_RUNNING,
        OUTSIDE_RUNNING,
        IN_LINK,
        IN_LINK_RESET,
        CHANGE_SIDE, 
        CHANGE_CIRCLE,
        DEAD,
        SUCCESS,
        RESET
    };

    public GameObject center_point;

    public float line_speed;
    float line_speed_backup;

    public Camera main_camera;

    //private float radius;
    private float radius_inside;
    private float radius_outside;

    private float radius;

    private Link link;

    private BallStatus ball_status;

    //private List<GameObject> circles = new List<GameObject>();

    private GameObject reset_center_go;
    //private GameObject first_center_go;

    //private float smallball_interval = 0.05f;
    //private float smallball_timer = 0f;

    private MeshRenderer mr;

    float camera_z;

    private float angle_speed;

    private int god_count = 0; //-1 god 0 normal 1 one time god 

    private bool is_camera_follow_ball = false;

    GameStage gs;
    ClickScreen cs;

    private List<int> timerList = new List<int>();

    AudioSource audio;

    public GameObject follow;

    public AudioClipSet audioclip_set;

    private List<int> save_list = new List<int>();

    int trigger_timer = 0;

    public ChangeAvatar ca;

    BoxCollider box;

    public ParticleSystem save_particle_system;

    Quaternion quaternion_backup;

	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
        
        box = GetComponent<BoxCollider>();

        quaternion_backup = transform.rotation;

        GameObject go_audioclip = GameObject.Find("AudioClipSet");

        if (go_audioclip) {
            audioclip_set = go_audioclip.GetComponent<AudioClipSet>();
        }

        ChangeStatus(BallStatus.INIT);
	}
	
	// Update is called once per frame
	void Update () {
        if (ball_status != BallStatus.DEAD)
        {
            transform.RotateAround(center_point.transform.position, Vector3.forward, angle_speed * Time.deltaTime);

            /*
            smallball_timer += Time.deltaTime;
            if (smallball_timer > smallball_interval) {
                MakeSmallBall();

                smallball_timer = 0f;
            }
            */

            if (is_camera_follow_ball)
            {
                MoveCamera(follow , 0f);
            }
        }
	}

    void MakeSmallBall() {
        GameObject go_smallball = GameObjectPool.instance.Spawn();

        go_smallball.transform.position = gameObject.transform.position;
    }

    void ChangeStatus(BallStatus status) {


        switch(status){
            case BallStatus.INIT:
                gs = GameObject.Find("GameStage").GetComponent<GameStage>();
                cs = GameObject.Find("Main Camera").GetComponent<ClickScreen>();

                trigger_timer = 0;

                SetFirstCenterPoint(center_point);
                MoveCamera(center_point);
                //circles.Add(center_point);

                //reset_go = center_point;

                ball_status = BallStatus.INSIDE_RUNNING;
                break;
            case BallStatus.CHANGE_SIDE:

                if (ball_status == BallStatus.INSIDE_RUNNING)
                {
                    AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.game_tap);

                    SetRadius(radius_outside);

                    ball_status = BallStatus.OUTSIDE_RUNNING;
                }
                else if (ball_status == BallStatus.OUTSIDE_RUNNING)
                {
                    AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.game_tap);

                    SetRadius(radius_inside);

                    ball_status = BallStatus.INSIDE_RUNNING;
                }

                break;
            case BallStatus.CHANGE_CIRCLE:

                AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_into_circle);

                if (ball_status == BallStatus.IN_LINK)
                {
                    SetCenterPoint(link.next_center_point);
                }else{
                    SetCenterPoint(link.previous_center_point);
                }

                ball_status = BallStatus.INSIDE_RUNNING;

                //circles.Add(link.next_center_point);
                break;
            case BallStatus.DEAD:

                
                if (gs != null)
                {
                    gs.life_num--;

                    if (gs.life_num <= 0)
                    {
                        Debug.Log("DEAD");

                        ball_status = BallStatus.DEAD;

                        GameObjectPool.instance.RecycleAll();
                        SetFirstCenterPoint(reset_center_go);

                        Event_GameResult event_star = new Event_GameResult();
                        event_star.result = false;
                        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.GAME_RESULT](event_star);
 //                       gs.Reset();
 //                       ChangeStatus(BallStatus.RESET);
                    }
                    else {
                        //ChangeStatus(BallStatus.RESET);

                        AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_dead);

                        Event_ChangeLifeNum event_life = new Event_ChangeLifeNum();
                        event_life.life_num = gs.life_num;
                        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.CHANGE_LIFE_NUM](event_life);

                        ca.HideBody();
                        ca.Explode();
                        box.enabled = false;

                        Invoke("Reset" , 1f);

                        //ChangeStatus(BallStatus.RESET);
                    }
                }
                
                break;

            case BallStatus.SUCCESS:
                //GameStage gs = transform.parent.GetComponent<GameStage>();

                //Debug.Log("SUCCESS");
                cs.no_click = true;

                

                if(SceneManager.GetActiveScene().name.Equals("Stage_Newbies")){
                    bool b = PlayerData.GetInstance().GetNewbieDone();
                    if (b)
                    {
                        SceneManager.LoadScene("Main");
                    }
                    else {
                        PlayerData.GetInstance().SetNewbieDone(true);
                        SceneManager.LoadScene("SelectLevels");
                    }
                }
                else
                {
                    Event_GameResult event_star = new Event_GameResult();
                    event_star.result = true;
                    CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.GAME_RESULT](event_star);
                }
                //gs.Reset();
                //ChangeStatus(BallStatus.RESET);

                break;

            case BallStatus.RESET:
                GameObjectPool.instance.RecycleAll();

                trigger_timer = 0;

                ball_status = BallStatus.INSIDE_RUNNING;


                ResetStatus();

                foreach (int id in timerList) {
                    TimerManager.GetInstance().StopTimer(id);
                }

                timerList.Clear();

                //TimerManager.GetInstance().OnDestroy();
                SetFirstCenterPoint(reset_center_go);
                MoveCamera(reset_center_go);

                ca.ShowBody();
                //ca.Explode();
                box.enabled = true;

                break;
        }
    }

    void Reset() {
        ChangeStatus(BallStatus.RESET);
    }

    void SetRadius(float _radius) {
        
        Vector3 dir = (transform.position - center_point.transform.position).normalized;
        transform.position = center_point.transform.position + (dir * _radius);

        if (follow != null)
        {
            Vector3 follow_dir = (follow.transform.position - center_point.transform.position).normalized;
            follow.transform.position = center_point.transform.position + (dir * radius);
        }
        
    }

    public void OnResurgence() {
        Event_ChangeLifeNum event_life = new Event_ChangeLifeNum();
        event_life.life_num = gs.life_num = CommonData.life_num;
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.CHANGE_LIFE_NUM](event_life);

        ChangeStatus(BallStatus.RESET);


        Event_ResetObstacleBuff reset = new Event_ResetObstacleBuff();
        CustomEventSystem.GetInstance().custom_event_delegate[(int)CUSTOM_EVENT_TYPE.RESET_OBSTACLE_BUFF](reset);
        
    }

    public void Change() {
        if (ball_status == BallStatus.IN_LINK || ball_status == BallStatus.IN_LINK_RESET)
        {
            ChangeStatus(BallStatus.CHANGE_CIRCLE);
        }
        /*else if (ball_status == BallStatus.IN_LINK_RESET)
        {
            ChangeStatus(BallStatus.RESET);
        }*/
        else {
            ChangeStatus(BallStatus.CHANGE_SIDE);
        }
    }

    public void SetCenterPoint(GameObject center) {

        //bool is_plus = false;

        //if (angle_speed > 0)
        //{
        //    is_plus = true;
        //}

        /*angle_speed = */
        CIRCLE_TYPE type = Init(center);

        //if (is_plus)
        {
            angle_speed = -angle_speed;
        }

        /////////////////////////////////////////////////////////////////
        //if(angle_speed >= 0){
        //    this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        //}else{
        //    this.transform.eulerAngles = new Vector3(0f, 0f, 180f);
        //}

        //this.transform.LookAt(center.transform);

        //Debug.Break();

        float x = transform.position.x - center.transform.position.x;
        float y = transform.position.y - center.transform.position.y;


        if (x >= 0 && y >= 0)
        {               //+
            if (angle_speed >= 0)
            {
                this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }
        }
        else if (x >= 0 && y <= 0)          //-
        {
            if (angle_speed >= 0)
            {
                this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }

            //angle = -angle;
        }
        else if (x <= 0 && y >= 0)          //-
        {
            if (angle_speed >= 0)
            {
                this.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }

            //angle = -angle;
        }
        else if (x <= 0 && y <= 0)          //+
        {
            if (angle_speed >= 0)
            {
                this.transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }
            else
            {
                this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
        }

        //Debug.Break();

        //return;

        Vector3 up = this.transform.up;
        Vector3 ball_center = new Vector3(center.transform.position.x - this.transform.position.x, center.transform.position.y - this.transform.position.y, center.transform.position.z - this.transform.position.z);

        Debug.DrawRay(transform.position,up,Color.yellow,100f);
        Debug.DrawRay(transform.position,ball_center, Color.yellow,100f);

        //Debug.Break();

        float angle = Vector2.Angle(up, ball_center);

        //Debug.Log("Angle : " +  angle.ToString());

        if (angle >= 90) {
            angle -= 90;
        }else if(angle < 90){
            angle = 90 - angle;
        }

        //float x = transform.position.x - center.transform.position.x;
        //float y = transform.position.y - center.transform.position.y;


        if(x >= 0 && y >= 0){               //+

        }
        else if (x >= 0 && y <= 0)          //-
        {

            angle = -angle;
        }
        else if (x <= 0 && y >= 0)          //-
        {

            angle = -angle;
        }
        else if (x <= 0 && y <= 0)          //+
        {

        }

        transform.Rotate(Vector3.forward, angle, Space.World);

        //Debug.Break();
        ////////////////////////////////////////////////////////////////

        SetRadius(radius_inside);

        is_camera_follow_ball = false;

        if (type == CIRCLE_TYPE.CIRCLE_TYPE_SAVE)
        {
            AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_savepoint);

            reset_center_go = center;
            MoveCamera(center);
        }
        else if (type == CIRCLE_TYPE.CIRCLE_TYPE_FOLLOW_BALL)
        {
            is_camera_follow_ball = true;
        }
        else if (type == CIRCLE_TYPE.CIRCLE_TYPE_END)
        {

            cs.no_click = true;
            AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_finish);

            MoveCamera(center);
           // ChangeStatus(BallStatus.SUCCESS);

            

        }
        else if (type == CIRCLE_TYPE.CIRCLE_TYPE_NORMAL)
        {
            MoveCamera(center);
        }
    }

    public void SetFirstCenterPoint(GameObject center)
    {
        camera_z = main_camera.transform.position.z;

        main_camera.transform.position = new Vector3(center_point.transform.position.x, center_point.transform.position.y,camera_z);

        /*angle_speed = */
        Init(center);

        angle_speed = angle_speed < 0 ? -angle_speed : angle_speed;

        transform.position = new Vector3(center_point.transform.position.x, center_point.transform.position.y + radius_inside, center_point.transform.position.z);

        transform.rotation = quaternion_backup;

        reset_center_go = /*first_center_go= */center;

        //MoveCamera(center);

        is_camera_follow_ball = false;
    }

    private /*float*/ CIRCLE_TYPE Init(GameObject center)
    {
        center_point = center;

        Circle c = center.GetComponent<Circle>();

        if (c.circle_id > gs.save_done)
            gs.save_done = c.circle_id;

        radius_inside = c.radius - CommonData.RADIUS_INSIDE;
        radius_outside = c.radius + CommonData.RADIUS_OUTSIDE;

        radius = c.radius;

        if(mr != null){
            mr.enabled = false;
        }

        mr = c.mr;

        if (mr != null)
        {
            mr.enabled = true;
        }

        /*return angle_speed = */
        SetSpeed(line_speed, c.radius);

        if(c.type == CIRCLE_TYPE.CIRCLE_TYPE_SAVE){
            if(!save_list.Contains(c.circle_id)){
                save_list.Add(c.circle_id);

                gs.save_done = save_list.Count;
            }

            if (save_particle_system != null)
            {
                save_particle_system.gameObject.SetActive(false);
            }

            if (c.particle_system != null)
            {
                save_particle_system = c.particle_system;

                c.particle_system.gameObject.SetActive(true);
            }
        }
        else if (c.type == CIRCLE_TYPE.CIRCLE_TYPE_END)
        {
            if (c.particle_system != null)
            {
                c.particle_system.gameObject.SetActive(true);
            }
        }

        return c.type;
    }

    void SetSpeed(float line_speed ,float radius) {

        float f = line_speed / radius;

        angle_speed = angle_speed >= 0 ? f:-f;
    }

    void OnTriggerEnter(Collider collider){

        if (collider == null || collider.gameObject == null)
        {
            return;
        }

        //Debug.Log("Enter : " + collider.gameObject.name);

        GameObject go = collider.gameObject;
        string tag = go.tag;

        if (tag.Equals("Link"))
        {
            trigger_timer++;

            link = go.GetComponent<Link>();

            if(ball_status == BallStatus.INSIDE_RUNNING){
                if (link.next_center_point != center_point)
                {
                    ball_status = BallStatus.IN_LINK;
                }
                else {
                    ball_status = BallStatus.IN_LINK_RESET;
                }
            }
            else if (ball_status == BallStatus.OUTSIDE_RUNNING)
            {
                ChangeStatus(BallStatus.DEAD);
            }
        }

        if (tag.Equals("Obstacle"))
        {
            AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_break_obstacle);

            if (god_count > 0)
            {
                god_count--;
            }
            else
            {
                ChangeStatus(BallStatus.DEAD);
            }
        }

        if (tag.Equals("Buff"))
        {
            AddBuff(go);
        }
    }

    int success_num = 0;

    void AddBuff(GameObject go) {
        Buff buff = go.GetComponent<Buff>();

        if(buff == null){
            return;
        }
 
        switch(buff.buff_type){
            case BUFF_TYPE.BUFF_TYPE_SHIELD:

                AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_shield);

                god_count = 1;

                break;
            case BUFF_TYPE.BUFF_TYPE_GOLD:

                PlayerData.GetInstance().SetGold(buff.gold_num);

                AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_gold);

                break;
            case BUFF_TYPE.BUFF_TYPE_SPEED_UP:

                AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_speedup);

                line_speed_backup = line_speed;
                line_speed *= buff.speed_multiple;

                if (ball_status == BallStatus.INSIDE_RUNNING)
                {
                    /*angle_speed = */SetSpeed(line_speed, radius_inside);
                }
                else if (ball_status == BallStatus.OUTSIDE_RUNNING)
                {
                    /*angle_speed = */SetSpeed(line_speed, radius_outside);
                }

                int id_speed = 0;

                TimerManager.GetInstance().CreateTimer(
                    out id_speed,
                    buff.valid_time,
                    ResetSpeed,//Action<object> callback, 
                    null, 
                    TimerType.UnityTime,
                    false
                ).Start();

                timerList.Add(id_speed);

                break;
            case BUFF_TYPE.BUFF_TYPE_LIGHTNING:

                AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_lightning);

                god_count = 10000;

                int id_gold = 0;

                TimerManager.GetInstance().CreateTimer(
                    out id_gold,
                    buff.valid_time,
                    ResetGodCount,//Action<object> callback, 
                    null,
                    TimerType.UnityTime,
                    false
                ).Start();

                timerList.Add(id_gold);

                break;
            case BUFF_TYPE.BUFF_TYPE_OUTSIDE:

                AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_in_out_and_return);

                if(ball_status == BallStatus.INSIDE_RUNNING){
                    //ChangeStatus(BallStatus.CHANGE_CIRCLE);

                    ChangeStatus(BallStatus.CHANGE_SIDE);
                }

                break;
            case BUFF_TYPE.BUFF_TYPE_INSIDE:

                AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_in_out_and_return);

                if (ball_status == BallStatus.OUTSIDE_RUNNING)
                {
                    //ChangeStatus(BallStatus.CHANGE_CIRCLE);

                    ChangeStatus(BallStatus.CHANGE_SIDE);
                }

                break;
            case BUFF_TYPE.BUFF_TYPE_REVERSE:

                AudioSourcesManager.GetInstance().Play(audio, (audioclip_set == null) ? null : audioclip_set.a_game_in_out_and_return);

                angle_speed = -angle_speed;

                break;

            case BUFF_TYPE.BUFF_TYPE_SUCCESS:

                success_num++;


                if(success_num == 2){
                    ChangeStatus(BallStatus.SUCCESS);
                }
                

                break;
        }

        //GameObject.Destroy(go);
    }

    void ResetSpeed(object o){
        if (line_speed_backup == 0)
        {
            return;
        }

        line_speed  = line_speed_backup;

        line_speed_backup = 0;

        if (ball_status == BallStatus.INSIDE_RUNNING || ball_status == BallStatus.IN_LINK || ball_status == BallStatus.IN_LINK_RESET)
        {
            /*angle_speed = */
            SetSpeed(line_speed, radius_inside);
        }
        else if (ball_status == BallStatus.OUTSIDE_RUNNING)
        {
            /*angle_speed = */
            SetSpeed(line_speed, radius_outside);
        }
    }

    void ResetGodCount(object o)
    {
        god_count = 0;
    }

    void ResetStatus() {
        ResetGodCount(null);
        ResetSpeed(null);
    }

    void OnTriggerExit(Collider collider){

        //Debug.Log("Exit : " + collider.gameObject.name);

        if (collider.gameObject.tag.Equals("Link"))
        {
            trigger_timer--;

            if (trigger_timer > 0)
                return;

            if (ball_status == BallStatus.IN_LINK || ball_status == BallStatus.IN_LINK_RESET)
            {
                ball_status = BallStatus.INSIDE_RUNNING;
                link = null;
            }
        }

    }
    
    void MoveCamera(GameObject target , float d = 1f) {
        //Debug.Log(target.name);

        Vector3 v_target = new Vector3(target.transform.position.x , target.transform.position.y,camera_z);

        main_camera.transform.DOMove(v_target,d);
    }

}