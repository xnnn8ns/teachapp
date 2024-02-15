using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Events;

public class NewsData
{
    public string id;
    public string title;
    public string text;
    public string image;
    public string icon;
    public int version;
}

public class NewsList
{
    public int Version { get; set; }
    public List<NewsData> News { get; set; }
}

public class NewsDataScript : MonoBehaviour
{
    [SerializeField] private GameObject newsItemWithImagePrefab;
    [SerializeField] private GameObject newsItemWithIconPrefab;
    [SerializeField] private GameObject newsItemWithTextPrefab;
    [SerializeField] private GameObject loadMoreButtonPrefab;
    private Button loadMoreButton;
    [SerializeField] private GameObject loadPreviousButtonPrefab;
    private Button loadPreviousButton;
    private List<GameObject> obj = new List<GameObject>();
    private List<NewsData> previousNewsData;
    private List<NewsData> allNewsData;
    private List<GameObject> newsItemPool = new List<GameObject>();
    private Dictionary<int, List<NewsData>> cachedPages = new Dictionary<int, List<NewsData>>();
    private string jsonNewsFilePath = "newsList_";
    private bool jsonFileChanged = false;
    private int lastLoadedNewsIndex = 0;
    private int currentPage = 0;
    private const int itemsPerPage = 10;
    private const int NewsPerPage = 10;

    void Start()
    {
        // получаем текущий язык
        string currentLanguage = PlayerPrefs.GetString("language", "En");
        jsonNewsFilePath += currentLanguage;
        NewsList newsList = LoadNewsFromFile(jsonNewsFilePath);
        allNewsData = newsList.News;

        // Проверяем, был ли изменен JSON файл
        jsonFileChanged = CheckIfJsonFileChanged();

        // Если JSON файл был изменен, обновляем кэш
        if (jsonFileChanged)
        {
            cachedPages.Clear();
        }

        LoadNewsPage();
    }

    // Метод для загрузки страницы с новостями
    private NewsList LoadNewsFromFile(string filePath)
    {
        string json = Resources.Load<TextAsset>(filePath)?.text;
        if (json != null)
        {
            return JsonConvert.DeserializeObject<NewsList>(json);
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("File not found: ");
            sb.Append(filePath);
            Debug.LogError(sb.ToString());
            return new NewsList { Version = 0, News = new List<NewsData>() };
        }
    }

    // Метод для загрузки новостей из файла
    private void LoadNewsPage()
    {
        // получаем текущие новости
        List<NewsData> newsData = allNewsData;

        // Получаем данные из кэша, если они там есть
        if (!cachedPages.TryGetValue(currentPage, out newsData))
        {
            newsData = allNewsData;
            cachedPages[currentPage] = allNewsData;
        }
        else
        {
            // Если данных в кэше нет, загружаем их из файла и сохраняем в кэше
            cachedPages[currentPage] = allNewsData;
        }

        // удаляем старые новости
        foreach (GameObject newsItem in obj)
        {
            ReturnNewsItemToPool(newsItem);
        }
        obj.Clear();

        // удаляем старые кнопки
        DestroyButton(loadMoreButton);
        DestroyButton(loadPreviousButton);

        // загружаем новые новости
        int startIndex = currentPage * itemsPerPage;
        for (int i = startIndex; i < startIndex + itemsPerPage && i < newsData.Count; i++)
        {
            NewsData data = newsData[i];
            GameObject newsItem = CreateNewsItem(data);
            UpdateNewsItem(newsItem, data);
            obj.Add(newsItem);
        }

        // Если есть еще новости, добавляем кнопку "Загрузить еще..."
        if (startIndex + itemsPerPage < newsData.Count)
        {
            loadMoreButton = CreateButton(loadMoreButtonPrefab, LoadNextPage);
        }

        // Если мы не на первой странице, добавляем кнопку "Загрузить предыдущие новости"
        if (startIndex > 0)
        {
            loadPreviousButton = CreateButton(loadPreviousButtonPrefab, LoadPreviousPage);
        }

        gameObject.GetComponentInParent<ScrollRect>().normalizedPosition = new Vector2(0, 1);
    }
    
