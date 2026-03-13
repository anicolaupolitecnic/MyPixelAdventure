using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;

    void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else
            Debug.LogError("[CameraController] No s'ha trobat cap GameObject amb tag 'Player'!");
    }

    void Update()
    {
        if (player == null) return;
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }
}