using UnityEngine.UIElements;

public class CustomButton : VisualElement
{
    public string buttonName = string.Empty;
    public delegate void ButtonClicked(string buttonName);

    public static ButtonClicked buttonClicked;
    public CustomButton()
    {
        AddToClassList("custom-button");
        this.RegisterCallback<ClickEvent>(OnButtonClick);
    }

    public void SetButtonProperties(string buttonName, string additionalButtonClassName = null)
    {
        this.buttonName = buttonName;
        if (!string.IsNullOrEmpty(additionalButtonClassName))
            AddToClassList(additionalButtonClassName);

    }

    private void OnButtonClick(ClickEvent evt)
    {
        var parent = this.parent;
        if (parent != null)
        {
            foreach (var child in parent.Children())
                if (child is CustomButton customButton)
                    customButton.AddAdditionalClass("inactiveButton");
            RemoveAdditionalClass("inactiveButton");
        }

        buttonClicked?.Invoke(buttonName);
    }

    public void AddAdditionalClass(string className)
    {
        AddToClassList(className);
    }

    public void RemoveAdditionalClass(string className)
    {
        RemoveFromClassList(className);
    }
}
