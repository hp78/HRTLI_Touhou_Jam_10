using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameControllerMultiplayer : MonoBehaviour
{
    public static GameControllerMultiplayer instance;
    public Camera mainCam;
    float targetCamSize = 7;
    public PlayerInputManager playerInputManager;

    List<PlayerControllerMP> _players = new List<PlayerControllerMP>();

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

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        InitializeObstacles();
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
    }

    // Update is called once per frame
    void Update()
    {
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
                Vector3.Lerp(mainCam.transform.position, addedPos, 0.5f);

            if (playerCount > 1)
            {
                //Vector2 crossDistance = new Vector2(largestX - smallestX, largestY - smallestY);
                float largestDistance = 
                    (((largestX - smallestX)*2) > (largestY - smallestY)) ?
                    ((largestX - smallestX)*2)  : (largestY - smallestY);
                targetCamSize = Mathf.Clamp(largestDistance * 0.5f + 3f, 6, 50);
            }
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, targetCamSize, 0.1f);
        }
    }

    public void AddPlayer(PlayerControllerMP tf)
    {
        _players.Add(tf);
    }

    public void SpawnYeti()
    {
        if (_isYetiSpawned)
            return;

        _isYetiSpawned = true;

        GameObject yeti = Instantiate(pfYeti);
        YetiAIController yetiAI = yeti.GetComponent<YetiAIController>();

        foreach(PlayerControllerMP player in _players)
        {
            yetiAI.AddPlayer(player.transform);
        }
    }
}
