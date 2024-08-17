using UnityEngine.UIElements;

public class CustomButton : Button
{
    public new class UxmlFactory : UxmlFactory<CustomButton, UxmlTraits> { }

    public string buttonName = string.Empty;
    public void SetButtonProperties(string buttomName, string additionalButtomClassName)
    {
        name = buttomName;
        AddToClassList(additionalButtomClassName);
        RegisterCallback<ClickEvent>(OnButtonClick);
    }

    private void OnButtonClick(ClickEvent evt)
    {
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
