using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ColorStateManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button playButton;
    [Tooltip("The number of objects that need to have their color changed before the game can start.")]
    [SerializeField] private int neededColorsChangeToPlay;

    private HashSet<ColorDropHandler> changedColors = new HashSet<ColorDropHandler>();

    public void MarkColorChanged(ColorDropHandler element)
    {
        if (!changedColors.Contains(element))
        {
            changedColors.Add(element);
            Debug.Log($"Color changed for {element}.");

            CheckPlayButtonState();
        }
    }

    private void CheckPlayButtonState()
    {
        if (changedColors.Count >= neededColorsChangeToPlay)
        {
            playButton.interactable = true;
            Debug.Log("Play button activated!");
        }
    }
}
