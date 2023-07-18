using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUIPieces : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isRightAnswer;
    public TextMeshProUGUI txtPilihanJawaban;

    public Canvas canvas;
    //public RectTransform parentSlotRectTransform;
    //CanvasGroup parentSlotCanvasGroup;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    public bool isDropValid;

    private void Awake()
    {
        TryGetComponent(out rectTransform);
        TryGetComponent(out canvasGroup);
        //parentSlotRectTransform.TryGetComponent(out parentSlotCanvasGroup);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //parentSlotCanvasGroup.alpha = 0.3f;
        canvasGroup.alpha = 0.3f;
        canvasGroup.blocksRaycasts = false;
        isDropValid = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        //parentSlotCanvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        if (!isDropValid)
        {
            //rectTransform.position = originalPosition;
            ResetPosition();
        }

        //Debug.Log("END DRAG");
    }

    public void DropValid(bool isValid)
    {
        isDropValid = isValid;
    }

    public void ResetPosition()
    {
        rectTransform.SetLeft(0);
        rectTransform.SetRight(0);
        rectTransform.SetTop(0);
        rectTransform.SetBottom(0);
    }
}

public static class RectTransformExtensions
{
    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}