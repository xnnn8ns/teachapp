using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimationLoadText : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // ������ �� ��������� ������
    public string[] textArray; // ������ �������
    public float typingSpeed = 0.05f; // �������� ������
    public float delayBetweenTexts = 2.0f; // �������� ����� ��������

    private bool isTyping = false; // ����, �����������, ���� �� ������� ������
    private int currentTextIndex = -1; // ������ �������� ������

    private void Start()
    {
        // ������� �������� ��������
        TypeRandomText();
    }



    private void TypeRandomText()
    {
        
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, textArray.Length);
            } while (randomIndex == currentTextIndex); // ���������, ��� ����� ����� �� ��������� � ����������

            currentTextIndex = randomIndex;

            // ������� ������ ���������� ������
            StartCoroutine(TypeText(textArray[currentTextIndex]));
        
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        textComponent.text = "";

        for (int charIndex = 0; charIndex < text.Length; charIndex++)
        {
            textComponent.text += text[charIndex];
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
