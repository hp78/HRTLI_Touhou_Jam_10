using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionFeedScript : MonoBehaviour
{
    [SerializeField] TMP_Text _leftTextElement;
    [SerializeField] TMP_Text _rightTextElement;
    [SerializeField] Image _interactionIcon;
    [SerializeField] Image _interactionBackground;

    float _currTime = 0f;
    float _maxTime = 5f;

    public void InitParams(string leftText, string rightText, Color leftColor, Color rightColor, Sprite iconSprite)
    {
        _leftTextElement.text = leftText;
        _leftTextElement.color = leftColor;

        _rightTextElement.text = rightText;
        _rightTextElement.color = rightColor;

        _interactionIcon.sprite = iconSprite;

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponentInParent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    // Update is called once per frame
    void Update()
    {
        if (_currTime > _maxTime)
            Destroy(gameObject);

        _currTime += Time.deltaTime;

        _leftTextElement.color = new Color(_leftTextElement.color.r, _leftTextElement.color.g, _leftTextElement.color.b, (_maxTime - _currTime) / _maxTime);
        _rightTextElement.color = new Color(_rightTextElement.color.r, _rightTextElement.color.g, _rightTextElement.color.b, (_maxTime - _currTime) / _maxTime);
        _interactionIcon.color = new Color(_interactionIcon.color.r, _interactionIcon.color.g, _interactionIcon.color.b, (_maxTime - _currTime) / _maxTime);

        _interactionBackground.color = new Color(_interactionBackground.color.r, _interactionBackground.color.g, _interactionBackground.color.b, (_maxTime - _currTime) / (_maxTime * 2f));
    }
}
