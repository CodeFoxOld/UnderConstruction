using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using de.TrustfallGames.UnderConstruction.Core;
using de.TrustfallGames.UnderConstruction.SoundManager;

public class MainMenuBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        sfxSlider.value = GetGameSoundVolume();
        musicSlider.value = GetGameMusicVolume();

        optionsMenu.SetActive(false);
    }

    public void SwapMenuDisplay()
    {
        mainMenu.SetActive(!mainMenu.activeSelf);
        optionsMenu.SetActive(!optionsMenu.activeSelf);
        SoundHandler.GetInstance().PlaySound(SoundName.Click);
    }

    public int GetGameSoundVolume()
    {
        return (int)(Math.Round(PlayerPrefHandler.GetSfxVolume() * 5, 0));
    }

    public int GetGameMusicVolume()
    {
        return (int)(Math.Round(PlayerPrefHandler.GetMusicVolume() * 5, 0));
    }

    public void SetGameSoundVolume()
    {
        float newValue = sfxSlider.value / 5;
        PlayerPrefHandler.SetSfxVolume(newValue);
        SoundHandler.GetInstance().Refresh();
    }

    public void SetGameMusicVolume()
    {
        float newValue = musicSlider.value / 5;
        PlayerPrefHandler.SetMusicVolume(newValue);
        SoundHandler.GetInstance().Refresh();
    }
}
