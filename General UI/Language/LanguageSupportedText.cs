using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(StaticLanguageText))]
public class LanguageSupportedText : TextMeshProUGUI
{
    StaticLanguageText languageText;
    protected override void Start()
    {
        if (!Application.IsPlaying(this)) return;
        base.Start();
        languageText = GetComponent<StaticLanguageText>();
        Settings.Instance.LanguageHandler += UpdateText;
        UpdateText(Settings.Instance.CurrentLanguage);
    }

    private void UpdateText(Languages language)
    {
        if (!Application.IsPlaying(this))
            return;

        UpdateText(language, languageText.GetActiveTexts());
    }

    public virtual void UpdateText(Languages language, List<DictionaryNode<Languages, string>> texts)
    {
        if (!Application.IsPlaying(this))
            return;

        text = texts.Where(t => t.firstElement == language).FirstOrDefault().secondElement;
    }

    [MenuItem("GameObject/UI/LanguageSupportedUI/LanguageSupportedText")]
    static void CreateLanguageSupportedText()
    {
        var multiText = new GameObject("LanguageSupportedText");

        if (Selection.activeGameObject == null)
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();

            if (canvas != null)
                multiText.transform.SetParent(canvas.transform);
        }
        else
            multiText.transform.SetParent(Selection.activeGameObject.transform);

        multiText.transform.localPosition = Vector3.zero;


        Undo.RegisterCreatedObjectUndo(multiText, "Create Language Supported Text");

        Selection.activeGameObject = multiText;

        multiText.AddComponent<LanguageSupportedText>();
    }

    [MenuItem("GameObject/UI/LanguageSupportedUI/LanguageSupportedMultiText")]
    static void CreateLanguageSupportedMultiText()
    {
        var multiText = new GameObject("LanguageSupportedMultiText");

        if (Selection.activeGameObject == null)
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();

            if (canvas != null)
                multiText.transform.SetParent(canvas.transform);
        }
        else
            multiText.transform.SetParent(Selection.activeGameObject.transform);

        multiText.transform.localPosition = Vector3.zero;


        Undo.RegisterCreatedObjectUndo(multiText, "Create Language Supported Multi Text");

        Selection.activeGameObject = multiText;

        multiText.AddComponent<StaticLanguageMultipleTextManager>();
    }

    [MenuItem("GameObject/UI/LanguageSupportedUI/LanguageSupportedButton")]
    static void CreateLanguageSupportedButton()
    {
        var multiButton = new GameObject("LanguageSupportedButton");

        if (Selection.activeGameObject == null)
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();

            if (canvas != null)
                multiButton.transform.SetParent(canvas.transform);
        }
        else
            multiButton.transform.SetParent(Selection.activeGameObject.transform);

        multiButton.transform.localPosition = Vector3.zero;


        Undo.RegisterCreatedObjectUndo(multiButton, "Create Language Supported Button");

        Selection.activeGameObject = multiButton;


        var image = multiButton.AddComponent<Image>();
        image.sprite = DefaultSpriteManager.instance.UISprite;
        image.type = Image.Type.Sliced;
        image.fillCenter = true;
        image.pixelsPerUnitMultiplier = 1;

        multiButton.AddComponent<Button>().targetGraphic = image;


        var rectTransform = multiButton.GetComponent<RectTransform>();

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        rectTransform.pivot = new Vector2(0.5f, 0.5f);


        var child = new GameObject("LanguageSupportedText");

        child.transform.SetParent(multiButton.transform);
        child.transform.localPosition = Vector3.zero;

        var text = child.AddComponent<LanguageSupportedText>();
        rectTransform = child.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        text.color = Color.black;
        text.alignment = TextAlignmentOptions.Center;
        text.text = "Button";
        text.fontSize = 24;
        text.margin = new Vector4(0, 0, 0, 0);
    }

    [MenuItem("GameObject/UI/LanguageSupportedUI/LanguageSupportedMultiTextButton")]
    static void CreateLanguageSupportedMultiTextButton()
    {
        var multiButton = new GameObject("LanguageSupportedMultiTextButton");

        if (Selection.activeGameObject == null)
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();

            if (canvas != null)
                multiButton.transform.SetParent(canvas.transform);
        }
        else
            multiButton.transform.SetParent(Selection.activeGameObject.transform);

        multiButton.transform.localPosition = Vector3.zero;


        Undo.RegisterCreatedObjectUndo(multiButton, "Create Language Supported Multi Text Button");

        Selection.activeGameObject = multiButton;


        var image = multiButton.AddComponent<Image>();
        image.sprite = DefaultSpriteManager.instance.UISprite;
        image.type = Image.Type.Sliced;
        image.fillCenter = true;
        image.pixelsPerUnitMultiplier = 1;

        multiButton.AddComponent<Button>().targetGraphic = image;


        var rectTransform = multiButton.GetComponent<RectTransform>();

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

        rectTransform.pivot = new Vector2(0.5f, 0.5f);


        var child = new GameObject("LanguageSupportedMultiText");

        child.transform.SetParent(multiButton.transform);
        child.transform.localPosition = Vector3.zero;

        child.AddComponent<StaticLanguageMultipleTextManager>();
        var text = child.GetComponent<LanguageSupportedText>();
        rectTransform = child.GetComponent<RectTransform>();

        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(1, 1);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 160);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);

        text.color = Color.black;
        text.alignment = TextAlignmentOptions.Center;
        text.text = "Button";
        text.fontSize = 24;
        text.margin = new Vector4(0, 0, 0, 0);
    }
}