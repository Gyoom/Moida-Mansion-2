using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ScrollingText : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 30f)]
    public float scrollSpeed = 2;

    [SerializeField]
    [Min(1)]
    int _maxClones = 15;

    [SerializeField]
    float moveDelay;

    float _textPreferredWidth;
    readonly LinkedList<RectTransform> _textTransforms = new();

    public TextMeshProUGUI TextMesh => _textTransforms.First.Value.GetComponent<TextMeshProUGUI>();

    float moveTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _textTransforms.AddFirst((RectTransform)transform.GetChild(0));
        _textPreferredWidth = _textTransforms.First.Value.GetComponent<TextMeshProUGUI>().preferredWidth;

        CreateClones();
    }

    // Update is called once per frame
    void Update()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveDelay)
        {
            Debug.Log("1");
            MoveTransforms();
            moveTimer = 0;
        }
    }

    public void UpdateClones() { 
        RectTransform firstTrans = _textTransforms.First.Value;
        _textTransforms.RemoveFirst();

        foreach (RectTransform rectTrans in _textTransforms) { 
            Destroy(rectTrans.gameObject);
        }
        _textTransforms.Clear();

        _textTransforms.AddFirst(firstTrans);
        _textPreferredWidth = firstTrans.GetComponent<TextMeshProUGUI>().preferredWidth;
        CreateClones();
    }

    void MoveTransforms()
    { 
        float distance = scrollSpeed * 1200 * Time.deltaTime;

        foreach (RectTransform transform in _textTransforms) { 
            Vector3 newPos = transform.localPosition;
            newPos.x -= distance;

            transform.localPosition = newPos;
        }

        CheckIfLeftMostTransformLeftMask();
    }

    void CheckIfLeftMostTransformLeftMask() { 
        RectTransform rectTransform = _textTransforms.First.Value;


        if (rectTransform.localPosition.x + _textPreferredWidth <= 0)
        {
            ReAttachFirstTransformAtTheEnd();
        }
    }

    void CreateClones() {
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
