using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(LanguageSupportedText))]
public class StaticLanguageText : MonoBehaviour
{
    //[Tooltip(tooltip:"This is a static dictionary where you CAN NOT:\n" +
    //    "1 - Add any element\n" +
    //    "2 - Remove any element\n" +
    //    "3 - Change any key\n" +
    //    "4 - Move any element\n" +
    //    "Good news though : These are the perfect conditions to disable you from being able to make any mistakes in any way\n" +
    //    "And also you get to use the Minus Button as a clear button for the value field of an element :)\n" +
    //    "Also the first element will also write to the placeholder text field of the Text object,saving you a ton of time!")
    //    ]
    public List<DictionaryNode<Languages, string>> texts = new List<DictionaryNode<Languages, string>>();
    public static Array languages;
    bool isStarted = false;
    public static bool ISSTARTED;
    public static int languageCount;
    LanguageSupportedText text;
    [NonSerialized] public StaticLanguageMultipleTextManager manager;
    public void Start()
    {
        //if (Application.IsPlaying(this)) return;
        //if (isStarted) return;

        //isStarted = true;

        //text = GetComponent<LanguageSupportedText>();

        //if (languages == null && !ISSTARTED)
        //{
        //    languages = Enum.GetValues(typeof(Languages));
        //    languageCount = languages.Length;
        //    ISSTARTED = true;
        //}

        //if(texts.Count==languages.Length)
        //{
        //    foreach (var lang in languages)
        //        texts.Add(new DictionaryNode<Languages, string>((Languages)lang, ""));
        //}
    }

    public List<DictionaryNode<Languages, string>> GetActiveTexts()
    {
        if (!manager || manager.ActiveText == -1)
            return texts;
        else return manager.otherTexts[manager.ActiveText].list;
    }

    [Obsolete(message:"Don't use this lol",error:true)]
    void Validate()
    {
        if (!isStarted) Start();
        int counter = 0;
        bool naughtyBoy = true;

        Debug.Log(languages.Length);

        if (texts.Count < languages.Length)//If the user deleted one of the rows
        {
            foreach (var lang in languages)
            {
                try
                {
                    if (texts[counter].firstElement != (Languages)lang)
                    {
                        texts.Insert(counter, new DictionaryNode<Languages, string>((Languages)lang, ""));
                        break;
                    }
                }
                catch (Exception)
                {
                    texts.Add(new DictionaryNode<Languages, string>((Languages)lang, ""));
                }
                finally
                {
                    counter++;
                }
            }
            Debug.LogWarning($"Text of the {((Languages)languages.GetValue(counter - 1)).ToString()} language is deleted!\n" +
                $"Be aware to fill it again to escape any possible UI bugs in your game!");
            naughtyBoy = false;
        }
        else if (texts.Count > languages.Length)//If the user added a new row
            texts.RemoveAt(texts.Count - 1);
        else//If the user didn't add or remove a row
        {
            List<int> indexes = new List<int>();
            foreach (var lang in languages)//Checks for mismatches
            {
                if (texts[counter].firstElement != (Languages)lang)
                    indexes.Add(counter);
                counter++;
            }

            if (indexes.Count == 0)//Good boy
                naughtyBoy = false;
            else if (indexes.Count == 1)//If the user changed the enum in one of the rows
            {
                texts[indexes[0]] = new DictionaryNode<Languages, string>(
                    (Languages)languages.GetValue(indexes[0]),
                    texts[indexes[0]].secondElement);
            }
            else//If the user moved a row up or down
            {
                DictionaryNode<Languages, string> movedRow;
                if (texts[indexes[1]].firstElement == (Languages)languages.GetValue(indexes[1] - 1))//Moved down
                {
                    movedRow = texts[indexes[0]];
                    texts.RemoveAt(indexes[0]);
                    texts.Add(movedRow);
                }
                else if (texts[indexes[1]].firstElement == (Languages)languages.GetValue(indexes[1] + 1))//Moved up
                {
                    movedRow = texts[indexes[indexes.Count - 1]];
                    texts.RemoveAt(indexes[indexes.Count - 1]);
                    texts.Insert(indexes[0], movedRow);
                }
            }
        }

        GetComponent<LanguageSupportedText>().text = texts[0].secondElement == null ? "" : texts[0].secondElement;

        if (naughtyBoy)
            Debug.LogWarning("I arranged everything for you dumbass,\n" +
                "you can't add a new row or change any enum values in a row or change the order of rows in any possible way");
    }
    [Obsolete(message:"Don't use this lol",error:true)]
    void UpdateCall()
    {
        if (!text) text = GetComponent<LanguageSupportedText>();
        text.text = texts[0].secondElement;

        if (Application.IsPlaying(this)) return;

        var langs = Enum.GetValues(typeof(Languages));
        if (languages == null || languages.Length == 0)
        {
            languages = langs;
            return;
        }

        int[] indexMapper;

        void Map(Array array1, Array array2)
        {
            indexMapper = new int[array1.Length];
            int counter1 = 0;
            int counter2 = 0;
            foreach (var lang in array1)
            {
                indexMapper[counter1] = -1;
                foreach (var language in array2)
                {
                    if (lang == language)
                    {
                        indexMapper[counter1] = counter2;
                        break;
                    }
                    counter2++;
                }
                counter2 = 0;
                counter1++;
            }
        }

        if (languages.Length < langs.Length)
        {
            Map(langs, languages);

            for (int i = 0; i < indexMapper.Length; i++)
            {

            }
        }
        else if (languages.Length > langs.Length)
        {
            Map(languages, langs);
        }
        else
        {

        }
    }
}
