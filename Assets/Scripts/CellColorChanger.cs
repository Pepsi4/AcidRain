using UnityEngine;

public class CellColorChanger : MonoBehaviour
{
    [Header("Texture Settings")]
    [SerializeField] private Texture2D texture;
    [SerializeField] private int textureSize = 512;

    [Header("Cell Settings")]
    [SerializeField] private int cellSize = 48;
    [SerializeField] private int cellX = 0;
    [SerializeField] private int cellY = 0;
    [SerializeField] public Color NewColor = Color.red;

    [Header("File Settings")]
    [SerializeField] private string texturePath = "Assets/Textures/modified_texture.png";

    public void ChangeCellColor()
    {
        if (texture == null)
        {
            Debug.LogError("Texture not assigned!");
            return;
        }

        if (texture.width != textureSize || texture.height != textureSize)
        {
            Debug.LogError("Texture size does not match the specified size!");
            return;
        }

        int startX = cellX * cellSize;
        int startY = texture.height - cellY * cellSize;

        if (startX + cellSize > textureSize || startY - cellSize < 0)
        {
            Debug.LogError("Cell coordinates exceed texture boundaries!");
            return;
        }

        for (int x = startX; x < startX + cellSize; x++)
        {
            for (int y = startY; y > startY - cellSize; y--)
            {
                texture.SetPixel(x, y, NewColor);
            }
        }

        texture.Apply();

        SaveTextureToFile();
    }

    private void SaveTextureToFile()
    {
        byte[] textureBytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(texturePath, textureBytes);
    }
}
