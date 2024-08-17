using UnityEngine;
using UnityEngine.UIElements;

public class CustomButton : VisualElement
{
    public string buttonName = string.Empty;
    public CustomButton()
    {
        // Set default styles or classes
        AddToClassList("custom-button");

        // Register a click event handler
        this.RegisterCallback<ClickEvent>(OnButtonClick);
    }

    public void SetButtonProperties(string buttonName, string additionalButtonClassName = null)
    {
        this.buttonName = buttonName;

        // Apply any additional class names
        if (!string.IsNullOrEmpty(additionalButtonClassName))
        {
            AddToClassList(additionalButtonClassName);
        }

        // Create a label or any other content for the button
        var label = new Label(buttonName);
        label.AddToClassList("custom-button-label");
        this.Add(label);
    }

    private void OnButtonClick(ClickEvent evt)
    {
        Debug.Log("Button clicked: " + buttonName);

        var parent = this.parent;
        if (parent != null)
        {
            foreach (var child in parent.Children())
            {
                if (child is CustomButton customButton)
                {
                    customButton.AddAdditionalClass("inactiveButton");
                }
            }
            RemoveAdditionalClass("inactiveButton");
        }
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
