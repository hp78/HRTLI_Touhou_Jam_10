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
    [Space(5)]
    [SerializeField] float _speed;
    [SerializeField] float _speedModRange;

    [Space(5)]
    [SerializeField] Vector3 _currDirection;
    [SerializeField] Transform _targetDirection;
    [SerializeField] float _directionChangeRate;

    float _changeDirectionCooldown;
    bool _changingDirection;

    public Vector3 newTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

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

    void Movement()
    {
        _currDirection = (_targetDirection.position - transform.position).normalized;
        this.transform.position += (_currDirection * _speed) * Time.deltaTime;
        _sprite.flipX = _currDirection.x < 0.0f;

        if(_currDirection.y >0f) _sprite.sprite = _up;
        else if (Mathf.Abs(_currDirection.x) > 0.6f) _sprite.sprite = _side;
        else if (Mathf.Abs(_currDirection.x) > 0.3f) _sprite.sprite = _diagonal;
        else _sprite.sprite = _down;
    }

    IEnumerator ChangeDirection()
    {
        newTarget = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 0.125f),0f);
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
}
