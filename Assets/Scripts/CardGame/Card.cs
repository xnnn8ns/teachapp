using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour
{
    private EventTrigger eventTrigger;
    public Sprite backSprite;
    public Sprite successSprite;
    public Sprite failureSprite;
    public Image image;
    public bool HasStar { get; set; }
    public bool IsOpen { get; private set; }
    public bool CanInteract { get; set; } = true;

    // Событие, которое вызывается, когда карта выбрана
    public event Action<Card> OnCardSelected;

    public void Initialize(bool hasStar, bool isFaceUp = false)
    {
        HasStar = hasStar;
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        eventTrigger = GetComponent<EventTrigger>();
        IsOpen = isFaceUp;
        UpdateCardAppearance();
        CanInteract = true;
    }

    public void OnButtonClick()
    {
        if (!IsOpen && CanInteract)
        {
            // Вызываем событие OnCardSelected, если карта была выбрана
            OnCardSelected?.Invoke(this);
        }
    }

    public void Flip()
    {
        IsOpen = !IsOpen;
        UpdateCardAppearance();
    }

    public void ShowSuccess()
    {
        HasStar = true;
        UpdateCardAppearance();
    }

    public void ShowFailure()
    {
        HasStar = false;
        UpdateCardAppearance();
    }

    private void UpdateCardAppearance()
    {
        if (IsOpen)
        {
            image.sprite = HasStar ? successSprite : failureSprite;
        }
        else
        {
            image.sprite = backSprite;
        }
    }

    public void Block()
    {
        GetComponent<Button>().interactable = false;
    }

    public void BlockCardInteraction() 
    {
        CanInteract = false;
    }

    public void UnblockCardInteraction() 
    {
        CanInteract = true;
    }
}