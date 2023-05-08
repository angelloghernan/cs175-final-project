using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    public float speed = 10.0f;
    public float square = 1; // can be 0, 1, or 2 depending on the lane the player is in
                            // from left to right

    public float jump_force = 10.0f;


    Rigidbody rb;
    public bool is_jumping = false;
    public bool fast_falling = false;
    public bool moving = false;
    private Vector3 start_point;
    private Vector3 end_point;
    private float move_timer = 0.0f;
    private float time_to_reach_end = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        is_jumping = false;
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
            return;
        } else if (moving) {
            float t = move_timer / time_to_reach_end;
            Vector3 start = new Vector3(start_point.x, transform.position.y, start_point.z);
            Vector3 end = new Vector3(end_point.x, transform.position.y, end_point.z);
            transform.position = Vector3.Lerp(start, end, t);
        }

        if (!fast_falling && is_jumping && Input.GetKeyDown(KeyCode.DownArrow)) {
            rb.AddForce(new Vector3(0, -jump_force * 2, 0), ForceMode.Impulse);
            fast_falling = true;
        } else if (Input.GetKeyDown(KeyCode.LeftArrow) && square > 0) {
            move_timer = 0.0f;
            moving = true;
            start_point = transform.position;
            end_point = new Vector3(transform.position.x - 1.0f,
                                    0.184f,
                                    transform.position.z);
            --square;
        } else if (Input.GetKeyDown(KeyCode.RightArrow) && square < 2) {
            move_timer = 0.0f;
            moving = true;
            start_point = transform.position;
            end_point = new Vector3(transform.position.x + 1.0f,
                                    0.184f,
                                    transform.position.z);
            ++square;
        }

    }

    void OnCollisionEnter(Collision collision) {
        // Enable jumping when the player touches the ground
        if (collision.gameObject.tag == "ground") {
            is_jumping = false;
            fast_falling = false;
        }
    }
}
