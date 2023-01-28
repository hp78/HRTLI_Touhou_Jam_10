using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChenAiController : MonoBehaviour
{
    [Space(5)]
    [SerializeField] SpriteRenderer _walk;

    [Space(5)]
    [SerializeField] float _speed;
    [SerializeField] float _direction;

    [Space(5)]
    [SerializeField] GameObject _chenSprite;
    [SerializeField] GameObject _bloodSprite;
    [SerializeField] Collider2D _collider;

    [Space(5)]
    [SerializeField] Transform _Ran;
    [SerializeField] Transform _ranSpawnPoint;



    float _changeDirectionCooldown = 0f;
    bool _chendies = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_chendies)
        {
            Movement();
            if (_changeDirectionCooldown < 0f)
            {
                _direction = Random.Range(-1f, 1f);
                _changeDirectionCooldown = Random.Range(0.5f, 3f);
            }

            _changeDirectionCooldown -= Time.deltaTime;
        }
    }

    void Movement()
    {
        this.transform.position += new Vector3((_speed * _direction),
                                                0f, 0f) 
                                                *Time.deltaTime;

        _walk.flipX = _direction < 0.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            ChenFuckingDies();
        }
    }

    void ChenFuckingDies()
    {
        Instantiate(_Ran, _ranSpawnPoint.position, Quaternion.identity);
        _chendies = true;
        _collider.enabled = false;
        _chenSprite.SetActive(false);
        _bloodSprite.SetActive(true);
    }
}
