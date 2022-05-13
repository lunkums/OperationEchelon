using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI infoField;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private RectTransform rectTransform;

    public bool Active { set => gameObject.SetActive(value); }

    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        float xPivot = mousePos.x / Screen.width;
        float yPivot = mousePos.y / Screen.height;

        rectTransform.pivot = new Vector2(xPivot, yPivot);
        transform.position = mousePos;
    }

    public void SetText(string header, string info)
    {
        headerField.text = header;
        infoField.text = info;
        layoutElement.enabled = Math.Max(headerField.preferredWidth, infoField.preferredWidth) >= layoutElement.preferredWidth;
    }
}
