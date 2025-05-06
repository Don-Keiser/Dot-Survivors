// RuntimeColorPicker.cs
using UnityEngine;
using UnityEngine.UI;
using System;

public class RuntimeColorPicker : MonoBehaviour
{
    [SerializeField] private Slider rSlider, gSlider, bSlider;
    [SerializeField] private Image previewImage;
    [SerializeField] private Button confirmButton, cancelButton;

    private Action<Color> onColorSelected;
    private Color currentColor;

    public void Open(Color startColor, Action<Color> callback)
    {
        gameObject.SetActive(true);
        currentColor = startColor;
        onColorSelected = callback;

        rSlider.SetValueWithoutNotify(startColor.r);
        gSlider.SetValueWithoutNotify(startColor.g);
        bSlider.SetValueWithoutNotify(startColor.b);

        UpdatePreview();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        onColorSelected = null;
    }

    private void Awake()
    {
        rSlider.onValueChanged.AddListener(_ => OnSliderChanged());
        gSlider.onValueChanged.AddListener(_ => OnSliderChanged());
        bSlider.onValueChanged.AddListener(_ => OnSliderChanged());

        confirmButton.onClick.AddListener(() => {
            onColorSelected?.Invoke(currentColor);
            Close();
        });

        cancelButton.onClick.AddListener(Close);
    }

    private void OnSliderChanged()
    {
        currentColor = new Color(rSlider.value, gSlider.value, bSlider.value, 1f);
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        previewImage.color = currentColor;
    }
}