    // Метод для загрузки следующей страницы новостей
    private void LoadNextPage()
    {
        currentPage++;
        LoadNewsPage();
    }

    // Метод для загрузки предыдущей страницы новостей
    private void LoadPreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            LoadNewsPage();
        }
    }

    // Метод для загрузки новостей из файла
    private void LoadNewsItems(List<NewsData> newsData)
    {
        for (int i = lastLoadedNewsIndex; i < lastLoadedNewsIndex + NewsPerPage && i < newsData.Count; i++)
        {
            NewsData data = newsData[i];
            if (previousNewsData == null || !previousNewsData.Contains(data))
            {
                GameObject newsItem = CreateNewsItem(data);
                obj.Add(newsItem);
            }
        }
    }

    // Метод для создания элемента новостей
    private GameObject CreateNewsItem(NewsData data)
    {
        GameObject newsItem = GetNewsItemFromPool(data);

        // Если у новости есть изображение, загружаем его
        if (data.image != null)
        {
            Image NewsImage = newsItem.transform.Find("NewsImage").GetComponent<Image>();
            NewsImage.sprite = Resources.Load<Sprite>("Images/" + data.image);
        }
        // Если у новости есть иконка, загружаем ее
        else if (data.icon != null)
        {
            Image NewsIcon = newsItem.transform.Find("NewsIcon").GetComponent<Image>();
            NewsIcon.sprite = Resources.Load<Sprite>("Icons/" + data.icon);
        }

        // Устанавливаем заголовок и текст новости
        TextMeshProUGUI NewsTitle = newsItem.transform.Find("NewsTitle").GetComponent<TextMeshProUGUI>();
        NewsTitle.text = data.title;

        TextMeshProUGUI NewsText = newsItem.transform.Find("NewsText").GetComponent<TextMeshProUGUI>();
        NewsText.text = data.text;

        return newsItem;
    }

    private Button CreateButton(GameObject buttonPrefab, UnityAction onClickAction)
    {
        GameObject buttonObject = Instantiate(buttonPrefab, gameObject.transform);
        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(onClickAction);
        return button;
    }

    private void DestroyButton(Button button)
    {
        if (button != null)
        {
            Destroy(button.gameObject);
        }
    }
    
    // Метод для получения элемента новости из пула или создания нового
    private GameObject GetNewsItemFromPool(NewsData data)
    {
        foreach (GameObject newsItem in newsItemPool)
        {
            if (!newsItem.activeInHierarchy)
            {
                newsItem.SetActive(true);
                UpdateNewsItem(newsItem, data); // Обновляем объект новости данными
                return newsItem;
            }
        }

        // Если не нашли подходящего элемента в пуле, создаем новый
        GameObject newNewsItem;
        if (data.image != null)
        {
            newNewsItem = Instantiate(newsItemWithImagePrefab, gameObject.transform);
        }
        else if (data.icon != null)
        {
            newNewsItem = Instantiate(newsItemWithIconPrefab, gameObject.transform);
        }
        else
        {
            newNewsItem = Instantiate(newsItemWithTextPrefab, gameObject.transform);
        }
        UpdateNewsItem(newNewsItem, data);
        newsItemPool.Add(newNewsItem);
        return newNewsItem;
    }

    // Метод для обновления элемента новости данными
    private void UpdateNewsItem(GameObject newsItem, NewsData data)
    {
        TextMeshProUGUI NewsTitle = newsItem.transform.Find("NewsTitle").GetComponent<TextMeshProUGUI>();
        NewsTitle.text = data.title;

        TextMeshProUGUI NewsText = newsItem.transform.Find("NewsText").GetComponent<TextMeshProUGUI>();
        NewsText.text = data.text;
    }

    private void ReturnNewsItemToPool(GameObject newsItem)
    {
        newsItem.SetActive(false);
    }

    private bool CheckIfJsonFileChanged()
    {
        string json = Resources.Load<TextAsset>(jsonNewsFilePath)?.text;
        NewsList newsList = JsonConvert.DeserializeObject<NewsList>(json);

        // Если версия в JSON файле равна 1, возвращаем True
        if (newsList.Version == 1)
        {
            return true;
        }

        // если 0 - False
        return false;
    }
}