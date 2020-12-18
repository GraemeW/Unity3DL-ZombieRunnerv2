using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryIndicator : MonoBehaviour
{
    public static BatteryIndicator instance { get; private set; }

    // Tunables
    [SerializeField] Image mask = null;

    // State
    float originalSize;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
    }

    public void SetValue(float fractionalValue)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * fractionalValue);
    }
}
