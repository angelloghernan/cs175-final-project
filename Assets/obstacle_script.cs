using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle_script : MonoBehaviour
{

    public GameObject player;
    public float speed = 0.0f;
    private int instance_id = 0;
    private float timer = 0.0f;
    private float time_to_reach_end = 0.0f;
    private Vector3 start_point;
    // Start is called before the first frame update
    void Start()
    {
        if (instance_id == 0) {
            instance_id = GetInstanceID();
        } else if (GetInstanceID() != instance_id) {
            speed = 12.0f;
        }
        start_point = transform.position;
        time_to_reach_end = Mathf.Abs(player.transform.position.z - 20.0f - start_point.z) / speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= time_to_reach_end) {
            Destroy(gameObject);
            return;
        }

        float t = timer / time_to_reach_end;
        transform.position = Vector3.Lerp(start_point, 
                                          new Vector3(start_point.x, 
                                                      start_point.y, 
                                                      player.transform.position.z - 20.0f), 
                                          t);
    }
}
