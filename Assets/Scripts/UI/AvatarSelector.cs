using System;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AvatarSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public GameObject circleImageObject;
    public Sprite[] avatars;
    public GameObject objectToDisable;
    private string dataPath;

    [System.Serializable]
    public class UserData
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private int avatar;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public int Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
    }

    public static event Action<int> OnAvatarChanged;

    void Start()
    {
        dataPath = Path.Combine(Application.dataPath, "Resources/userData.json");

        dropdown.onValueChanged.AddListener(delegate {
            ChangeAvatar(dropdown.value);
            SaveAvatarSelection(1, dropdown.value); // нужен способ определения id игрока
        });

        LoadAvatarSelection(1); // нужен способ определения id игрока
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

    void SaveAvatarSelection(int id, int index)
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            UserData data = JsonUtility.FromJson<UserData>(json);
            if (data.ID == id)
            {
                data.Avatar = index;
                json = JsonUtility.ToJson(data);
                File.WriteAllText(dataPath, json);
                OnAvatarChanged?.Invoke(index);
            }
        }
    }

    void LoadAvatarSelection(int id)
    {
        if (File.Exists(dataPath))
        {
            string json = File.ReadAllText(dataPath);
            UserData data = JsonUtility.FromJson<UserData>(json);
            if (data.ID == id)
            {
                dropdown.value = data.Avatar;
                ChangeAvatar(data.Avatar);
            }
        }
    }
}