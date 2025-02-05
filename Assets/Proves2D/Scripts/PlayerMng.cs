using UnityEditor.Animations;
using UnityEngine;

public class PlayerMng : MonoBehaviour
{
    [SerializeField] private Animator animCon;

    void Update()
    {
        Debug.Log("Dead: " + animCon.GetBool("Die"));

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!animCon.GetBool("Die"))
            {
                Debug.Log("Die");
                animCon.SetBool("Die", true);
            } else
            {
                animCon.SetBool("Die", false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Run");
            animCon.SetFloat("Speed", 1f);
        }
        else if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Idle");
            animCon.SetFloat("Speed", 0f);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Attack");
            animCon.SetTrigger("Attack");
        }
    }
}
