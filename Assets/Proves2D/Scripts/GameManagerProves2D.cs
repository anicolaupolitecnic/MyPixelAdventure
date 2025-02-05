using UnityEngine;

public class GameManagerProves2D : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform spawnPoint;
    public void RestartLevel()
    {
        player.transform.position = spawnPoint.position;
    }
}
