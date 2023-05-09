using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner_script : MonoBehaviour
{
    public List<GameObject> game_objects;
    private List<float> spawn_delays = new List<float> {1.0f, 1.0f, 1.0f};
    private List<float> spawn_timers = new List<float> {0.0f, 0.0f, 0.0f};
    public float spawn_delay_max = 100.0f;
    
    void gen_new_spawn_delays(int i) {
        spawn_delays[i] = Random.Range(0.25f, spawn_delay_max);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < game_objects.Count; ++i) {
            obstacle_script script = game_objects[i].GetComponent<obstacle_script>();
            gen_new_spawn_delays(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < game_objects.Count; ++i) {
            spawn_timers[i] += Time.deltaTime;

            if (spawn_timers[i] >= spawn_delays[i]) {
                spawn_timers[i] = 0.0f;
                var new_obstacle = Instantiate(game_objects[i], 
                                               new Vector3(game_objects[i].transform.position.x,
                                                           game_objects[i].transform.position.y,
                                                           game_objects[i].transform.position.z), 
                                               Quaternion.Euler(270.0f, 0.0f, 0.0f));

                new_obstacle.GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, -1000.0f));
                new_obstacle.GetComponent<MeshCollider>().enabled = true;
                new_obstacle.GetComponent<Renderer>().enabled = true;
                new_obstacle.GetComponent<obstacle_script>().speed = 25.0f;
                gen_new_spawn_delays(i);
            }
        }
    }
}
