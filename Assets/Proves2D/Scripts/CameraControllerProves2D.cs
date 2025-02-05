using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerProves2D : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform deadlyFloor;

    void Update()
    {
        if (this.transform.position.y > deadlyFloor.position.y)
        {
            transform.position = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        }
    }
}
