using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderUI : MonoBehaviour
{
    public BuildOptionUI buildOptionUI;
    public RectTransform optionContainer;

    public void Init(List<BuildOption> options)
    {
        options.ForEach(option => Instantiate(buildOptionUI, optionContainer).Init(option));
    }
}
