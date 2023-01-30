using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
public class GameControllerMultiplayer : MonoBehaviour
{
    public static GameControllerMultiplayer instance;
    public Camera mainCam;
    float targetCamSize = 7;
    public PlayerInputManager playerInputManager;

    List<PlayerControllerMP> _players = new List<PlayerControllerMP>();
    public List<PlayerControllerMP> _playersPerma = new List<PlayerControllerMP>();

    [Space(5)]
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject loosePanel;

    [Space(5)]
    public float horizontalRange;
    public float verticalRange;
    public int density;

    [Space(5)]
    public Transform obstacleParent;

    [Space(5)]
    bool _isYetiSpawned = false;
    [SerializeField] GameObject pfYeti;

    [Space(5)]
    public GameObject[] obstacles;

    public GameObject pfChen;
    public GameObject pfSnowboarder;

    [Space(5)]
    public TMP_Text lobbyTmp;
    public Button startGameBtn;

    [Space(5)]
    [SerializeField] BGM _mainBgm;

    public TMP_Text winTmp;
    public TMP_Text loseTmp;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        instance = this;
        InitializeObstacles();

        _mainBgm = GameObject.FindGameObjectWithTag("BGM").GetComponent<BGM>();
        _mainBgm.CallFader(false);

    }

    void InitializeObstacles()
    {
        for (int i = 0; i < density; ++i)
        {
            GameObject go;

            go = Instantiate(obstacles[i % (obstacles.Length)], obstacleParent);

            go.transform.position = new Vector3(
                Random.Range(-horizontalRange, horizontalRange),
                Random.Range(-verticalRange, verticalRange) - verticalRange, 0);
        }

        for(int i = 1; i < 10; ++i)
        {
            GameObject go;

            go = Instantiate(pfChen, obstacleParent);
            go.transform.position = new Vector3(0, -100f * i, 0);

            go = Instantiate(pfSnowboarder, obstacleParent);
            go.transform.position = new Vector3(0, -100f * i, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLobby();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0f)
                ResumeGame();
            else
                PauseGame();
        }

        int playerCount = _players.Count;

        if (playerCount > 0)
        {
            float smallestX = _players[0].transform.position.x;
            float largestX = _players[0].transform.position.x;
            float smallestY = _players[0].transform.position.y;
            float largestY = _players[0].transform.position.y;

            Vector3 addedPos = Vector3.zero;
            foreach(PlayerControllerMP p in _players)
            {
                addedPos += p.transform.position;

                smallestX = (p.transform.position.x < smallestX) ? p.transform.position.x : smallestX;
                largestX = (p.transform.position.x > smallestX) ? p.transform.position.x : largestX;
                smallestY = (p.transform.position.y < smallestY) ? p.transform.position.y : smallestY;
                largestY = (p.transform.position.y > largestY) ? p.transform.position.y : largestY;
            }
            addedPos /= playerCount;
            addedPos += new Vector3(0, -1, -10);

            mainCam.transform.position = 
                Vector3.Lerp(mainCam.transform.position, addedPos, Time.deltaTime * 10f);

            if (playerCount > 1)
            {
                float largestDistance = 
                    (((largestX - smallestX)*2) > (largestY - smallestY)) ?
                    ((largestX - smallestX)*2)  : (largestY - smallestY);
                targetCamSize = Mathf.Clamp(largestDistance * 0.5f + 3f, 6, 50);
            }
            else
            {
                targetCamSize = 6;
            }
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, targetCamSize, 0.1f);
        }
    }

    public void UpdateLobby()
    {

    }

    public void AddPlayer(PlayerControllerMP pcmp, string joinMessage = "")
    {
        _players.Add(pcmp);
        _playersPerma.Add(pcmp);
        lobbyTmp.text += joinMessage;
        startGameBtn.interactable = true;
    }

    public void RemovePlayer(PlayerControllerMP pcmp)
    {
        _players.Remove(pcmp);
    }

    public void SpawnYeti()
    {
        if (_isYetiSpawned)
            return;

        _isYetiSpawned = true;

        GameObject yeti = Instantiate(pfYeti);
        YetiAIController yetiAI = yeti.GetComponent<YetiAIController>();
    }

    public void WinGame()
    {
        int bestPlayer = 0;
        int bestDistance = 0;

        foreach (PlayerControllerMP pcmp in _playersPerma)
        {
            if ((int)(-pcmp.transform.position.y) > bestDistance)
            {
                bestDistance = (int)(-pcmp.transform.position.y);
                bestPlayer = pcmp.playerIndex;
            }
        }

        winTmp.text = "MVP : <color=\"red\">Player " + bestPlayer;

        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        playerInputManager.DisableJoining();
        _mainBgm.CallFader(true);
    }

    public void LooseGame()
    {
        int bestPlayer = 0;
        int bestDistance = 0;

        foreach(PlayerControllerMP pcmp in _playersPerma)
        {
            if((int)(-pcmp.transform.position.y) > bestDistance)
            {
                bestDistance = (int)(-pcmp.transform.position.y);
                bestPlayer = pcmp.playerIndex;
            }
        }

        loseTmp.text = "MVP : <color=\"red\">Player " + bestPlayer + "<color=\"blue\"> (" + bestDistance + "m)";
        loosePanel.SetActive(true);
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void RestartGame()
    {

        SceneManager.LoadScene("MainMultiplayer");
    }

    public void QuitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

  
}
