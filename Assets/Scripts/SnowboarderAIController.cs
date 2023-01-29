using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowboarderAIController : MonoBehaviour
{
    [Space(5)]
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] Sprite _up;
    [SerializeField] Sprite _side;
    [SerializeField] Sprite _diagonal;
    [SerializeField] Sprite _down;
    [SerializeField] Sprite _crash;
    [Space(5)]
    [SerializeField] float _speed;
    [SerializeField] Collider2D _col;

    [Space(5)]
    [SerializeField] Vector3 _currDirection;
    [SerializeField] Transform _targetDirection;
    [SerializeField] float _directionChangeRate;

    [Space(5)]
    [SerializeField] float _catchUpDistThreshold = 12;
    [SerializeField] float _speedMod = 12;
    [SerializeField] List<Transform> _playerList = new List<Transform>();


    float _changeDirectionCooldown;
    bool _changingDirection;
    bool _crashed = false;

    float _crashDuration;
    float _currentSpeed = 0;

    public Vector3 newTarget;
    GameObject _currObstacle = null;
    DistanceComparer distanceComparer;


    // Start is called before the first frame update
    void Start()
    {
        distanceComparer = new DistanceComparer(transform);

    }

    // Update is called once per frame
    void Update()
    {
        if (!_crashed)
            Movement();
        else
        {
            _crashDuration -= Time.deltaTime;
            if (_crashDuration < 0.0f)
            {
                _crashed = false;
                _currentSpeed = 0f;
                _col.enabled = true;
            }
        }

        if (_changeDirectionCooldown < 0f)
        {
            if (!_changingDirection)
            {
                _changingDirection = true;
                StartCoroutine( ChangeDirection());
            }
        }
        _changeDirectionCooldown -= Time.deltaTime;
    }


    void CheckPlayerDistance()
    {
        if(_playerList.Count <1)
        {
            var temp = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject go in temp) _playerList.Add(go.transform);
        }

        _playerList.Sort(distanceComparer);

    }

    void Movement()
    {
        if (_currentSpeed < _speed) _currentSpeed += 2 *Time.deltaTime;
        _currDirection = (_targetDirection.position - transform.position).normalized;
        this.transform.position += (_currDirection * _currentSpeed) * Time.deltaTime;
        _sprite.flipX = _currDirection.x < 0.0f;

        if(_currDirection.y >0f) _sprite.sprite = _up;
        else if (Mathf.Abs(_currDirection.x) > 0.6f) _sprite.sprite = _side;
        else if (Mathf.Abs(_currDirection.x) > 0.3f) _sprite.sprite = _diagonal;
        else _sprite.sprite = _down;
    }

    IEnumerator ChangeDirection()
    {
        newTarget = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 0.1f),0f);
        float xdif = newTarget.x - _targetDirection.localPosition.x;
        float ydif = newTarget.y - _targetDirection.localPosition.y;
        float randomCooldown = Random.Range(0.5f, 2f);

        while(randomCooldown>0f)
        {

               _targetDirection.localPosition = new Vector3(_targetDirection.localPosition.x + xdif *Time.deltaTime, _targetDirection.localPosition.y + ydif * Time.deltaTime, 0f);
            if (_targetDirection.localPosition.y >= 0.0f) ydif = 0f;
             randomCooldown -= Time.deltaTime;
             yield return new WaitForSeconds(Time.deltaTime);


        }
        _changeDirectionCooldown = Random.Range(0.5f, 2f);
        _changingDirection = false ;

        yield return 0;

        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") && collision.gameObject != _currObstacle)
        {
            _currObstacle = collision.gameObject;
            _crashed = true;
            _crashDuration = 1.5f;
            _sprite.sprite = _crash;

        }

        if(collision.CompareTag("Player"))
        {
            _crashed = true;
            _crashDuration = 2.5f;
            _sprite.sprite = _crash;
            _col.enabled = false;

        }
    }
}
