using Script;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private ScrollingText scrollingTextScript;
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

    void Start()
    {
        // initial state
        //atlas.SetActive(true);

        /*map.SetActive(false);
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
        cal.SetActive(false);*/



        StartCoroutine(ToOutside());
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            StartCoroutine(ToMansion());
        }
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(TransitionToLeft());
        }
    }


    // Game state Transitions

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

    IEnumerator ToMansion()
    {

        yield return new WaitForSeconds(0.1f);

        UpdateMap(true, debugPos);
        search.SetActive(true);

        gameState = GameState.Mansion;
    }

    // HUD Update 

    void UpdateMap(bool state, Vector2 pos) {

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

    // Room change update

    IEnumerator TransitionToLeft()
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
}

enum GameState
{
    Starting,
    Outside,
    Mansion,
    Ending
}
