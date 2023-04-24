using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HotbarView : View
{
    [SerializeField] private BuildOptionUI optionPrefab;
    [SerializeField] private RectTransform container;

    [SerializeField] private List<BuildOptionUI> options = new List<BuildOptionUI>();

    public override void Initialize()
    {
        base.Initialize();
    }
    public override void Show(object args = null)
    {
        if (args is List<BuildCategory> argsList)
        {
            // add missing options
            int missingOptions = argsList.Count - container.childCount;
            for (int i = 0; i < missingOptions; i++) options.Add(Instantiate(optionPrefab, container));

            for (int i = 0; i < options.Count; i++)
            {
                if (i < argsList.Count)
                {
                    options[i].gameObject.SetActive(true);
                    options[i].SetOption(argsList[i].options[0]);
                }
                else
                {
                    options[i].gameObject.SetActive(false);
                }
            }
        }

        base.Show(args);
    }
}
