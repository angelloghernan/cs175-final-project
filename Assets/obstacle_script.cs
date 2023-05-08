using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle_script : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < player.transform.position.z - 5.0f) {
            Destroy(gameObject);
        }
    }
}
