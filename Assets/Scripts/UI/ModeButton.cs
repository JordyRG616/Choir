using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModeButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Image icon;
    [SerializeField] private KeyCode key;
    [SerializeField] private InteractionMode mode;
    private Color ogIconColor;
    private Image image;
    private bool selected;

    private InputManager inputManager;

    void Start()
    {
        inputManager = InputManager.Main;

        image = GetComponent<Image>();
        ogIconColor = icon.color;
    }

    public void ChangeSelection(bool overrideRestriction = false, bool muteCancel = false)
    {
        if(ShopManager.Main.panelOpen && !overrideRestriction) return;

        selected = !selected;
        if(selected)
        {
            if(inputManager.selectedButton != null) inputManager.selectedButton.ChangeSelection(muteCancel:true);
            image.overrideSprite = selectedSprite;
            icon.color = Color.white;
            inputManager.selectedButton = this;
            inputManager.interactionMode = mode;
            AudioManager.Main.RequestEvent(FixedAudioEvent.Confirm);
        }
        else
        {
            image.overrideSprite = null;
            icon.color = ogIconColor;
            inputManager.selectedButton = null;
            inputManager.interactionMode = InteractionMode.Default;
            if(!muteCancel) AudioManager.Main.RequestEvent(FixedAudioEvent.Cancel);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            ChangeSelection();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ChangeSelection();
    }
}
