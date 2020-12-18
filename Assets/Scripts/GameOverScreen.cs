using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    // Tunables
    float fadeInRate = 0.4f;

    // State
    bool isFading = false;

    private void Start()
    {
        SetChildrenState(false);
    }

    private void Update()
    {
        if (isFading)
        {
            TextMeshProUGUI[] textElements = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI textElement in textElements)
            {
                if (textElement.alpha < 255)
                {
                    textElement.alpha = Mathf.Clamp(textElement.alpha += Time.deltaTime * fadeInRate, 0, 255);
                }
                else
                {
                    isFading = false;
                }
            }
        }
    }

    public void SetChildrenState(bool state)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(state);
        }
    }

    public void FadeInUI()
    {
        if (!isFading)
        {
            SetChildrenState(true);
            TextMeshProUGUI[] textElements = GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI textElement in textElements)
            {
                textElement.alpha = 0;
            }
            isFading = true;
        }
    }
}
