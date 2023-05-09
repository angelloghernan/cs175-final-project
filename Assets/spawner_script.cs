using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner_script : MonoBehaviour
{
    public List<GameObject> game_objects;
    public List<GameObject> other_objects;
    private List<float> spawn_delays = new List<float> {1.0f, 1.0f, 1.0f};
    private List<float> spawn_timers = new List<float> {0.0f, 0.0f, 0.0f};
    private List<Material> object_materials = new List<Material>();
    public float spawn_delay_max = 100.0f;
    private float x_curve = 0.0f;
    private float y_curve = 0.0f;
    public float x_curve_delta = 0.00001f;
    public float y_curve_delta = 0.00001f;
    private int curve_frame = 0;
    
    void gen_new_spawn_delays(int i) {
        spawn_delays[i] = Random.Range(0.25f, spawn_delay_max);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < game_objects.Count; ++i) {
            obstacle_script script = game_objects[i].GetComponent<obstacle_script>();
            gen_new_spawn_delays(i);
            object_materials.Add(game_objects[i].GetComponent<Renderer>().material);
        }

        for (var i = 0; i < other_objects.Count; ++i) {
            object_materials.Add(other_objects[i].GetComponent<Renderer>().material);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ++curve_frame;
        if (curve_frame > 20) {
            curve_frame = 0;
            x_curve += x_curve_delta;
            y_curve += y_curve_delta;

            if (x_curve >= 0.01f || x_curve <= -0.01f) {
                x_curve_delta = -x_curve_delta;
            }
            
            if (y_curve >= 0.01f || y_curve <= 0.00f) {
                y_curve_delta = -y_curve_delta;
            }

            for (int i = 0; i < game_objects.Count; ++i) {
                game_objects[i].GetComponent<Renderer>().material.SetFloat("_XCurve", x_curve);
                game_objects[i].GetComponent<Renderer>().material.SetFloat("_YCurve", y_curve);
            }

            for (int i = 0; i < other_objects.Count; ++i) {
                other_objects[i].GetComponent<Renderer>().material.SetFloat("_XCurve", x_curve);
                other_objects[i].GetComponent<Renderer>().material.SetFloat("_YCurve", y_curve);
            }
        }

        for (int i = 0; i < game_objects.Count; ++i) {
            spawn_timers[i] += Time.deltaTime;

            if (spawn_timers[i] >= spawn_delays[i]) {
                spawn_timers[i] = 0.0f;
                var new_obstacle = Instantiate(game_objects[i], 
                                               new Vector3(game_objects[i].transform.position.x,
                                                           game_objects[i].transform.position.y,
                                                           game_objects[i].transform.position.z), 
                                               Quaternion.Euler(270.0f, 0.0f, 0.0f));

                new_obstacle.GetComponent<MeshCollider>().enabled = true;
                new_obstacle.GetComponent<Renderer>().enabled = true;
                new_obstacle.GetComponent<obstacle_script>().speed = 12.0f;
                gen_new_spawn_delays(i);
            }
        }
    }
}
