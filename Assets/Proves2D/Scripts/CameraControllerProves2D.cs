using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraControllerProves2D : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform deadlyFloor;
    [SerializeField] private Transform initPoint;
    [SerializeField] private Transform finalPoint;
    private float cameraWidth;

    private void Awake()
    {
        transform.position = new Vector3(player.position.x, player.position.y, this.transform.position.z);
    }

    private void Start()
    {
        cameraWidth = Camera.main.orthographicSize * 2 * Camera.main.aspect;
    }

    void Update()
    {
        if (this.transform.position.y > deadlyFloor.position.y)
        {
            if ((this.transform.position.x - cameraWidth / 2) <= initPoint.position.x)
            {
                Debug.Log("left");
                //transform.position = new Vector3(initPoint.position.x + cameraWidth / 2 + 0.1f, player.position.y, this.transform.position.z);
            }
            else if ((this.transform.position.x - cameraWidth / 2) >= finalPoint.position.x)
            {
                Debug.Log("right");
                //transform.position = new Vector3(finalPoint.position.x - cameraWidth / 2 - 0.1f, player.position.y, this.transform.position.z);
            }
            
            if ((player.transform.position.x - initPoint.position.x) >= cameraWidth/2 ||
                (finalPoint.position.x - player.transform.position.x ) <= cameraWidth / 2)
            {
                Debug.Log("NO");
                transform.position = new Vector3(player.position.x, player.position.y, this.transform.position.z);
            }
        }
    }
}
