using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerField;
    [SerializeField] private TextMeshProUGUI infoField;
    [SerializeField] private LayoutElement layoutElement;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Animator animator;
    [SerializeField] private Image image;

    private bool active;

    // I'll be honest; this logic may not be bulletproof, but it appears to work in most cases.
    private bool Active => active && image.color.a > 0;

    private void Start()
    {
        HideImmediately();
    }

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

    public void FadeIn()
    {
        if (!Active)
            animator.SetTrigger("FadeIn");
        active = true;
    }

    public void FadeOut()
    {
        if (Active)
            animator.SetTrigger("FadeOut");
        else
            HideImmediately();
        active = false;
    }

    private void HideImmediately()
    {
        animator.SetTrigger("FadeOutImmediately");
    }
}
