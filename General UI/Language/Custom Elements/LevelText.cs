using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelText : LanguageSupportedText
{
    public override void UpdateText(Languages language, List<DictionaryNode<Languages, string>> texts)
    {
        base.UpdateText(language, texts);
    }
}
