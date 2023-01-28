using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float horizontalRange;
    public float verticalRange;
    public int density;

    [Space(5)]
    public GameObject pfTreeObstacle0;
    public GameObject pfTreeObstacle1;
    public GameObject pfTreeObstacle2;
    public GameObject pfRockObstacle;
    public GameObject pfHoleObstacle;

    // Start is called before the first frame update
    void Start()
    {
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
                go = Instantiate(pfTreeObstacle0);
            }
            else if(i%5 == 1)
            {
                go = Instantiate(pfTreeObstacle1);
            }
            else if (i % 5 == 2)
            {
                go = Instantiate(pfTreeObstacle2);
            }
            else if (i % 5 == 3)
            {
                go = Instantiate(pfRockObstacle);
            }
            else
            {
                go = Instantiate(pfHoleObstacle);
            }

            go.transform.position = new Vector3(
                Random.Range(-horizontalRange, horizontalRange),
                Random.Range(-verticalRange, verticalRange) - verticalRange, 0);
        }
    }
}
