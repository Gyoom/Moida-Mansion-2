using System;
using System.Collections;
using System.Collections.Generic;
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

    [Header("Outro")]
    public Action OnOutroFinish;

    private HUDManager hud;
    private bool loop = true;

    private void Awake() { 
        Instance = this;
    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Intro
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    
    IEnumerator Start()
    {
        if (!active) yield break;

        PlayerController.instance.canInput = false;

        mainRoom.SetActive(false);

        PlayerController.instance.OnStartGeneration += onStartGame;

        hud = HUDManager.Instance;
        
        hud.atlas.SetActive(true);

        yield return StartCoroutine(AtlasFading());

        hud.DisplayStaticText("BEWARE OF", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText("MOIDA MANSION", 2f, childs.none);
        introRooms.SetActive(true);
        StartCoroutine(MonsterDisplay());
        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText("BY", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        hud.DisplayScrollingText("GUILL - PIERRE - ALOIS - \t", 3f, childs.none);
        yield return new WaitForSeconds(3f);

        hud.DisplayStaticText("1.0.0", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        PlayerController.instance.canInput = true;

        hud.DisplayScrollingText("RESCUE YOUR FRIENDS!     \t", -1f, childs.none);
        
        StartCoroutine(BlinkHUD());
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
        hud.scrollingText.SetActive(false);
        introRooms.SetActive(false);
        mainRoom.SetActive(true);

    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /// Outro
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public IEnumerator ExitMansion() {
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
        StartCoroutine(ArrowBlink());
        OnOutroFinish?.Invoke();


    }

    IEnumerator InventoryBlink(float totalDuration) {
        float blinkDuration = totalDuration / 6;

        for (int i = 0; i < 6; i++)
        {
            hud.inventory.SetActive(!hud.inventory.activeSelf);
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    IEnumerator ArrowBlink()
    {
        do
        {
            hud.arrowRight.SetActive(!hud.arrowRight.activeSelf);

            yield return new WaitForSeconds(blinkSpeed);
        } while (loop);

    }
}
