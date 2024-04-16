using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class Card : MonoBehaviour
{
    private EventTrigger eventTrigger;
    public Sprite backSprite;
    public Sprite successSprite;
    public Sprite[] failureSprites;
    public Sprite[] allSprites;
    public Sprite chosenFailureSprite;
    public Image image;
    public AudioClip flipSound;
    public AudioSource audioSource;
    private static int playingSoundsCount = 0;
    public bool HasStar { get; set; }
    public bool IsOpen { get; private set; }
    public bool CanInteract { get; set; } = true;
    private bool previousIsOpen;

    // Событие, которое вызывается, когда карта выбрана
    public event Action<Card> OnCardSelected;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

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

        // Воспроизведите звук переворачивания карты, только если воспроизводится менее двух звуков
        if (playingSoundsCount < 2)
        {
            playingSoundsCount++;
            StartCoroutine(PlayFlipSound());
        }
    }

    private IEnumerator PlayFlipSound()
    {
        audioSource.PlayOneShot(flipSound);
        yield return new WaitForSeconds(flipSound.length);
        playingSoundsCount--;
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

    private IEnumerator ChangeSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    
        if (IsOpen)
        {
            image.sprite = HasStar ? successSprite : chosenFailureSprite;
        }
        else
        {
            image.sprite = backSprite;
        }
    }
    
    private void UpdateCardAppearance()
    {
        Animator animator = GetComponent<Animator>();
        
        if (IsOpen != previousIsOpen)
        {
            if (IsOpen)
            {
                animator.Play("FlipCardUpAnimation");
            }
            else
            {
                animator.Play("FlipCardDownAnimation");
            }
    
            StartCoroutine(ChangeSpriteAfterDelay(0.364f));
        }
    
        previousIsOpen = IsOpen;
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