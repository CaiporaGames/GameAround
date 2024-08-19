using System.Collections;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Image Setup", menuName = "Scriptable Objects / Image Setup")]
public class SOImageSetup : ScriptableObject
{
    [SerializeField] SpriteAtlas spriteAtlas;

    public Sprite SetupComponentImage(VisualElement parent, string spriteName, string parentName = null)
    {
        var imageInstance = parent.Q<Image>(parentName);
        Sprite sprite = spriteAtlas.GetSprite(spriteName);

        if (sprite != null)
            imageInstance.sprite = sprite;
        else
            Debug.LogError("Sprite component not found.");

        return sprite;
    }
}
