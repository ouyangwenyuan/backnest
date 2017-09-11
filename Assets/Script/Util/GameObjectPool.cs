using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {

    public int size;

    public static GameObjectPool instance;

    int count = 0;
    List<GameObject> list_go = new List<GameObject>();
    Dictionary<GameObject, GameObject> pool = new Dictionary<GameObject, GameObject>();

    void Awake() {
        instance = this;

 //       Object smallball;
 //       Transform trans;
        GameObject go_smallball;

        for (int i = 0; i < size; i++ )
        {
            go_smallball = CreateItem();

            go_smallball.SetActive(false);

            list_go.Add(go_smallball);
        }
    }

    GameObject CreateItem() {
        count++;

        Object smallball = Resources.Load("Prefabs/SmallBall", typeof(GameObject));
        GameObject go_smallball = Instantiate(smallball) as GameObject;

        go_smallball.name = count.ToString();

        Transform trans;
        trans = go_smallball.transform;
        trans.parent = transform;
        trans.position = transform.position;
        trans.rotation = transform.rotation;

        //SmallBall ball = go_smallball.GetComponent<SmallBall>();

        //go_smallball.SetActive(false);

        return go_smallball;
    }

    public GameObject Spawn() {
        GameObject go = null;

        if (list_go.Count > 0)
        {
            go = list_go[0];
            list_go.RemoveAt(0);
        }
        else {
            go = CreateItem();
        }

        go.SetActive(true);

        pool.Add(go, gameObject);

        return go;
    }

    public void Recycle(GameObject go) {

        //Debug.Log(go.name);

        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;

        go.SetActive(false);

        pool.Remove(go);
        list_go.Add(go);
    }


    public void RecycleAll() {
        foreach(KeyValuePair<GameObject,GameObject> go in pool){
            go.Key.transform.position = transform.position;
            go.Key.transform.rotation = transform.rotation;

            go.Key.SetActive(false);

            //pool.Remove(go);
            list_go.Add(go.Key);
        }

        pool.Clear();
    }


	// Use this for initialization
	//void Start () {
		
	//}
	
	// Update is called once per frame
	//void Update () {
		
	//}
}
