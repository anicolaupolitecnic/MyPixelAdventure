using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowerY : MonoBehaviour
{
    public float offsetY = 0f;

    void Update()
    {
        transform.position = new Vector3(this.transform.position.x, Camera.main.transform.position.y + offsetY, this.transform.position.z);
    }
}