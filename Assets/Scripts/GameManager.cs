using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

<<<<<<< Updated upstream
public class GameManager : MonoBehaviour {
    private GameObject sndManager;
=======
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int items = 0;
    private Text itemsText;

    [Header("Prefabs")]
    [SerializeField] private GameObject soundManagerPrefab;
    [SerializeField] private GameObject playerPrefab;
    public SoundManager Sound { get; private set; }

>>>>>>> Stashed changes
    //LEVEL STATES
    public bool isGameOver = false;
    public bool isLevelRestarted = false;
    public bool isLevelCompleted = false;
    public bool isGameCompleted = false;

<<<<<<< Updated upstream
    void Start()  {
        //SOUNDMANAGER
=======
    [SerializeField] private GameObject mobileControlsPrefab;
    private GameObject mobileControlsInstance;

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
>>>>>>> Stashed changes
        InitSoundManager();
    }

<<<<<<< Updated upstream
    void InitSoundManager() {
        sndManager = GameObject.FindGameObjectWithTag("SoundManager");
        sndManager.GetComponent<SoundManager>().SetMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
        sndManager.GetComponent<SoundManager>().fxVolume = PlayerPrefs.GetFloat("fxVolume");
        sndManager.GetComponent<SoundManager>().SetEnableMusic(PlayerPrefs.GetInt("music")==1?true:false);
        sndManager.GetComponent<SoundManager>().isFXEnabled = PlayerPrefs.GetInt("fx")==1?true:false;
        sndManager.GetComponent<SoundManager>().PlayMusic(0);
=======
    private bool IsMobile()
    {
#if UNITY_EDITOR
        return false;
#elif UNITY_ANDROID || UNITY_IOS
        return true;
#else
        return false;
#endif
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
        SpawnPlayer();
        FindItemsText();
    }

    // ── Player Spawn ─────────────────────────────────────────────────
    private void FindItemsText()
    {
        itemsText = null;
        items = 0;

        GameObject textObj = GameObject.FindGameObjectWithTag("itemsTxt");
        if (textObj != null)
        {
            itemsText = textObj.GetComponent<Text>();
            itemsText.text = "Items: 0";
        }
        else
        {
            Debug.LogWarning("[GameManager] No s'ha trobat cap GameObject amb tag 'itemsTxt'.");
        }
    }

    private void SpawnPlayer()
    {
        if (!IsGameScene(SceneManager.GetActiveScene().name)) return;

        if (playerPrefab == null)
        {
            Debug.LogError("[GameManager] playerPrefab no assignat al Inspector!");
            return;
        }

        GameObject wp = GameObject.FindGameObjectWithTag("StartWP");
        if (wp == null)
        {
            Debug.LogError("[GameManager] No s'ha trobat cap StartWaypoint a l'escena!");
            return;
        }

        Instantiate(playerPrefab, wp.transform.position, wp.transform.rotation);
        Debug.Log("[GameManager] Player instanciat a: " + wp.transform.position);
    }

    // ── Mobile Controls ──────────────────────────────────────────────

    private void UpdateMobileControls()
    {
        bool isMobile = IsMobile();
        bool isGameScene = IsGameScene(SceneManager.GetActiveScene().name);

        if (isMobile && isGameScene)
        {
            if (mobileControlsInstance == null)
            {
                Canvas sceneCanvas = FindFirstObjectByType<Canvas>();
                if (sceneCanvas != null)
                {
                    mobileControlsInstance = Instantiate(mobileControlsPrefab, sceneCanvas.transform);
                }
            }
        }
        else
        {
            if (mobileControlsInstance != null)
            {
                Destroy(mobileControlsInstance);
                mobileControlsInstance = null;
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
>>>>>>> Stashed changes
    }

    // ── Level States ─────────────────────────────────────────────────

    void Reset()
    {
        isGameOver = false;
        isLevelRestarted = false;
        isLevelCompleted = false;
        isGameCompleted = false;
        items = 0;
    }

    void OnGUI()
    {
        if (isGameCompleted || isGameOver)
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 30;
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

    // Mètode públic perquè PlayerManager pugui cridar-lo
    public void CollectItem()
    {
        items++;
        itemsText.text = "Items: " + items.ToString();
        Debug.Log("[GameManager] Items recollits: " + items);
    }
}