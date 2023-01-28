using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Space(5)]
    BoxCollider2D _boxCollider;

    [Space(5)]
    [SerializeField] float _accelRate = 0.5f;
    [SerializeField] float _brakeRate = -1f;
    [SerializeField] float _decayRate = -0.1f;
    [SerializeField] public float _maxSpeed = 5f;
    [SerializeField] float _turnRate = 5f;

    [Space(5)]
    [SerializeField] float _currSpeed = 0f;
    [SerializeField] Vector3 _currDirection = Vector2.zero + Vector2.down;

    [Space(5)]
    [SerializeField] bool _inputActive = true;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateMovement();
    }

    void UpdateInput()
    {
        if (!_inputActive)
            return;

        Vector3 inputDirection = Vector2.zero;
        float inputAccel = _decayRate;

        if(Input.GetKey(KeyCode.DownArrow))
        {
            inputDirection += Vector3.down;
            inputAccel += _accelRate;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            inputDirection += Vector3.up;
            inputAccel += _brakeRate;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            inputDirection += Vector3.left;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            inputDirection += Vector3.right;
        }

        _currSpeed = Mathf.Clamp(_currSpeed + inputAccel * Time.deltaTime, 0, _maxSpeed);
        
        if(inputDirection != Vector3.zero)
        {
            _currDirection += inputDirection * Time.deltaTime * _turnRate;
            _currDirection.Normalize();
        } 
    }

    void UpdateMovement()
    {
        if(_currDirection != Vector3.zero)
            transform.position += (_currDirection * _currSpeed) * Time.deltaTime;
    }

    IEnumerator DisableCollisionForSeconds(float val)
    {
        _boxCollider.enabled = false;

        yield return new WaitForSeconds(val);

        _boxCollider.enabled = true;
    }

    IEnumerator DisableInputForSeconds(float val)
    {
        _inputActive = false;

        yield return new WaitForSeconds(val);

        _inputActive = true;

        yield return null;
    }

    void HitObstacle()
    {
        _currSpeed = 0f;
        StartCoroutine(DisableCollisionForSeconds(1));
        StartCoroutine(DisableInputForSeconds(0.5f));

        // change sprite
    }

    void GetEaten()
    {
        // lol die
    }

    void RampJump()
    {
        StartCoroutine(DisableCollisionForSeconds(1));

        // change sprite
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            HitObstacle();
            collision.gameObject.GetComponent<ObstacleBehaviour>().DisableCollider();
        }
    }
}
