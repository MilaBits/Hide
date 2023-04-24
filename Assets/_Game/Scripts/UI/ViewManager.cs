using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public static ViewManager Instance { get; private set; }

    [SerializeField]
    private View[] views;

    [SerializeField]
    private View defaultView;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialize()
    {
        foreach (View view in views)
        {
            view.Initialize();
        }
    }

    public void ShowAdditive<TView>(object args = null) where TView : View
    {
        foreach (View view in views)
        {
            if (view is TView)
            {
                view.Show(args);
            }
        }
    }

    public void Show<TView>(object args = null) where TView : View
    {
        foreach (View view in views)
        {
            if (view is TView)
            {
                view.Show(args);
            }else
            {
                view.Hide();
            }
        }
    }

    public void Hide<TView>() where TView : View
    {
        foreach (View view in views)
        {
            if (view is TView)
            {
                view.Hide();
            }
        }
    }
}
