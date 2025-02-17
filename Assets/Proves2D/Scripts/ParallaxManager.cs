using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxManager : MonoBehaviour {
    public float parallaxEffect;
    private float length, startPos;

    void Start() {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update() {
        float temp = (Camera.main.transform.position.x * (1 - parallaxEffect));
        float dist = (Camera.main.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);

        if (temp > startPos + length){
            startPos += length;
        } else if (temp < startPos - length) {
            startPos -= length;
        }

        if (transform.position.x < -(length))
        {
            transform.position = new Vector3(length, transform.position.y, transform.position.z);
        } else if (transform.position.x > (length))
        {
            transform.position = new Vector3(-length, transform.position.y, transform.position.z);
        }
    }
}
