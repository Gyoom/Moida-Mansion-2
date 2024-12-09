using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] bool active = false;
    [SerializeField] private GameObject mainRoom;

    [Header("Intro")]
    [SerializeField] private GameObject introRooms;
    [SerializeField] private float atlasFadeDuration = 1f;
    [SerializeField] private float blinkSpeed = 0.5f;
    [SerializeField] private float monsterSpeed = 1f;
    [SerializeField] private List<GameObject> monster;

    private HUDManager hud;
    private bool loop = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        if (!active) yield break;

        mainRoom.SetActive(false);

        PlayerController.instance.OnStartGeneration += onStartGame;

        hud = HUDManager.Instance;
        
        hud.atlas.SetActive(true);

        yield return StartCoroutine(AtlasFading());

        hud.DisplayStaticText("BEWARE OF", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText("MOIDA MANSION", 2f, childs.none);
        introRooms.SetActive(true);
        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText("BY", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        hud.DisplayScrollingText("GUILL - PIERRE - ALOIS - \t", 3f, childs.none);
        yield return new WaitForSeconds(3f);

        hud.DisplayStaticText("1.0.0", 2f, childs.none);
        yield return new WaitForSeconds(2f);

        hud.DisplayScrollingText("RESCUE YOUR FRIENDS!     \t", -1f, childs.none);
        
        StartCoroutine(MonsterDisplay());
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
        hud.scrollingText.SetActive(false);
        introRooms.SetActive(false);
        mainRoom.SetActive(true);
        loop = false;
    }
}
