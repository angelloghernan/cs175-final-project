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
    private float new_x = -4.502f;
    public float jump_force = 5.0f;

    private bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        char_controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && square > 0) {
            moving = true;
            animator.Play("dodgeLeft");
            new_x -= 1.0f;
            --square;
        } else if (Input.GetKeyDown(KeyCode.RightArrow) && square < 2) {
            moving = true;
            animator.Play("dodgeRight");
            new_x += 1.0f;
            ++square;
        }

        x = Mathf.Lerp(x, new_x, Time.deltaTime * speed);
        Vector3 final_pos = new Vector3(x - transform.position.x, 0.0f, 0.0f);
        char_controller.Move(final_pos);
    }
}
