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

    List<Transform> playerTransforms = new List<Transform>();

    [Space(5)]
    public float horizontalRange;
    public float verticalRange;
    public int density;

    [Space(5)]
    public Transform obstacleParent;

    [Space(5)]
    public GameObject pfTreeObstacle0;
    public GameObject pfTreeObstacle1;
    public GameObject pfTreeObstacle2;
    public GameObject pfRockObstacle;
    public GameObject pfHoleObstacle;

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

            if (i % 5 == 0)
            {
                go = Instantiate(pfTreeObstacle0, obstacleParent);
            }
            else if (i % 5 == 1)
            {
                go = Instantiate(pfTreeObstacle1, obstacleParent);
            }
            else if (i % 5 == 2)
            {
                go = Instantiate(pfTreeObstacle2, obstacleParent);
            }
            else if (i % 5 == 3)
            {
                go = Instantiate(pfRockObstacle, obstacleParent);
            }
            else
            {
                go = Instantiate(pfHoleObstacle, obstacleParent);
            }

            go.transform.position = new Vector3(
                Random.Range(-horizontalRange, horizontalRange),
                Random.Range(-verticalRange, verticalRange) - verticalRange, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int playerCount = playerTransforms.Count;


        if (playerCount > 0)
        {
            float smallestX = playerTransforms[0].position.x;
            float largestX = playerTransforms[0].position.x;
            float smallestY = playerTransforms[0].position.y;
            float largestY = playerTransforms[0].position.y;

            Vector3 addedPos = Vector3.zero;
            foreach(Transform t in playerTransforms)
            {
                addedPos += t.position;

                smallestX = (t.position.x < smallestX) ? t.position.x : smallestX;
                largestX = (t.position.x > smallestX) ? t.position.x : largestX;
                smallestY = (t.position.y < smallestY) ? t.position.y : smallestY;
                largestY = (t.position.y > largestY) ? t.position.y : largestY;
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

    public void AddPlayerTransform(Transform tf)
    {
        playerTransforms.Add(tf);
    }
}
