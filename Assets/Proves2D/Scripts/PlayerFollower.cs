using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform deadlyFloor;
    [SerializeField] private Transform respawn;
    private bool isPlayerFalling  = false;
    void Update()
    {
        if (!isPlayerFalling)
        {
            if (this.transform.position.y >= deadlyFloor.position.y)
            {
                transform.position = player.position;
            }
            else
            {
                isPlayerFalling = true;
            }
            
        } 
        else
        {
            if (player.transform.position.y >= deadlyFloor.position.y)
            {
                isPlayerFalling = false;
                transform.position = player.position;
            }
        }
        
    }
}
