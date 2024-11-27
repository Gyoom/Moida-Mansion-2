using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField]
    private GameObject console;
    [SerializeField]
    private GameObject atlas;
    [SerializeField] 
    private float fadeDuration = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DisableAtlas());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DisableAtlas() {
        atlas.SetActive(true);

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
    }
}
