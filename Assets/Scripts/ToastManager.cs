using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG;
using DG.Tweening;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour
{
    public GameObject toastObject;
    public TextMeshProUGUI textObject;
    
    public Transform target;
    public GameObject ToastScript;
    public Image sliderImage;
    float t = 0f;
    float duration = 4.5f;
    bool startSlider;

    private void Update()
    {
        if (startSlider)
        {
            if (t <= duration)
            {
                t += Time.deltaTime * 1;
                sliderImage.fillAmount = Mathf.Lerp(1, 0, t / duration);
            }
        }   
    }
    public IEnumerator ShowToast(string s)
    {
        textObject.text = s;
        StartCoroutine(moveToast());
        yield return new WaitForSeconds(1f);
        startSlider = true;

    }

    IEnumerator moveToast()
    {
        yield return toastObject.transform.DOMove(target.position, 1f).WaitForCompletion();
        yield return new WaitForSecondsRealtime(5f);
        yield return toastObject.transform.DOMove(target.position + new Vector3(290, 0, 0), 1f).WaitForCompletion();
        ToastScript.SetActive(false);
        toastObject.SetActive(false);

    }

    private void OnEnable()
    {
        toastObject.SetActive(true);
        StartCoroutine(ShowToast("Coming Soon"));
        startSlider = false;
    }
    private void OnDisable()
    {
        t = 0;
        startSlider = false;
        if (sliderImage.fillAmount == 0f)
        {
            sliderImage.fillAmount = 1f;
            // t = 0;
        }
    }
}