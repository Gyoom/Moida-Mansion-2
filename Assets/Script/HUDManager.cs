using Script;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    [SerializeField] private GameObject console;
    
    [Header("Starting")]

    [SerializeField] private GameObject atlas;

    [SerializeField] private float fadeDuration = 1f;
    
    [Header("Mansion - HUD")]
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject key;
    [SerializeField] private GameObject codeParent;
    [SerializeField] private GameObject arrowLeft;
    [SerializeField] private GameObject upStairs;
    [SerializeField] private GameObject search;
    [SerializeField] private GameObject downStairs;
    [SerializeField] private GameObject arrowRight;
    [SerializeField] private GameObject dot;
    [SerializeField] private GameObject ace;
    [SerializeField] private GameObject bek;
    [SerializeField] private GameObject cal;

    [Header("Texting")]
    [SerializeField] private GameObject scrollingText;
    [SerializeField] private GameObject staticText;

    [Header("Backgrounds")]
    [SerializeField] private GameObject transitionX;
    private float XposLeft = -12.18f;
    private float XCenter = -6.05f;
    private float XposRight = 0.1f;
    [SerializeField] private GameObject transitionY;
    private float YposTop = 5.54f;
    private float YCenter = 1.06f;
    private float YposDown = -3.376f;
    [SerializeField] private float transitionSpeed = 1;
    [SerializeField] private float transitionDelay = 0.3f;

    [Header("Debug")]
    [SerializeField] private Vector2 debugPos;
    [SerializeField] private GameState gameState = GameState.Starting;

    private void Awake()
    {
        Instance = this;
    }

    IEnumerator Start()
    {
        // initial state
        atlas.SetActive(true);

        
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
        
        foreach (Transform d in ace.transform) { 
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
        
        
        yield return StartCoroutine(ToOutside());
        

        ace.SetActive(true);
        bek.SetActive(true);
        cal.SetActive(true);

    }


    void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            DisplayScrollingText("Hello    \t", 10, childs.bek);
            //DisplayStaticText("Hello    \t", 15);
        }
    }


    // Game state Transitions ------------------------------------------------------------------------

    IEnumerator ToOutside() {
 
        yield return new WaitForSeconds(1f);

        float time = 0;
        Color currentColor = atlas.GetComponent<SpriteRenderer>().color;
        float startAlpha = currentColor.a;

        while (time < fadeDuration)
        {

            currentColor.a = Mathf.Lerp(startAlpha, 0, time / fadeDuration);
            atlas.GetComponent<SpriteRenderer>().color = currentColor;

            time += Time.deltaTime;
            yield return null;
        }
        atlas.SetActive(false);

        gameState = GameState.Outside;
    }

    // HUD Update ---------------------------------------------------------------------------------------------

    public void UpdateMap(bool state, Vector2 pos) {

            if (state)
            {
                map.SetActive(true);

                for (int y = 0; y < 3; y++) {
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
            else { 
                map.SetActive(false);
            }
    }

    public void updateInputs() {
        Script.Procedural_Generation.Room r = MansionManager.Instance.CurrentPlayerRoom();

        if (r.HasLeftDoor) {
            arrowLeft.SetActive(true);
        }
        else {
            arrowLeft.SetActive(false);
        }

        if (r.HasRightDoor)
        {
            arrowRight.SetActive(true);
        }
        else
        {
            arrowRight.SetActive(false);
        }

        if (r.HasStairsDown)
        {
            downStairs.SetActive(true);
        }
        else
        {
            downStairs.SetActive(false);
        }

        if (r.HasStairsUp)
        {
            upStairs.SetActive(true);
        }
        else
        {
            upStairs.SetActive(false);
        }
    }

    public void addCode() {
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

    private IEnumerator stopScrolling(GameObject text, float duration, childs child)
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

    public void DisplayScrollingText(string text, float duration, childs child) {
        scrollingText.SetActive(true);
        scrollingText.GetComponent<ScrollingText>().UpdateClones(text);
  
        StartCoroutine(stopScrolling(scrollingText, duration, child));

        GameObject childObject = GetChildObject(child);
        if (childObject != null) {
            for (int i = 0; i < childObject.transform.childCount; i++)
            {
                childObject.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void DisplayStaticText(string text, float duration, childs child)
    {
        staticText.SetActive(true);
        staticText.GetComponent<TextMeshProUGUI>().text = text;
        StartCoroutine(stopScrolling(staticText, duration, child));
    }

    // Room change update -----------------------------------------------------------------------------------

    IEnumerator Transition()
    {

        Vector3 pos = transitionX.transform.localPosition;
        float startX = XposRight;
        float endX = XCenter;

        float distance = endX - startX;

   
        float deplacement = transitionSpeed;
        bool loop = true;
        float timer = 0;
        float moved = 0;

        pos.x = startX;
        transitionX.transform.localPosition = pos;

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
                    pos.x = endX;
                    loop = false;
                }

                transitionX.transform.localPosition = pos;
            }
            yield return null;
        }

        // call change room function

        loop = true;
        moved = 0;
        startX = endX;
        endX = XposLeft;

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
                    pos.x = endX;
                    loop = false;
                }

                transitionX.transform.localPosition = pos;
            }
            yield return null;
        }
    }

    private GameObject GetChildObject(childs child)
    {
        if (child == childs.bek) {
            return bek;
        }

        if (child == childs.ace) {
            return ace;
        }

        if (child == childs.cal) {
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
