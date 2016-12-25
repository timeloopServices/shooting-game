using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour
{
    public float speed = 5f;

    void Update()
    {
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        if ((h != 0 || v != 0))
        {
            Vector3 angle = transform.eulerAngles + new Vector3(-v, h, 0f);
            if ((angle.y >= 315f || angle.y <= 100f) && (angle.x >= 330f || angle.x <= 20f))
            {
                transform.eulerAngles += new Vector3(-v, h, 0f);
            }
            //if (angle.x >= 330f || angle.x <= 20f)
            //{
            //    transform.eulerAngles += new Vector3(-v, 0f, 0f);
            //}
        }
    }
}
