using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBehaviour : MonoBehaviour
{
    BoxCollider2D _boxCollider;
    SpriteRenderer _spriteRender;

    public bool hasMultiSprite = false;
    public Sprite hitSprite;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRender = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //
    public void PlayerHit()
    {
        if (hasMultiSprite)
            _spriteRender.sprite = hitSprite;
    }

}
