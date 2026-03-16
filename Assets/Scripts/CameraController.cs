using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        FindPlayer();
    }

    void Update()
    {
        if (player == null)
            FindPlayer();

        if (player == null) return;

        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    private void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
    }
}