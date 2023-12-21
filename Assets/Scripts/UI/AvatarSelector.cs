using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AvatarSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject circleImageObject;
    public Sprite[] avatars;
    public GameObject objectToDisable;

    void Start()
    {
        dropdown.onValueChanged.AddListener(delegate {
            ChangeAvatar(dropdown.value);
        });
    }

    void ChangeAvatar(int index)
    {
        if (index == 0)
        {
            Image plusImage = circleImageObject.GetComponent<Image>();
            if (plusImage != null)
            {
                objectToDisable.SetActive(true);
                plusImage.sprite = avatars[index];
            }
            
        }
        else if (index > 0 && index <= avatars.Length)
        {
            Image plusImage = circleImageObject.GetComponent<Image>();
            if (plusImage != null)
            {
                plusImage.sprite = avatars[index];
                objectToDisable.SetActive(false);
            }
            else
            {
                Debug.LogError("No Image component found on " + circleImageObject.name);
            }
        }
        else
        {
            Debug.LogError("Invalid avatar index: " + index);
        }
    }
}