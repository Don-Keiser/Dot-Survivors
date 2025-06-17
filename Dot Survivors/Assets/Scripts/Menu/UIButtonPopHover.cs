using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonPopHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleMultiplier = 1.05f;
    [SerializeField] private float transitionSpeed = 10f;

    private Vector3 originalScale;
    private bool hovering = false;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        Vector3 targetScale = hovering ? originalScale * scaleMultiplier : originalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}
