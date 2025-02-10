using UnityEngine;

public class GameManagerProves2D : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform spawnPoint;

    public int numLives;

    private void Start()
    {
        numLives = 3;
    }

    public void RestartLevel()
    {
        player.transform.position = spawnPoint.position;
    }
}
