using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroller_script : MonoBehaviour
{
    private float x_curve = 0.0f;
    private float y_curve = 0.0f;
    public float x_curve_delta = 0.00001f;
    public float y_curve_delta = 0.00001f;
    private long curve_frame = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ++curve_frame;
        if (curve_frame % 20 == 0) {
            x_curve += x_curve_delta;

            if (x_curve >= 0.01f || x_curve <= -0.01f) {
                x_curve_delta = -x_curve_delta;
            }

            for (int i = 0; i < object_renderers.Count; ++i) {
                GetComponent<Renderer>().material.SetFloat("_XCurve", x_curve);
            }
        }

        if (curve_frame % 30 == 0) {
            y_curve += y_curve_delta;

            if (y_curve >= 0.002f || y_curve < 0.00f) {
                y_curve_delta = -y_curve_delta;
            }

            for (int i = 0; i < object_renderers.Count; ++i) {
                GetComponent<Renderer>().material.SetFloat("_YCurve", y_curve);
            }
        }
        
    }
}
