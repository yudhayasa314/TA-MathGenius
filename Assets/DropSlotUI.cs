using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlotUI : MonoBehaviour, IDropHandler
{
    public TextMeshProUGUI textSoal;
    RectTransform rectTransform;

    public event System.Action<DragUIPieces> OnDropValid;

    private void Awake()
    {
        TryGetComponent(out rectTransform);
    }


    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            DragUIPieces dragPieces = eventData.pointerDrag.GetComponent<DragUIPieces>();
            RectTransform rectDragUITransform = dragPieces.GetComponent<RectTransform>();

            rectDragUITransform.position = rectTransform.position;
            dragPieces.DropValid(true);

            OnDropValid?.Invoke(dragPieces);
        }
        else
        {
            //Debug.Log("DROP FALSE");
        }
    }

    
}
