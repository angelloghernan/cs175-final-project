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
    public bool is_jumping = false;
    public bool is_fast_falling = false;
    public bool moving = false;
    private Vector3[] end_points = new Vector3[3];
    private Vector3 start_point;
    private float move_timer = 0.0f;
    private float time_to_reach_end = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        is_jumping = false;
        end_points[0] = new Vector3(transform.position.x - 1.0f, 0.0f, transform.position.z);
        end_points[1] = new Vector3(transform.position.x, 0.0f, transform.position.z);
        end_points[2] = new Vector3(transform.position.x + 1.0f, 0.0f, transform.position.z);
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

        if (!is_fast_falling && is_jumping && Input.GetKeyDown(KeyCode.DownArrow)) {
            rb.AddForce(new Vector3(0.0f, -jump_force * 1.5f, 0.0f), ForceMode.Impulse);
            is_fast_falling = true;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow) && square > 0) {
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
        }
    }
}
