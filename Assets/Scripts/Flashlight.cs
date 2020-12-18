using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    // Flashlight
    [Header("Battery")]
    [SerializeField] float initialBattery = 100f;
    [SerializeField] float maxBattery = 100f;
    [SerializeField] float drainRate = 5f;
    [Header("Decay")]
    [SerializeField] float minimumLightIntensity = 0.5f;
    [SerializeField] float maximumLightInsensity = 4f;
    [SerializeField] float minimumAngle = 40f;
    [SerializeField] float maximumAngle = 70f;
    [Header("BatteryEmpty")]
    [SerializeField] AudioClip lowBatteryCutOutSound = null;
    [SerializeField] AudioClip lowBatteryCutInSound = null;
    [SerializeField] float lowBatteryCheckPeriod = 10.0f;
    [SerializeField] float lowBatteryCheckJitter = 4.0f;

    // State
    bool isOn = true;
    float currentBattery = 100f;
    bool batteryCut = false;
    
    // Cached References
    Light flashlightLight = null;
    AudioSource audioSource = null;
    BatteryIndicator batteryIndicator = null;

    private void Start()
    {
        flashlightLight = GetComponent<Light>();
        InitializeFlashlight();
        audioSource = GetComponent<AudioSource>();
        batteryIndicator = FindObjectOfType<BatteryIndicator>();
    }

    private void InitializeFlashlight()
    {
        if (isOn) { flashlightLight.enabled = true; }
        currentBattery = initialBattery;
        UpdateLightAngle();
        UpdateLightIntensity();
    }

    private void Update()
    {
        ToggleFlashlight();
        if (isOn)
        {
            DrainBattery();
            CheckBatteryEmpty();
        }
    }

    private void FixedUpdate()
    {
        batteryIndicator.SetValue(currentBattery / maxBattery);
    }

    private void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlightLight.enabled = isOn;
        }
    }

    private float GetBatteryFraction()
    {
        return (currentBattery / maxBattery);
    }

    private void DrainBattery()
    {
        currentBattery = Mathf.Clamp(currentBattery - drainRate * Time.deltaTime, 0f, maxBattery);
        UpdateLightAngle();
        UpdateLightIntensity();
    }

    private void UpdateLightAngle()
    {
        float currentLightAngle = minimumAngle + (maximumAngle - minimumAngle) * GetBatteryFraction();
        flashlightLight.spotAngle = currentLightAngle;
    }

    private void UpdateLightIntensity()
    {
        float currentLightIntensity = minimumLightIntensity + (maximumLightInsensity - minimumLightIntensity) * GetBatteryFraction();
        flashlightLight.intensity = currentLightIntensity;
    }

    private void CheckBatteryEmpty()
    {
        if (!batteryCut && Mathf.Approximately(currentBattery, 0f))
        {
            batteryCut = true;
            StartCoroutine(CutBattery());
        }
    }

    private IEnumerator CutBattery()
    {
        while (batteryCut)
        {
            flashlightLight.enabled = !flashlightLight.enabled;
            if (flashlightLight.enabled) { audioSource.clip = lowBatteryCutInSound; }
            else { audioSource.clip = lowBatteryCutOutSound; }
            audioSource.Play();
            yield return new WaitForSeconds(UnityEngine.Random.Range(lowBatteryCheckPeriod - lowBatteryCheckJitter, lowBatteryCheckPeriod + lowBatteryCheckJitter));
        }
    }

    public void AddToBattery(float batteryCharge)
    {
        currentBattery = Mathf.Clamp(currentBattery + batteryCharge, 0f, maxBattery);
        UpdateLightAngle();
        UpdateLightIntensity();
        batteryCut = false;
        flashlightLight.enabled = true;
        isOn = true;
    }
}
