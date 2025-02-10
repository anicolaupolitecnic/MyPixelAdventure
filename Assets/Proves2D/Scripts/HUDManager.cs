using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Sprite leftIconPressed;
    [SerializeField] private Sprite leftIconReleased;
    [SerializeField] private Sprite rightIconPressed;
    [SerializeField] private Sprite rightIconReleased;
    [SerializeField] private GameObject leftButton;
    [SerializeField] private GameObject rightButton;
    private GameManagerProves2D gameManager;

    [SerializeField] private List<GameObject> lives;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerProves2D>();
    }

    public void LeftPressedIcon()
    {
        leftButton.GetComponent<Image>().sprite = leftIconPressed;
    }

    public void LeftReleasedIcon()
    {
        leftButton.GetComponent<Image>().sprite = leftIconReleased;
    }

    public void RightPressedIcon()
    {
        rightButton.GetComponent<Image>().sprite = rightIconPressed;
    }

    public void RightReleasedIcon()
    {
        rightButton.GetComponent<Image>().sprite = rightIconReleased;
    }

    public void UpdateHUD()
    {
        for (int i=0; i < 3; i++)
        {
            if (gameManager.numLives > i)
                lives[i].SetActive(true);
            else
                lives[i].SetActive(false);
        }
    }
}
