using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Animator fadePanel;

    private void Awake()
    {
        GameEvents.Instance.onFadeInOut += HazardReset;
    }

    void HazardReset()
    {
        StartCoroutine(FadeInOut());
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onFadeInOut -= HazardReset;
    }

    IEnumerator FadeInOut()
    {
        fadePanel.SetTrigger("fadeOut");
        yield return new WaitForSeconds(0.7f);
        fadePanel.SetTrigger("fadeIn");
    }
}
