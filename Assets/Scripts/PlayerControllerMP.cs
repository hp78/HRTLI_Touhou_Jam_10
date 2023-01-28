using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerControllerMP : MonoBehaviour
{
    public PlayerInput playerInput;
    bool isMouse = false;

    [Space(5)]
    BoxCollider2D _boxCollider;
    public SpriteRenderer spriteRender;

    [Space(5)]
    public Sprite lunaNormalDown;
    public Sprite lunaNormalDownRight;
    public Sprite lunaNormalRight;
    public Sprite lunaNormalTopRight;
    public Sprite lunaNormalTop;
    [Space(2)]
    public Sprite lunaJump1;
    public Sprite lunaJump2;
    public Sprite lunaFumble;

    [Space(5)]
    [SerializeField] float _accelRate = 0.5f;
    [SerializeField] float _brakeRate = -1f;
    [SerializeField] float _decayRate = -0.1f;
    [SerializeField] public float _maxSpeed = 5f;
    [SerializeField] float _turnRate = 5f;

    [Space(5)]
    [SerializeField] float _currSpeed = 0f;
    [SerializeField] Vector3 _currDirection = Vector2.zero + Vector2.down;
    Vector2 movementInput;

    [Space(5)]
    [SerializeField] bool _inputActive = true;
    [SerializeField] bool _isJumping = false;
    float _currJumpDura = 0f;
    GameObject _currObstacle = null;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        Debug.Log(playerInput.currentControlScheme + " joined");
        transform.position += new Vector3(playerInput.playerIndex * 3f, 0);
        GameControllerMultiplayer.instance.AddPlayerTransform(transform);

        if(playerInput.currentControlScheme == "Mouse")
        {
            isMouse = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
        UpdateMovement();
    }

    public void OnMove(InputAction.CallbackContext ctx) 
        => movementInput = ctx.ReadValue<Vector2>();
    void UpdateInput()
    {
        if (!_inputActive)
            return;

        Vector3 inputDirection = _currDirection;
        float inputAccel = _decayRate;

        if (_isJumping)
        {
            inputAccel += _accelRate * 2f;
        }
        else
        {
            inputDirection = movementInput;

            if (movementInput.y > 0.25f)
                inputAccel += _brakeRate;
            else if (movementInput.y < -0.25f)
                inputAccel += _accelRate;
        }

        if (movementInput.x < -0.25f)
        {
            spriteRender.flipX = true;
        }
        else if(movementInput.x > 0.25f)
        {
            spriteRender.flipX = false;
        }

        _currSpeed = Mathf.Clamp(_currSpeed + inputAccel * Time.deltaTime, 0, _maxSpeed);

        if (inputDirection != Vector3.zero)
        {
            if (movementInput.y > 0.25f)
            {
                if (movementInput.x > 0.25f || movementInput.x < -0.25f)
                    spriteRender.sprite = lunaNormalTopRight;
                else
                    spriteRender.sprite = lunaNormalTop;
            }
            else if (movementInput.y < -0.25f)
            {
                if (movementInput.x > 0.25f || movementInput.x < -0.25f)
                    spriteRender.sprite = lunaNormalDownRight;
                else
                    spriteRender.sprite = lunaNormalDown;
            }
            else
            {
                spriteRender.sprite = lunaNormalRight;
            }

            _currDirection += inputDirection * Time.deltaTime * _turnRate;
            _currDirection.Normalize();
        }
    }

    void UpdateMovement()
    {
        if (_currDirection != Vector3.zero)
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
        _isJumping = true;
        _currJumpDura = 0f;

        // change sprite
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") && collision.gameObject != _currObstacle)
        {
            _currObstacle = collision.gameObject;
            HitObstacle();
        }
    }
}
