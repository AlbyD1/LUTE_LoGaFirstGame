using Mapbox.Unity.MeshGeneration.Modifiers;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(StickerManager))]
public class StickerMenu : MonoBehaviour
{
    [SerializeField] protected UnityEngine.UI.Button postcardButton;
    [SerializeField] protected Transform buttonLayout;

    private StickerManager stickerManager;
    private List<UnityEngine.UI.Button> spawnedButtons = new List<UnityEngine.UI.Button>();

    private void Awake()
    {
        stickerManager = GetComponent<StickerManager>();
    }

    public void ShowPostcardMenu()
    {
        if (stickerManager == null)
            return;

        var engine = stickerManager.engine;
        if (engine == null)
            return;

        var postcards = engine.Postcards;
        if (postcards == null || postcards.Count <= 0)
            return;

        if (postcardButton == null || buttonLayout == null)
            return;

        var canvasGroup = GetComponent<CanvasGroup>();
        if(canvasGroup == null) return;

        // If there are no buttons then we must be showig the menu
        if (spawnedButtons.Count <= 0)
        {
            // Spawn or use a bunch of buttons on this guy
            for (int i = 0; i < postcards.Count; i++)
            {
                int index = i;

                var newButton = Instantiate(postcardButton, buttonLayout);
                newButton.onClick.AddListener(() => stickerManager.LoadPostCard(index));
                newButton.onClick.AddListener(() => ClearMenu(canvasGroup));

                var image = newButton.GetComponent<UnityEngine.UI.Image>();

                // Set the button image to first sticker
                if (image != null)
                {
                    var firstSticker = engine.Postcards[i].StickerVars[0].Image;
                    image.sprite = firstSticker;
                }

                var buttonName = newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

                if (buttonName != null)
                {
                    var postcardName = engine.Postcards[i].PostcardName;
                    buttonName.text = postcardName;
                }

                spawnedButtons.Add(newButton);
            }

            canvasGroup.alpha = 1.0f;

        }
        else
        {
            ClearMenu(canvasGroup);
        }
    }

    private void ClearMenu(CanvasGroup canvasGroup)
    {
        if (canvasGroup == null)
            return;
        // If there are buttons then we must destroy them and hide this menu
        foreach (var button in spawnedButtons)
        {
            Destroy(button.gameObject);
        }
        spawnedButtons.Clear();

        canvasGroup.alpha = 0.0f;
    }
}