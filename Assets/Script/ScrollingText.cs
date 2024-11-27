using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScrollingText : MonoBehaviour
{
    [Range(0f, 30f)]
    public float scrollSpeed = 2;

    [SerializeField]
    [Min(1)]
    int _maxClones = 15;

    float _textPreferredWidth;
    readonly LinkedList<RectTransform> _textTransforms = new();



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _textTransforms.AddFirst((RectTransform)transform.GetChild(0));
        _textPreferredWidth = _textTransforms.First.Value.GetComponent<TextMeshProUGUI>().preferredWidth;

        CreateClone();
    }

    // Update is called once per frame
    void Update()
    {
        MoveTransforms();
    }

    void MoveTransforms()
    { 
        float distance = scrollSpeed * 30 * Time.deltaTime;

        foreach (RectTransform transform in _textTransforms) { 
            Vector3 newPos = transform.localPosition;
            newPos.x -= distance;

            transform.localPosition = newPos;
        }

        CheckIfLeftMostTransformLeftMask();
    }

    void CheckIfLeftMostTransformLeftMask() { 
        RectTransform rectTransform = _textTransforms.First.Value;

        Debug.Log(rectTransform.localPosition.x);

        if (rectTransform.localPosition.x + _textPreferredWidth <= 0)
        {
            ReAttachFirstTransformAtTheEnd();
        }
    }

    void CreateClone() {
        int clones = CalculateNecessaryClones();

        for (int i = 1; i <= clones; i++) {
            RectTransform cloneTransform = Instantiate(_textTransforms.First.Value, transform);
            AttachTransformAtTheEnd(cloneTransform);
            _textTransforms.AddLast(cloneTransform);
        }
    }

    int CalculateNecessaryClones() {
        int clones = 0;
        RectTransform maskTransform = GetComponent<RectTransform>();

        do {
            clones++;
            if (clones == _maxClones)
            {
                Debug.LogWarning($"Scrolling Text stopped after { _maxClones } clones, increase the limit if necessary");
            }

        } while (maskTransform.rect.width / (_textPreferredWidth * clones) >= 1);

        return clones;
    }

    void AttachTransformAtTheEnd(RectTransform rectTransform) { 
        float lastTransPosX = _textTransforms.Last.Value.localPosition.x;

        Vector3 newPos = rectTransform.localPosition;
        newPos.x = lastTransPosX + _textPreferredWidth;

        rectTransform.localPosition = newPos;
    }

    void ReAttachFirstTransformAtTheEnd() {
        float lastTransPosX = _textTransforms.Last.Value.localPosition.x;

        LinkedListNode<RectTransform> node = _textTransforms.First;

        Vector3 newPos = node.Value.localPosition;
        newPos.x = lastTransPosX + _textPreferredWidth;
        node.Value.localPosition = newPos;

        _textTransforms.RemoveFirst();
        _textTransforms.AddLast(node);

    }
}
