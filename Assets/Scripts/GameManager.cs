using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int items = 0;
    private Text itemsText;

    [Header("Prefabs")]
    [SerializeField] private GameObject soundManagerPrefab;
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;
    public SoundManager Sound { get; private set; }

    //LEVEL STATES
    public bool isGameOver = false;
    public bool isLevelRestarted = false;
    public bool isLevelCompleted = false;
    public bool isGameCompleted = false;
    
    [Header("Configuració d'escenes")]
    [Tooltip("Noms exactes de les escenes que usen la música de menú")]
    public string[] menuSceneNames = { "Menu", "MainMenu" };
    [Tooltip("Noms exactes de les escenes que usen la música de joc")]
    public string[] gameSceneNames = { "Game", "Level1", "Level2", "Level3" };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("[GameManager] Duplicat detectat, destruint.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitSoundManager();
    }

    void Start()
    {
        InitSoundManager();
        UpdateMobileControls();
    }

    private bool IsMobile()
    {
        return true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateMobileControls();
        FindItemsText();
        SpawnPlayer();
    }

    // ── Items Text ───────────────────────────────────────────────────

    private void FindItemsText()
    {
        itemsText = null;
        items = 0;

        GameObject textObj = GameObject.FindGameObjectWithTag("ItemsText");
        if (textObj != null)
        {
            itemsText = textObj.GetComponent<Text>();
            itemsText.text = "Items: 0";
        }
        else
        {
            Debug.LogWarning("[GameManager] No s'ha trobat cap GameObject amb tag 'ItemsText'.");
        }
    }

    // ── Player Spawn ─────────────────────────────────────────────────

    private void SpawnPlayer()
    {
        if (!IsGameScene(SceneManager.GetActiveScene().name)) return;

        GameObject wp = GameObject.FindGameObjectWithTag("StartWP");
        if (wp == null)
        {
            Debug.LogError("[GameManager] No s'ha trobat cap StartWaypoint a l'escena!");
            return;
        }

        if (playerPrefab != null)
        {
            player = playerPrefab;
            player.transform.position = wp.transform.position;
        }
        else
            Debug.Log("Error");
    }

    // ── Mobile Controls ──────────────────────────────────────────────

    private void UpdateMobileControls()
    {
        bool isMobile = IsMobile();
        bool isGameScene = IsGameScene(SceneManager.GetActiveScene().name);

        if (isMobile && isGameScene)
        {
            GameObject go = GameObject.FindWithTag("Touch");
            foreach (Transform child in go.transform)
            {
                child.transform.gameObject.SetActive(true);
            }
        }
    }

    private bool IsGameScene(string sceneName)
    {
        foreach (string name in gameSceneNames)
        {
            if (name == sceneName) return true;
        }
        return false;
    }

    // ── Sound Manager ────────────────────────────────────────────────

    private void InitSoundManager()
    {
        Sound = FindFirstObjectByType<SoundManager>();

        if (Sound != null)
        {
            Debug.Log("[GameManager] SoundManager ja existeix a l'escena, no s'instancia de nou.");
            return;
        }

        if (soundManagerPrefab == null)
        {
            Debug.LogError("[GameManager] soundManagerPrefab no assignat al Inspector!");
            return;
        }

        GameObject go = Instantiate(soundManagerPrefab);
        go.name = "SoundManager";
        Sound = go.GetComponent<SoundManager>();

        if (Sound == null)
        {
            Debug.LogError("[GameManager] El prefab SoundManager no té el component SoundManager!");
            return;
        }

        Debug.Log("[GameManager] SoundManager instanciat correctament.");
    }

    // ── Level States ─────────────────────────────────────────────────

    void Reset()
    {
        isGameOver = false;
        isLevelRestarted = false;
        isLevelCompleted = false;
        isGameCompleted = false;
        items = 0;
        if (itemsText != null) itemsText.text = "Items: 0";
    }

    void OnGUI()
    {
        if (isGameCompleted || isGameOver)
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 30;
            myButtonStyle.alignment = TextAnchor.MiddleCenter;

            // Fons base (necessari per poder tintar amb GUI.color)
            myButtonStyle.normal.background = Texture2D.whiteTexture;
            myButtonStyle.hover.background = Texture2D.whiteTexture;
            myButtonStyle.active.background = Texture2D.whiteTexture;

            myButtonStyle.normal.textColor = Color.gray;
            myButtonStyle.hover.textColor = Color.red;

            if (GUI.Button(new Rect(Screen.width / 2 - Screen.width / 8, Screen.height / 2 - Screen.height / 8, Screen.width / 4, Screen.height / 4),
                isGameCompleted ? "CONGRATULATIONS!!" : "GAMEOVER!!", myButtonStyle))
            {
                Reset();
                SceneManager.LoadScene(1);
            }
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CompleteLevel()
    {
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            isGameCompleted = true;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // ── Items ────────────────────────────────────────────────────────

    public void CollectItem()
    {
        items++;
        if (itemsText != null) itemsText.text = "Items: " + items;
        Debug.Log("[GameManager] Items recollits: " + items);
    }
}