using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventFeedScript : MonoBehaviour
{
    [SerializeField] TMP_Text _textElement;
    [SerializeField] Image _interactionBackground;

    float _currTime = 0f;
    float _maxTime = 5f;

    public void InitParams(string text)
    {
        _textElement.text = text;

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponentInParent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    // Update is called once per frame
    void Update()
    {
        if (_currTime > _maxTime)
            Destroy(gameObject);

        _currTime += Time.deltaTime;

        _textElement.color = new Color(_textElement.color.r, _textElement.color.g, _textElement.color.b, (_maxTime - _currTime) / _maxTime);

        _interactionBackground.color = new Color(_interactionBackground.color.r, _interactionBackground.color.g, _interactionBackground.color.b, (_maxTime - _currTime) / (_maxTime * 2f));
    }
}
