using System;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[Serializable]
[RequireComponent(typeof(StaticLanguageText))]
public class StaticLanguageMultipleTextManager : MonoBehaviour
{
    [Tooltip(tooltip: "Use the StaticLanguageText for primary text on the gameobject and if you have to\n" +
        "use alternative/changeable texts for the gameobject,use this list below:")]
    public List<LanguageListWrapper> otherTexts;

    StaticLanguageText staticPrimaryText;
    LanguageSupportedText textComponent;
    Settings settings;
    [NonSerialized] int activeText = -1;//-1 represents the original text held within the staticPrimaryText
    public int ActiveText 
    {
        get => activeText;
        set
        {
            if (value < -1 || value >= otherTexts.Count)
                return;
            activeText = value;
            textComponent.UpdateText(
                settings.CurrentLanguage,
                activeText == -1 ? staticPrimaryText.texts : otherTexts[activeText].list
                );
        } 
    }
    private void Start()
    {
        staticPrimaryText = GetComponent<StaticLanguageText>();
        staticPrimaryText.Start();
        staticPrimaryText.manager = this;
        settings = Settings.Instance;
    }
    

    [Serializable]
    public class LanguageListWrapper : ListWrapper<DictionaryNode<Languages, string>> { }
}