using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RungManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _rungObject = new List<GameObject>();
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject hewf;
    public int id;
    private void Start()
    {
        CurrentUserRung(id);
    }

    public void ResetRung(int idRung) 
    {
        for (int i = 0; i < _rungObject.Count; i++)
        {
            GameObject obj = _rungObject[i];

            foreach (Transform child in obj.transform) // перебираем дочерние элементы объекта obj
            {
                RectTransform childTransform = child.GetComponent<RectTransform>(); // получаем RectTransform каждого дочернего элемента
                childTransform.localScale = new Vector3(.6f, .6f, 1f); // установка нового размера
            }
        }

        CurrentUserRung(idRung);
    }

    

    public void ScrollToItem(int id)
    {
        Transform child = transform.GetChild(id);
        RectTransform obj = child.GetComponent<RectTransform>();

        float contentWidth = scrollRect.content.rect.width;
        float viewportWidth = scrollRect.viewport.rect.width;

        float elementPosition = obj.anchoredPosition.x;
        float elementMiddlePosition = elementPosition + obj.rect.width / 2;
        float elementCenteredPosition = elementMiddlePosition - contentWidth / 2;

        float normalizedPosition = elementCenteredPosition / (contentWidth - viewportWidth);
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(normalizedPosition);
    }

    public void CurrentUserRung(int idRung) 
    {
        GameObject obj = _rungObject[idRung];

        foreach (Transform child in obj.transform) // перебираем дочерние элементы объекта obj
        {
            RectTransform childTransform = child.GetComponent<RectTransform>(); // получаем RectTransform каждого дочернего элемента

            childTransform.localScale = new Vector3(.8f, .8f, 1f); // установка нового размера
            
        }
        
        ScrollToItem(idRung);

    }
}
