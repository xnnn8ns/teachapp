using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Surface : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
{
    [SerializeField]
    private TextMeshPro _titleText;

    private Information _parentObject;
    private AnimationExecuter _parentAnimationExecutor;

    private Action<Information, AnimationExecuter> _actionClickSurface;
    private Action<AnimationExecuter, Vector2> _actionDownSurface;
    private Action<AnimationExecuter, Vector2> _actionMoveSurface;
    private Action<AnimationExecuter, Vector2> _actionUpSurface;

    public void OnPointerClick(PointerEventData eventData)
    {
        _actionClickSurface?.Invoke(_parentObject, _parentAnimationExecutor);
    }

    public void SetTitle(string title)
    {
        _titleText.text = title;
    }

    public void SetActionClickCallback(Information parentObject,
                        AnimationExecuter parentAnimationExecuter,
                        Action<Information, AnimationExecuter> actionClickSurface)
    {
        _parentObject = parentObject;
        _parentAnimationExecutor = parentAnimationExecuter;
        _actionClickSurface = actionClickSurface;
    }

    public void SetActionDragCallback(Information parentObject,
                        AnimationExecuter parentTransform,
                        Action<AnimationExecuter, Vector2> actionMoveSurface)
    {
        _parentObject = parentObject;
        _parentAnimationExecutor = parentTransform;
        _actionMoveSurface = actionMoveSurface;
    }

    public void SetActionDownCallback(Information parentObject,
                        AnimationExecuter parentTransform,
                        Action<AnimationExecuter, Vector2> actionDownSurface,
                        Action<AnimationExecuter, Vector2> actionMoveSurface,
                        Action<AnimationExecuter, Vector2> actionUpSurface)
    {
        _parentObject = parentObject;
        _parentAnimationExecutor = parentTransform;
        _actionDownSurface = actionDownSurface;
        _actionMoveSurface = actionMoveSurface;
        _actionUpSurface = actionUpSurface;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _actionDownSurface?.Invoke(_parentAnimationExecutor, eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _actionUpSurface?.Invoke(_parentAnimationExecutor, eventData.position);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        _actionMoveSurface?.Invoke(_parentAnimationExecutor, eventData.position);
    }

}
