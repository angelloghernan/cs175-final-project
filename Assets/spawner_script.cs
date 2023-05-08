using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner_script : MonoBehaviour
{
    public GameObject game_object;
    public float spawn_delay = 1.0f;
    private float spawn_timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        spawn_timer += Time.deltaTime;

        if (spawn_timer >= spawn_delay) {
            spawn_timer = 0.0f;
            var new_obstacle = Instantiate(game_object, 
                                           new Vector3(game_object.transform.position.x,
                                                       game_object.transform.position.y,
                                                       game_object.transform.position.z), 
                                           Quaternion.identity);

            new_obstacle.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, -1000.0f));
        }
    }
}
