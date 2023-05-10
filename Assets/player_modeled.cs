using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_modeled : MonoBehaviour
{
    private CharacterController char_controller;
    private Animator animator;
    private Rigidbody rb;
    public float speed = 10.0f;
    // can be 0, 1, or 2 depending on the lane the player is in
    private int square = 1;
    private float x = -4.502f;
    private float y = 0.0f;
    private float new_x = -4.502f;
    public float jump_force = 2.5f;

    private bool moving = false;
    private bool jumping = false;
    private bool rolling = false;
    private float roll_timer = 0.0f;
    private float collision_height;
    private float center_y;

    // Start is called before the first frame update
    void Start()
    {
        char_controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        char_controller.Move(new Vector3(x, 0.0f, transform.position.z));
        collision_height = char_controller.height;
        center_y = char_controller.center.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rolling && Input.GetKeyDown(KeyCode.LeftArrow) && square > 0) {
            moving = true;
            animator.Play("dodgeLeft");
            new_x -= 1.0f;
            --square;
        } else if (!rolling && Input.GetKeyDown(KeyCode.RightArrow) && square < 2) {
            moving = true;
            animator.Play("dodgeRight");
            new_x += 1.0f;
            ++square;
        }

        // handle jumping
        if (char_controller.isGrounded) {
            if (jumping) {
                animator.Play("landing2");
                jumping = false;
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                y = jump_force;
                animator.CrossFadeInFixedTime("jump2", 0.1f);
                jumping = true;
            }
        } else {
            y -= jump_force * 2 * Time.deltaTime;
            if (char_controller.velocity.y < 0.1f) {
                animator.Play("hangtime3");
            }
        }

        handle_rolling();

        Vector3 final_pos = new Vector3(x - transform.position.x, y * Time.deltaTime, 0.0f);
        x = Mathf.Lerp(x, new_x, Time.deltaTime * speed);
        char_controller.Move(final_pos);
    }

    void handle_rolling() {
        roll_timer -= Time.deltaTime;
        if (roll_timer <= 0.0f) {
            roll_timer = 0.0f;
            char_controller.center = Vector3.up * center_y;
            char_controller.height = collision_height;
            rolling = false;
        }

        if (!rolling && Input.GetKeyDown(KeyCode.DownArrow)) {
            roll_timer = 0.2f;
            y -= 10.0f;
            char_controller.center = Vector3.up * (center_y / 2);
            char_controller.height = collision_height / 2;
            animator.CrossFadeInFixedTime("roll", 0.1f);
            rolling = true;
            jumping = false;
        }
    }
}
