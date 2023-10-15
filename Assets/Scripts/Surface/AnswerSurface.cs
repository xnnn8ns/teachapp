using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnswerSurface : Surface
{
    private Answer _answer;

    public Vector3 BasePosition = Vector3.zero;

    public Answer GetAnswer()
    {
        return _answer;
    }

    public void SetAnswer(Answer answer, bool scaleByTitleLenth)
    {
        _answer = answer;
        if (scaleByTitleLenth)
            SetScaleForContent();
        else
            SetScaleForShelf();
    }

    private void SetScaleForShelf()
    {
        float lenthScale = Settings.SCALE_SURFACE_MAX_SHELF;
        Vector3 localScale = transform.localScale;
        float multi = lenthScale / localScale.x;
        RectTransform rect = GetComponentInChildren<RectTransform>();
        Vector3 rectScale = rect.localScale;
        rectScale.x /= multi;
        rect.localScale = rectScale;

        localScale.x = lenthScale;
        transform.localScale = localScale;
    }

    private void SetScaleForContent()
    {
        int contentCount = _answer.Title.Length;
        float lenthScale = Settings.SCALE_CHAR * contentCount;
        if (lenthScale > Settings.SCALE_SURFACE_MAX)
            lenthScale = Settings.SCALE_SURFACE_MAX;
        if (lenthScale < Settings.SCALE_SURFACE_MIN)
            lenthScale = Settings.SCALE_SURFACE_MIN;
        Vector3 localScale = transform.localScale;

        float multi = lenthScale / localScale.x;

        RectTransform rect = GetComponentInChildren<RectTransform>();
        Vector3 rectScale = rect.localScale;
        rectScale.x /= multi;
        rect.localScale = rectScale;

        localScale.x = lenthScale;
        transform.localScale = localScale;
    }
}
