using UnityEngine;
using TMPro;
using System.Collections;

public class PointAnimation : MonoBehaviour
{
    public TextMeshProUGUI textComponent; // ������ �� ��������� ������
    public string loadingText ; // ����� ��������
    public float animationSpeed = 0.5f; // �������� ��������
    public int maxDots = 3; // ������������ ���������� �����

    private int dotCount = 0; // ������� ���������� �����
    private float lastUpdateTime;

    private void Start()
    {
        // ������� �������� ��������
        lastUpdateTime = Time.time;
    }

    private void Update()
    {
        // �������� ����� � �������� ��������� ��������
        if (Time.time - lastUpdateTime > animationSpeed)
        {
            dotCount = (dotCount + 1) % (maxDots + 1); // ����������� ���������� ����� � 0 �� maxDots
            UpdateLoadingText();
            lastUpdateTime = Time.time;
        }
    }

    private void UpdateLoadingText()
    {
        textComponent.text = loadingText;
        for (int i = 0; i < dotCount; i++)
        {
            textComponent.text += ".";
        }
    }
}
