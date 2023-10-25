using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset;

    public void OnPointerDown(PointerEventData eventData)
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 필요한 경우 드래그 시작 시 수행할 작업을 여기에 추가
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPosition = new Vector3(eventData.position.x, eventData.position.y, Camera.main.nearClipPlane);
        transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 필요한 경우 드래그 종료 시 수행할 작업을 여기에 추가
    }
}
