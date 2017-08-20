using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    float h, v;

    public void OnStickChanged(Vector2 stickPos)
    {
        h = stickPos.x;
        v = stickPos.y;
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody rigibody = GetComponent<Rigidbody>();

        if (rigibody)
        {
            if (h != 0f && v != 0f)
            {
                transform.rotation = Quaternion.LookRotation(new Vector3(h, 0f, v));
            }
        }
    }
}
