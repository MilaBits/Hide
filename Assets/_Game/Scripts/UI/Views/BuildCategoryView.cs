using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
struct BuildCategory
{
    public string Name;
    public List<BuildOption> options;
}
public class BuildCategoryView : View
{
    [SerializeField] private RectTransform container;

    [SerializeField] private BuildOptionUI optionPrefab;
    [SerializeField] private List<BuildOptionUI> options = new List<BuildOptionUI>();

    public override void Initialize()
    {
        base.Initialize();
        base.Hide();
    }

    public override void Show(object args = null)
    {
        if (args is BuildCategory category)
        {
            // add missing options
            int missingOptions = category.options.Count - container.childCount;
            for (int i = 0; i < missingOptions; i++) options.Add(Instantiate(optionPrefab, container));


            for (int i = 0; i < options.Count; i++)
            {
                if (i < category.options.Count)
                {
                    options[i].gameObject.SetActive(true);
                    options[i].SetOption(category.options[i]);
                }else
                {
                    options[i].gameObject.SetActive(false);
                }
            }
        }

        base.Show(args);
    }

}