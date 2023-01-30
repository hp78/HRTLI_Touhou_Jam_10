using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTreeAiController : MonoBehaviour
{

    [Space(5)]
    [SerializeField] SpriteRenderer _walk;
    [SerializeField] SpriteRenderer _idle;
    [SerializeField] ParticleSystem _snow;


    [Space(5)]
    [SerializeField] float _speed;
    [SerializeField] float _direction;

    [Space(5)]
    [SerializeField] List<PlayerControllerMP> _playerList = new List<PlayerControllerMP>();

    float _changeDirectionCooldown = 0f;
    float _idleDuration = 0f;
    float _walkDuration = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_idleDuration > 0.0f)
            Idle();
        else
        {
            Movement();

            if (_changeDirectionCooldown < 0f)
            {
                _direction = Random.Range(-1f, 1f);
                _changeDirectionCooldown = Random.Range(0.5f, 3f);
                CheckPlayerDistance();
            }

            _changeDirectionCooldown -= Time.deltaTime;
        }
    }
    void Movement()
    {
        this.transform.position += new Vector3((_speed * _direction),
                                                0f, 0f)
                                                * Time.deltaTime;

        _walk.flipX = _direction < 0.0f;

        _walkDuration -= Time.deltaTime;
        if(_walkDuration<0.0f)
        {
            _idleDuration = Random.Range(0.5f, 2f);
            _walk.gameObject.SetActive(false);
            _idle.gameObject.SetActive(true);
        }
    }

    void Idle()
    {
        
        _idleDuration -= Time.deltaTime;
        if(_idleDuration<0.0f)
        {
            _walkDuration = Random.Range(0.5f, 2f);
            _walk.gameObject.SetActive(true);
            _idle.gameObject.SetActive(false);


        }
    }

    void CheckPlayerDistance()
    {
        var temp = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject go in temp)
        {
            var tempplayer = go.GetComponent<PlayerControllerMP>();
            if (!_playerList.Contains(tempplayer))
                _playerList.Add(go.GetComponent<PlayerControllerMP>());
        }

        if (_playerList.Count == 0) return;


        List<Transform> aliveList = new List<Transform>();
        foreach (PlayerControllerMP player in _playerList)
        {

            if (player.isAlive)
                aliveList.Add(player.transform);
        }

        Transform lowestPlayer = null;

        foreach (Transform t in aliveList)
        {
            if (t.position.y+5f > this.transform.position.y)
                return;

            if (!lowestPlayer) lowestPlayer = t;

            else if (t.position.y < lowestPlayer.position.y) lowestPlayer = t;
        }

        if (lowestPlayer) this.transform.position = new Vector3(lowestPlayer.position.x + Random.Range(-10f, 10f),
                                                                 lowestPlayer.position.y - Random.Range(30f, 40f),
                                                                 lowestPlayer.position.z);


    }
}
