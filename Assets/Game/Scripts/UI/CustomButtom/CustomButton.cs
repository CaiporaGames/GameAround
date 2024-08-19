using System.Threading.Tasks;
using UnityEngine.UIElements;

//This is used to setup custom properties to the buttons.
public class CustomButton : VisualElement
{
    public string buttonName = string.Empty;
    public delegate Task ButtonClickedAsync(string buttonName);
    public static ButtonClickedAsync buttonClicked;
    public CustomButton()
    {
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
        buttonClicked?.Invoke(buttonName);
    }
}
