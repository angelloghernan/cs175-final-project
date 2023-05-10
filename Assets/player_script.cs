using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    public float speed = 2.5f;
    // can be 0, 1, or 2 depending on the lane the player is in
    public int square = 1;

    public float jump_force = 5.0f;


    Rigidbody rb;
    public Camera main_camera;
    BoxCollider box_collider;
    public bool is_jumping = false;
    public bool is_fast_falling = false;
    public bool moving = false;
    public bool ducking = false;
    private Vector3[] end_points = new Vector3[3];
    private Vector3 start_point;
    private float move_timer = 0.0f;
    private float duck_timer = 0.0f;
    private float time_to_reach_end = 0.25f;

    private Vector3 panning_camera_start;
    private Vector3 panning_camera_end;
    private float panning_camera_timer = 0.0f;
    private float panning_camera_timer_end = 0.75f;
    private bool is_panning_camera = false;
    private bool is_panning_camera_back = false;
    private float camera_x_rotation = 20;

    // Start is called before the first frame update
    void Start()
    {
        panning_camera_start = main_camera.transform.position;
        panning_camera_end = new Vector3(main_camera.transform.position.x,
                                         transform.position.y,
                                         main_camera.transform.position.z);

        rb = GetComponent<Rigidbody>();
        box_collider = GetComponent<BoxCollider>();
        is_jumping = false;
        end_points[0] = new Vector3(transform.position.x - 1.0f, 0.0f, transform.position.z);
        end_points[1] = new Vector3(transform.position.x, 0.0f, transform.position.z);
        end_points[2] = new Vector3(transform.position.x + 1.0f, 0.0f, transform.position.z);

        GetComponent<Renderer>().material.color = Color.green;
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Jump") && !is_jumping) {
            rb.AddForce(new Vector3(0, jump_force, 0), ForceMode.Impulse);
            is_jumping = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        move_timer += Time.deltaTime;
        duck_timer += Time.deltaTime;

        update_camera();

        if (moving && move_timer >= time_to_reach_end) {
            moving = false;
        } else if (moving) {
            float t = move_timer / time_to_reach_end;
            Vector3 start = new Vector3(start_point.x, transform.position.y, start_point.z);
            Vector3 end = new Vector3(end_points[square].x, 
                                      transform.position.y, 
                                      end_points[square].z);
            transform.position = Vector3.Lerp(start, end, t);
        }

        if (ducking && duck_timer >= 2.0 / speed) {
            ducking = false;
            transform.localScale = new Vector3(transform.localScale.x,
                                               transform.localScale.y * 2.0f,
                                               transform.localScale.z);
            box_collider.size = new Vector3(box_collider.size.x,
                                            box_collider.size.y * 2.0f,
                                            box_collider.size.z);
        }

        if (!ducking && !is_jumping && Input.GetKeyDown(KeyCode.DownArrow)) {
            ducking = true;
            duck_timer = 0;
            transform.localScale = new Vector3(transform.localScale.x,
                                               transform.localScale.y / 2.0f,
                                               transform.localScale.z);
            box_collider.size = new Vector3(box_collider.size.x,
                                            box_collider.size.y / 2.0f,
                                            box_collider.size.z);
        } else if (!ducking && !is_fast_falling && 
            is_jumping && Input.GetKeyDown(KeyCode.DownArrow)) {
            rb.AddForce(new Vector3(0.0f, -jump_force * 1.5f, 0.0f), ForceMode.Impulse);
            is_fast_falling = true;
        } 

        if (Input.GetKeyDown(KeyCode.LeftArrow) && square > 0) {
            move_timer = 0.0f;
            moving = true;
            start_point = transform.position;
            --square;
            time_to_reach_end = Mathf.Abs(end_points[square].x - start_point.x) / speed;
        } else if (Input.GetKeyDown(KeyCode.RightArrow) && square < 2) {
            move_timer = 0.0f;
            moving = true;
            start_point = transform.position;
            ++square;
            time_to_reach_end = Mathf.Abs(end_points[square].x - start_point.x) / speed;
        }

    }


    void OnCollisionEnter(Collision collision) {
        // Enable jumping when the player touches the ground
        if (collision.gameObject.tag == "ground") {
            is_jumping = false;
            is_fast_falling = false;
        } else if (collision.gameObject.tag == "obstacle") {
            // player gets a game over...
            GetComponent<Renderer>().material.color = Color.red;
        }
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "arch_end") {
            panning_camera_timer = 0.0f;
            is_panning_camera = false;
            is_panning_camera_back = true;
        } else if (collision.gameObject.tag == "arch_trigger") {
            panning_camera_timer = 0.0f;
            is_panning_camera = true;
            is_panning_camera_back = false;
        }
    }

    void update_camera() {
        panning_camera_timer += Time.deltaTime;

        if (panning_camera_timer > panning_camera_timer_end) {
            is_panning_camera = false;
            is_panning_camera_back = false;
            panning_camera_timer = 0.0f;
        }

        if (is_panning_camera) {
            float t = panning_camera_timer / panning_camera_timer_end;
            main_camera.transform.position = Vector3.Lerp(panning_camera_start, 
                                                          panning_camera_end,
                                                          t);
            float angle = Mathf.LerpAngle(camera_x_rotation, 0.0f, t);
            main_camera.transform.eulerAngles = new Vector3(angle, 0.0f, 0.0f);
        } else if (is_panning_camera_back) {
            float t = panning_camera_timer / panning_camera_timer_end;
            main_camera.transform.position = Vector3.Lerp(panning_camera_end, 
                                                          panning_camera_start,
                                                          t);
            float angle = Mathf.LerpAngle(0.0f, camera_x_rotation, t);
            main_camera.transform.eulerAngles = new Vector3(angle, 0.0f, 0.0f);
        }
    }
}
