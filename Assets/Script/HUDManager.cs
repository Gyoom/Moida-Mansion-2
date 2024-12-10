using System;
using Script;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [SerializeField] private GameObject console;

    [Header("Starting")]
    public GameObject atlas;

    [Header("Mansion - HUD")] public GameObject map;
    public GameObject key;
    public GameObject codeParent;
    public GameObject arrowLeft;
    public GameObject upStairs;
    public GameObject search;
    public GameObject downStairs;
    public GameObject arrowRight;
    public GameObject inventory;
    public GameObject dot;
    public GameObject ace;
    public GameObject bek;
    public GameObject cal;

    [Header("Texting")] public GameObject scrollingText;
    public GameObject staticText;

    [Header("Backgrounds")] [SerializeField]
    private GameObject transitionX;

    private float XposLeft = -12.18f;
    private float XCenter = -6.05f;
    private float XposRight = 0.1f;
    [SerializeField] private GameObject transitionY;
    private float YposTop = 5.54f;
    private float YCenter = 1.06f;
    private float YposDown = -3.376f;
    [SerializeField] private float transitionSpeed = 1;
    [SerializeField] private float transitionDelay = 0.3f;

    public Action OnMoveTransition;

    private Coroutine StaticTextCoroutine;
    private Coroutine ScrollingTextCoroutine;

    public List<childs> activeChilds;

    //[Header("Debug")]
    //[SerializeField] private Vector2 debugPos;
    //[SerializeField] private GameState gameState = GameState.Starting;

    private void Awake()
    {
        Instance = this;

        // initial state
        map.SetActive(false);
        key.SetActive(false);
        foreach (Transform code in codeParent.transform)
        {
            code.gameObject.SetActive(false);
        }

        arrowLeft.SetActive(false);
        upStairs.SetActive(false);
        search.SetActive(false);
        downStairs.SetActive(false);
        arrowRight.SetActive(false);
        dot.SetActive(false);

        foreach (Transform d in ace.transform)
        {
            d.gameObject.SetActive(false);
        }

        ace.SetActive(false);

        foreach (Transform d in bek.transform)
        {
            d.gameObject.SetActive(false);
        }

        bek.SetActive(false);

        foreach (Transform d in cal.transform)
        {
            d.gameObject.SetActive(false);
        }

        cal.SetActive(false);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            string[] a = { "AAA", "BBB" };
            CinematicManager.Instance.FoundChild(childs.ace, a);
            //DisplayScrollingText("Hello    \t", 10, childs.bek);
            //DisplayStaticText("Hello    \t", 15);
        }
    }

    // HUD Update ---------------------------------------------------------------------------------------------

    public void UpdateMap(bool state, Vector2 pos)
    {
        if (state)
        {
            map.SetActive(true);

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    int index = y * 4 + x;
                    if (y == pos.y && x == pos.x)
                    {
                        map.transform.GetChild(index).gameObject.SetActive(true);
                    }
                    else
                    {
                        map.transform.GetChild(index).gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            map.SetActive(false);
        }
    }

    public void UpdateInputs(Script.Procedural_Generation.Room currentRoom)
    {
        arrowLeft.SetActive(currentRoom.LeftDoor != null);
        arrowRight.SetActive(currentRoom.RightDoor != null);
        downStairs.SetActive(currentRoom.HasStairsUp);
        upStairs.SetActive(currentRoom.HasStairsDown);
    }

    public void addCode()
    {
        if (!codeParent.transform.GetChild(0).gameObject.activeSelf)
        {
            codeParent.transform.GetChild(0).gameObject.SetActive(true);
        }

        if (!codeParent.transform.GetChild(1).gameObject.activeSelf)
        {
            codeParent.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (!codeParent.transform.GetChild(2).gameObject.activeSelf)
        {
            codeParent.transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public void removeCode()
    {
        if (codeParent.transform.GetChild(2).gameObject.activeSelf)
        {
            codeParent.transform.GetChild(2).gameObject.SetActive(false);
        }

        if (codeParent.transform.GetChild(1).gameObject.activeSelf)
        {
            codeParent.transform.GetChild(1).gameObject.SetActive(false);
        }

        if (codeParent.transform.GetChild(0).gameObject.activeSelf)
        {
            codeParent.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void hasKey(bool has)
    {
        key.SetActive(has);
    }

    // Inventory get/set -----------------------------------------------------------------------------

    public void hasDot(bool has)
    {
        dot.SetActive(has);
    }

    public void hasAce(bool has)
    {
        ace.SetActive(has);
    }

    public void hasBek(bool has)
    {
        bek.SetActive(has);
    }

    public void hasCal(bool has)
    {
        cal.SetActive(has);
    }

    // Texting ---------------------------------------------------------------------------------------
    private IEnumerator removeText(GameObject text, float duration, childs child)
    {
        yield return new WaitForSeconds(duration);

        text.SetActive(false);

        GameObject childObject = GetChildObject(child);
        if (childObject != null)
        {
            for (int i = 0; i < childObject.transform.childCount; i++)
            {
                childObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    // Static
    public void StopDisplayStaticText()
    {
        if (StaticTextCoroutine != null)
            StopCoroutine(StaticTextCoroutine);

        staticText.SetActive(false);
    }

    public void DisplayStaticText(string text, float duration, childs child)
    {
        if(StaticTextCoroutine != null)
            StopCoroutine(StaticTextCoroutine);

        staticText.SetActive(true);
        staticText.GetComponent<TextMeshProUGUI>().text = text;
        if (duration > 0)
            StaticTextCoroutine = StartCoroutine(removeText(staticText, duration, child));
    }

    // Scrolling
    public void StopDisplayScrollingText()
    {
        if (ScrollingTextCoroutine != null)
            StopCoroutine(ScrollingTextCoroutine);
        
        scrollingText.SetActive(false);
    }

    public void DisplayScrollingText(string text, float duration, childs child)
    {
        if (ScrollingTextCoroutine != null)
            StopCoroutine(ScrollingTextCoroutine);

        // active text
        scrollingText.SetActive(true);
        scrollingText.GetComponent<ScrollingText>().UpdateClones(text);

        // program the end of displaying
        if (duration > 0)
            ScrollingTextCoroutine =  StartCoroutine(removeText(scrollingText, duration, child));

        // if child speak, display dots
        GameObject childObject = GetChildObject(child);
        if (childObject != null)
        {
            for (int i = 0; i < childObject.transform.childCount; i++)
            {
                childObject.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    // Static - Searching
    private IEnumerator UpdateSearching(GameObject text, float totalDuration)
    {
        float currentDuration = totalDuration / 10;

        yield return new WaitForSeconds(currentDuration);
        staticText.GetComponent<TextMeshProUGUI>().text = "#EARCHING";

        yield return new WaitForSeconds(currentDuration);
        staticText.GetComponent<TextMeshProUGUI>().text = "##ARCHING";

        yield return new WaitForSeconds(currentDuration);
        staticText.GetComponent<TextMeshProUGUI>().text = "###RCHING";

        yield return new WaitForSeconds(currentDuration);
        staticText.GetComponent<TextMeshProUGUI>().text = "####CHING";

        yield return new WaitForSeconds(currentDuration);
        staticText.GetComponent<TextMeshProUGUI>().text = "#####HING";

        yield return new WaitForSeconds(currentDuration);
        staticText.GetComponent<TextMeshProUGUI>().text = "######ING";

        yield return new WaitForSeconds(currentDuration);
        staticText.GetComponent<TextMeshProUGUI>().text = "#######NG";

        yield return new WaitForSeconds(currentDuration);
        staticText.GetComponent<TextMeshProUGUI>().text = "########G";

        yield return new WaitForSeconds(currentDuration);
        staticText.GetComponent<TextMeshProUGUI>().text = "#########";

        yield return new WaitForSeconds(currentDuration);
        text.SetActive(false);
    }

    public void stopDisplaySearchingText()
    {
        if (StaticTextCoroutine != null)
            StopCoroutine(StaticTextCoroutine);

        staticText.SetActive(false);

    }

    public void DisplaySearchingText(float totalDuration = 5f) {
        if (StaticTextCoroutine != null)
            StopCoroutine(StaticTextCoroutine);

        staticText.SetActive(true);
        staticText.GetComponent<TextMeshProUGUI>().text = "SEARCHING";
        
        StaticTextCoroutine = StartCoroutine(UpdateSearching(staticText, totalDuration));
    }

    // Room change update -----------------------------------------------------------------------------------

    public IEnumerator Transition(Dir dir)
    {
        if (dir == Dir.right || dir == Dir.left)
        {
            yield return StartCoroutine(TransitionX(dir));
        }
        else
        {
            yield return StartCoroutine(TransitionY(dir));
        }
    }

    private IEnumerator TransitionX(Dir dir)
    {
        GameObject transitionScreen = transitionX;
        Vector3 pos = transitionScreen.transform.localPosition;
        float startPosX = 0;
        float centerPosX = XCenter;
        float endPosX = 0;

        if (dir == Dir.right)
        {
            startPosX = XposRight;
            endPosX = XposLeft;
        }
        else
        {
            startPosX = XposLeft;
            endPosX = XposRight;
        }

        float deplacement = transitionSpeed;
        bool loop = true;
        float timer = 0;
        float moved = 0;

        transitionScreen.transform.localPosition = pos;
        float distance = centerPosX - startPosX;
        pos.x = startPosX;
        PlayerController.instance.canInput = false;

        while (loop)
        {
            timer += Time.deltaTime;
            if (timer >= transitionDelay)
            {
                moved += deplacement * Mathf.Sign(distance);
                pos.x += deplacement * Mathf.Sign(distance);
                timer = 0;


                if (Mathf.Abs(moved) >= Mathf.Abs(distance))
                {
                    pos.x = centerPosX;
                    loop = false;
                }

                transitionScreen.transform.localPosition = pos;
            }

            yield return null;
        }

        // call change room function
        OnMoveTransition?.Invoke();
        OnMoveTransition = null;


        loop = true;
        moved = 0;
        distance = endPosX - centerPosX;
        pos.x = centerPosX;

        while (loop)
        {
            timer += Time.deltaTime;
            if (timer >= transitionDelay)
            {
                moved += deplacement * Mathf.Sign(distance);
                pos.x += deplacement * Mathf.Sign(distance);
                timer = 0;


                if (Mathf.Abs(moved) >= Mathf.Abs(distance))
                {
                    pos.x = endPosX;
                    loop = false;
                }

                transitionScreen.transform.localPosition = pos;
            }

            yield return null;
        }

        PlayerController.instance.canInput = true;
    }

    private IEnumerator TransitionY(Dir dir)
    {
        GameObject transitionScreen = transitionY;
        Vector3 pos = transitionScreen.transform.localPosition;
        float startPosY = 0;
        float centerPosY = YCenter;
        float endPosY = 0;

        if (dir == Dir.top)
        {
            startPosY = YposTop;
            endPosY = YposDown;
        }
        else
        {
            startPosY = YposDown;
            endPosY = YposTop;
        }

        float deplacement = transitionSpeed;
        bool loop = true;
        float timer = 0;
        float moved = 0;

        transitionScreen.transform.localPosition = pos;
        float distance = centerPosY - startPosY;
        pos.y = startPosY;

        while (loop)
        {
            timer += Time.deltaTime;
            if (timer >= transitionDelay)
            {
                moved += deplacement * Mathf.Sign(distance);
                pos.y += deplacement * Mathf.Sign(distance);
                timer = 0;


                if (Mathf.Abs(moved) >= Mathf.Abs(distance))
                {
                    pos.y = centerPosY;
                    loop = false;
                }

                transitionScreen.transform.localPosition = pos;
            }

            yield return null;
        }

        // call change room function
        OnMoveTransition?.Invoke();
        OnMoveTransition = null;

        loop = true;
        moved = 0;
        distance = endPosY - centerPosY;
        pos.y = centerPosY;


        while (loop)
        {
            timer += Time.deltaTime;
            if (timer >= transitionDelay)
            {
                moved += deplacement * Mathf.Sign(distance);
                pos.y += deplacement * Mathf.Sign(distance);
                timer = 0;


                if (Mathf.Abs(moved) >= Mathf.Abs(distance))
                {
                    pos.y = endPosY;
                    loop = false;
                }

                transitionScreen.transform.localPosition = pos;
            }

            yield return null;
        }
    }

    private GameObject GetChildObject(childs child)
    {
        if (child == childs.bek)
        {
            return bek;
        }

        if (child == childs.ace)
        {
            return ace;
        }

        if (child == childs.cal)
        {
            return cal;
        }

        return null;
    }
}

public enum GameState
{
    Starting,
    Outside,
    Mansion,
    Ending
}

public enum childs
{
    ace,
    bek,
    cal,
    none
}

public enum Dir
{
    top,
    down,
    left,
    right
}