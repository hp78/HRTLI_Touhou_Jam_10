using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public float horizontalRange;
    public float verticalRange;
    public int density;

    [Space(5)]
    public Transform obstacleParent;

    [Space(5)]
    public GameObject pfTreeObstacle0;
    public GameObject pfTreeObstacle1;
    public GameObject pfTreeObstacle2;

    [Space(2)]
    public GameObject pfRockObstacle;
    public GameObject pfHoleObstacle;

    [Space(2)]
    public GameObject pfChenObstacle;

    [Space(2)]
    public GameObject pfSnowboarderObstacle;

    [Space(2)]
    public GameObject pfJumpRamp;

    [Space(2)]
    public GameObject pfYeti;
    public GameObject pfRan;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        InitializeObstacles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitializeObstacles()
    {
        for(int i = 0; i < density; ++i)
        {
            GameObject go;

            if (i%5 == 0)
            {
                go = Instantiate(pfTreeObstacle0, obstacleParent);
            }
            else if(i%5 == 1)
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
}
