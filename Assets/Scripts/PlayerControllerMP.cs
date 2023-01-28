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
    public Sprite[] sprites;

    [Space(2)]
    Sprite spriteNormalRight;
    Sprite spriteNormalDownRight;
    Sprite spriteNormalTopRight;
    Sprite spriteNormalDown;
    Sprite spriteNormalTop;
    Sprite spriteJump1;
    Sprite spriteJump2;
    Sprite spriteFumble;

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
        int pIndex = playerInput.playerIndex;
        _boxCollider = GetComponent<BoxCollider2D>();
        Debug.Log(playerInput.currentControlScheme + " joined");
        transform.position += new Vector3(pIndex * 3f, 0);
        GameControllerMultiplayer.instance.AddPlayerTransform(transform);

        if(playerInput.currentControlScheme == "Mouse")
        {
            isMouse = true;
        }

        spriteNormalRight = sprites[(pIndex) * 8 + 0];
        spriteNormalDownRight = sprites[(pIndex) * 8 + 1];
        spriteNormalTopRight = sprites[(pIndex) * 8 + 2];
        spriteNormalDown = sprites[(pIndex) * 8 + 3];
        spriteNormalTop = sprites[(pIndex) * 8 + 4];
        spriteJump1 = sprites[(pIndex) * 8 + 5];
        spriteJump2 = sprites[(pIndex) * 8 + 6];
        spriteFumble = sprites[(pIndex) * 8 + 7];
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
            _currJumpDura += Time.deltaTime;
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
                    spriteRender.sprite = spriteNormalTopRight;
                else
                    spriteRender.sprite = spriteNormalTop; ;
            }
            else if (movementInput.y < -0.25f)
            {
                if (movementInput.x > 0.25f || movementInput.x < -0.25f)
                    spriteRender.sprite = spriteNormalDownRight;
                else
                    spriteRender.sprite = spriteNormalDown;
            }
            else
            {
                spriteRender.sprite = spriteNormalRight;
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
        spriteRender.sprite = spriteFumble;

        yield return new WaitForSeconds(val);

        _inputActive = true;

        yield return null;
    }

    IEnumerator RampJump()
    {       
        _isJumping = true;
        _inputActive = false;

        spriteRender.sprite = spriteJump1;
        yield return new WaitForSeconds(0.5f);

        spriteRender.sprite = spriteJump2;
        yield return new WaitForSeconds(0.5f);

        _isJumping = false;
        _inputActive = true;
        _currJumpDura = 0f;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle") && collision.gameObject != _currObstacle)
        {
            _currObstacle = collision.gameObject;
            HitObstacle();
        }

        if(collision.CompareTag("Ramp"))
        {
            StartCoroutine(RampJump());
        }
    }
}
