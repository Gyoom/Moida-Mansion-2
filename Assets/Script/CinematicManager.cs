using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class CinematicManager : MonoBehaviour
{
    [SerializeField] bool active = false;

    [Header("Intro")]
    [SerializeField] private float atlasFadeDuration = 1f;

    private HUDManager hud;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        if (!active) yield break;

        hud = HUDManager.Instance;
        
        hud.atlas.SetActive(true);

        yield return StartCoroutine(AtlasFading());

        hud.DisplayStaticText("BEWARE OF", 2f, childs.none);

        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText("MOIDA MANSION", 2f, childs.none);

        // display intro place

        yield return new WaitForSeconds(2f);

        hud.DisplayStaticText("BY", 2f, childs.none);

        yield return new WaitForSeconds(2f);

        hud.DisplayScrollingText("GUILL - PIERRE - ALOIS - \t", 4f, childs.none);

        yield return new WaitForSeconds(4f);

        hud.DisplayStaticText("1.0.0", 2f, childs.none);

        yield return new WaitForSeconds(2f);

        hud.DisplayScrollingText("RESCUE YOUR FRIENDS!     \t", -1f, childs.none);
        
        hud.upStairs.SetActive(true);
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


}
