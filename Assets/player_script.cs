using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    public float speed = 10.0f;
    public float square = 1; // can be 0, 1, or 2 depending on the lane the player is in
                            // from left to right

    public float jump_force = 1.0f;


    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.GetButton("Jump")) {
            rb.AddForce(transform.up * jump_force, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float new_x = transform.position.x;
        if (Input.GetKeyDown(KeyCode.LeftArrow) && square > 0) {
            --square;
            new_x = transform.position.x - 1.0f;
        } else if (Input.GetKeyDown(KeyCode.RightArrow) && square < 2) {
            ++square;
            new_x = transform.position.x + 1.0f;
        }

        transform.position = new Vector3(new_x, transform.position.y, transform.position.z);
    }
}
