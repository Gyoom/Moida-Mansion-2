using Script;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CinematicManager : MonoBehaviour
{
    public static CinematicManager Instance;

    [SerializeField] bool active = false;
    [SerializeField] private GameObject mainRoom;

    [Header("Intro")]
    [SerializeField] private GameObject introRooms;
    [SerializeField] private float atlasFadeDuration = 1f;
    [SerializeField] private float blinkSpeed = 0.5f;
    [SerializeField] private float monsterSpeed = 1f;
    [SerializeField] private List<GameObject> monster;

    [Header("Dot")]
    [SerializeField] private GameObject dotRoom;
    [SerializeField] private GameObject dotRoomStep1;
    [SerializeField] private GameObject dotRoomStep2;
    [SerializeField] private GameObject dotRoomStep3;

    [Header("Outro - win")]
    public Action OnOutroFinish;

    private HUDManager hud;
    private bool loop = true;
    // coroutine
    private IEnumerator IntroCoroutine;
    private IEnumerator IntroAtlasCoroutine;
    private IEnumerator IntroMonsterCoroutine;
    private IEnumerator IntroBlinkCoroutine;

    [Header("Outro - dead")]
    [SerializeField] private GameObject monsterRoom;
    [SerializeField] private GameObject hand;
    [SerializeField] private GameObject blood;

    private void Awake() { 
        Instance = this;
    }

    void Start() {
        IntroCoroutine = Intro();
        IntroAtlasCoroutine = AtlasFading();
        IntroMonsterCoroutine = MonsterDisplay();
        IntroBlinkCoroutine = BlinkHUD();

        Monster.Instance.OnMonsterKilling = PlayerDeath;

        StartCoroutine(IntroCoroutine);
    }

    public IEnumerator Blink(GameObject toBlink, float totalDuration)
    {
        float blinkDuration = totalDuration / 6;

        for (int i = 0; i < 6; i++)
        {
            toBlink.SetActive(!toBlink.activeSelf);
            yield return new WaitForSeconds(blinkDuration);
        }

    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Intro
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    IEnumerator Intro()
    {

        if (!active) yield break;

        mainRoom.SetActive(false);

        PlayerController.instance.OnStartGeneration += onStartGame;

        hud = HUDManager.Instance;
        
        hud.atlas.SetActive(true);

        yield return StartCoroutine(IntroAtlasCoroutine);

        hud.DisplayStaticText("BEWARE OF", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText("MOIDA MANSION", 2f, childs.none);
        introRooms.SetActive(true);
        StartCoroutine(IntroMonsterCoroutine);
        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText("BY", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        hud.DisplayScrollingText("GUILL - PIERRE - ALOIS - \t", 3f, childs.none);
        yield return new WaitForSeconds(3f);

        hud.DisplayStaticText("1.0.0", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        hud.DisplayScrollingText("RESCUE YOUR FRIENDS!     \t", -1f, childs.none);
        
        StartCoroutine(IntroBlinkCoroutine);
    }

    IEnumerator AtlasFading()
    {

        yield return new WaitForSeconds(1f);

        float time = 0;
        Color currentColor = hud.atlas.GetComponent<SpriteRenderer>().color;
        float startAlpha = currentColor.a;

        while (time < atlasFadeDuration)
        {

            currentColor.a = Mathf.Lerp(startAlpha, 0, time / atlasFadeDuration);
            hud.atlas.GetComponent<SpriteRenderer>().color = currentColor;

            time += Time.deltaTime;
            yield return null;
        }
        hud.atlas.SetActive(false);
    }

    IEnumerator MonsterDisplay()
    {
        yield return new WaitForSeconds(monsterSpeed);
        int previousIndex = 0;
        bool display = true;
        do
        {
            if (display)
            {
                monster[previousIndex].SetActive(false);
                display = false;
            }
            else {
                if (previousIndex == 0)
                {
                    monster[1].SetActive(true);
                    previousIndex = 1;
                }
                else 
                {
                    monster[0].SetActive(true);
                    previousIndex = 0;
                }
                display = true;
            }

            yield return new WaitForSeconds(monsterSpeed);
        } while (loop);
    }

    IEnumerator BlinkHUD()
    {
        do
        {
            hud.upStairs.SetActive(!hud.upStairs.activeSelf);

            yield return new WaitForSeconds(blinkSpeed);
        } while (loop);

    }

    private void onStartGame() {

        loop = false;
        StopCoroutine(IntroAtlasCoroutine);
        StopCoroutine(IntroMonsterCoroutine);
        StopCoroutine(IntroBlinkCoroutine);
        StopCoroutine(IntroCoroutine);

        hud.atlas.SetActive(false);
        hud.StopDisplayScrollingText();
        hud.StopDisplayStaticText();
        introRooms.SetActive(false);
        mainRoom.SetActive(true);

        hud.search.SetActive(true);

    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Dot
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void FoundDot()
    {
        StartCoroutine(DotCinematic());
    }
    private IEnumerator DotCinematic() { 
        PlayerController.instance.canInput = false;
        mainRoom.SetActive(false);
        dotRoom.SetActive(true);
        dotRoomStep1.SetActive(true);
        PlayerController.instance.canInput = true;

        // Room blink
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(Blink(dotRoomStep1, 1f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Blink(dotRoomStep1, 1f));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(Blink(dotRoomStep1, 1f));

        dotRoomStep1.SetActive(false);
        dotRoomStep2.SetActive(true);
        yield return new WaitForSeconds(1f);
        // display dot
        dotRoomStep3.SetActive(true);
        yield return StartCoroutine(Blink(dotRoomStep3, 3f));

        hud.DisplayStaticText("IT'S DOT!", 3f, childs.none);
        yield return new WaitForSeconds(3f);

        // return to gameloop
        hud.hasDot(true);
        dotRoom.SetActive(false);
        mainRoom.SetActive(true);

        yield return StartCoroutine(Blink(hud.dot, 2f));

        hud.DisplayScrollingText("LET'S GET OUT OF HERE!    \t", 6f, childs.none);

    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Outro - Victory
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void ExitMansion()
    {
        StartCoroutine(OutroCinematic());
    }

    public IEnumerator OutroCinematic() {
        PlayerController.instance.canInput = false;

        hud.map.SetActive(false);
        hud.key.SetActive(false);
        hud.codeParent.SetActive(false);
        hud.arrowLeft.SetActive(false);
        hud.arrowRight.SetActive(false);
        hud.search.SetActive(false);
        hud.downStairs.SetActive(false);
        hud.upStairs.SetActive(false);

        hud.bek.SetActive(true);
        hud.cal.SetActive(true);
        hud.ace.SetActive(true);
        hud.dot.SetActive(true);


        mainRoom.SetActive(false);
        introRooms.SetActive(true);

        hud.DisplayStaticText("YOU ESCAPED !", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText("RESCUED ALL!", 3f, childs.none);
        yield return StartCoroutine(InventoryBlink(3f));
        

        hud.DisplayStaticText(
             "MOVES : " + PlayerController.instance.stepAmount, 
             2f, 
             childs.none
        );
        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText(
             "SEARCHES : " + PlayerController.instance.searchAmount,
             2f,
             childs.none
        );
        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText(
             "ATTACKED : ??",
             2f,
             childs.none
        );
        yield return new WaitForSeconds(2f);

        hud.inventory.SetActive(false);

        hud.DisplayStaticText("CONGRATS!", -1f, childs.none);
        loop = true;
        StartCoroutine(ArrowBlink(blinkSpeed));
        OnOutroFinish?.Invoke();

        yield return new WaitForSeconds(2f);

        Script.Procedural_Generation.Room[,] rooms = MansionManager.Instance.MansionMatrix;
        introRooms.SetActive(false);
        mainRoom.SetActive(true);

        while (true) {

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 3; i++)
                {

                    rooms[j, i].DisplayRoom();
                    yield return new WaitForSeconds(5f);
                    rooms[j, i].HideRoom();
                }
            }
           
        }


    }

    IEnumerator InventoryBlink(float totalDuration) {
        float blinkDuration = totalDuration / 6;

        for (int i = 0; i < 6; i++)
        {
            hud.inventory.SetActive(!hud.inventory.activeSelf);
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    IEnumerator ArrowBlink(float delay)
    {
        do
        {
            hud.arrowRight.SetActive(!hud.arrowRight.activeSelf);

            yield return new WaitForSeconds(delay);
        } while (loop);

    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Outro - Death
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void PlayerDeath() {
        StartCoroutine(DeathCinematic());
    }

    private IEnumerator DeathCinematic() {
        PlayerController.instance.canInput = false;
        mainRoom.SetActive(false);
        monsterRoom.SetActive(true);
        hand.SetActive(true);
        // remove hud
        hud.StopDisplayScrollingText();
        hud.StopDisplayStaticText();
        hud.inventory.SetActive(false);
        hud.map.SetActive(false);
        hud.key.SetActive(false);
        hud.codeParent.SetActive(false);
        hud.arrowLeft.SetActive(false);
        hud.upStairs.SetActive(false);
        hud.search.SetActive(false);
        hud.downStairs.SetActive(false);
        hud.arrowRight.SetActive(false);
        // displaying
        yield return StartCoroutine(Blink(hand, 1f));
        yield return StartCoroutine(Blink(hand, 1f));
        yield return StartCoroutine(Blink(hand, 1f));

        monsterRoom.SetActive(false);
        blood.SetActive(true);

        StartCoroutine(Blink(blood, 1f));
        yield return StartCoroutine(Blink(hand, 1f));
        StartCoroutine(Blink(blood, 1f));
        yield return StartCoroutine(Blink(hand, 1f));
        StartCoroutine(Blink(blood, 1f));
        yield return StartCoroutine(Blink(hand, 1f));

        yield return new WaitForSeconds(1f);
        hud.DisplayStaticText("YOU'VE", 2f, childs.none);
        yield return new WaitForSeconds(2f);
        hud.DisplayStaticText("BEEN", 2f, childs.none);
        yield return new WaitForSeconds(2f);
        hud.DisplayStaticText("MOIDA'D", 10f, childs.none);
        yield return StartCoroutine(Blink(hud.staticText, 1f));
        yield return StartCoroutine(Blink(hud.staticText, 1f));
        yield return StartCoroutine(Blink(hud.staticText, 1f));
        yield return new WaitForSeconds(1f);

        hud.DisplayStaticText("TRY AGAIN!", -1f, childs.none);
        yield return new WaitForSeconds(1f);

        hud.arrowRight.SetActive(true);
        PlayerController.instance.canInput = true;
        loop = true;
        StartCoroutine(ArrowBlink(0.3f));
    }
}
