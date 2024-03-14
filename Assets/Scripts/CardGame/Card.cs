using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Card : MonoBehaviour
{
    private EventTrigger eventTrigger;
    public Sprite backSprite;
    public Sprite successSprite;
    public Sprite[] failureSprites;
    public Sprite[] allSprites;
    public Sprite chosenFailureSprite;
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

        // Выбираем случайный спрайт из массива allSprites при инициализации карты
        int index = UnityEngine.Random.Range(0, allSprites.Length);
        successSprite = allSprites[index];

        // Удаляем выбранный спрайт из массива
        List<Sprite> tempList = new List<Sprite>(allSprites);
        tempList.RemoveAt(index);
        allSprites = tempList.ToArray();

        // Выбираем случайный спрайт из оставшихся в массиве allSprites для проигрышной карты
        index = UnityEngine.Random.Range(0, allSprites.Length);
        chosenFailureSprite = allSprites[index];

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
            if (HasStar)
            {
                image.sprite = successSprite;
            }
            else
            {
                // Используем выбранный спрайт для проигрыша
                image.sprite = chosenFailureSprite;
            }
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