using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour {

    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text sensTextValue = null;
    [SerializeField] private Slider sensSlider = null;
    [SerializeField] int defaultSens = 5;
    public int mainControllerSens = 5;

    [Header("Graphics Settings")]
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private Slider brightnessSlide = null;
    [SerializeField] private float defaultBrightness = 1.0f;
    private float brightnessLevel;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels To Load")]
    public string newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSaveGameDialog = null;

    public void NewGameDialogYes() {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadGameDialogYes() {
        if (PlayerPrefs.HasKey("SavedLevel")) {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        } else {
            noSaveGameDialog.SetActive(true);
        }
    }

    public void ExitButton() {
        Application.Quit();
    }

    public void SetVolume(float volume) {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply() {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetSensitivity(float sensitivity) {
        mainControllerSens = Mathf.RoundToInt(sensitivity);
        sensTextValue.text = sensitivity.ToString("0");
    }

    public void GameplayApply() {
        // check this later.
        PlayerPrefs.SetFloat("masterSen", mainControllerSens);
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness) {
        brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0");

    }

    public void GraphicsApply() {
        // check this later
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType) {
        if (MenuType == "Sound") {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        if (MenuType == "Gameplay") {
            sensTextValue.text = defaultSens.ToString("0");
            sensSlider.value = defaultSens;
            GameplayApply();
        }
        if (MenuType == "Graphics") {
            brightnessTextValue.text = defaultBrightness.ToString("0");
            brightnessSlide.value = defaultBrightness;
            GraphicsApply();
        }
    } 

    public IEnumerator ConfirmationBox() {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}