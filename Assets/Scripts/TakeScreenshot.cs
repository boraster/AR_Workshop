using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class TakeScreenshot : MonoBehaviour
{
    public RectTransform shotTemplate;
    public RectTransform panel;
    public RectTransform scrollViewContent;
    public RectTransform scrollViewShot;

    private string dataPath;
    private string folderName = "Screenshots";
    private string fullPath;
    private string folderPath;
    private string filePath;
    private static int fileNumber = 0;
    private int textureWidth = 0;
    private int textureHeight = 0;

    private Texture2D SpriteTexture;

    // Start is called before the first frame update
    void Start()
    {
        fileNumber = 0;
        folderPath = Path.Combine(Application.persistentDataPath, folderName);
        CreateFileDirectory();
        DestroySavedImages();
    }

    private void CreateFileDirectory()
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
    }

    public void TakeShot()
    {
        filePath = fileNumber.ToString() + ".png";
        fullPath = Path.Combine(folderPath, filePath);
        fileNumber++;
        DestroyScrollImages();

        SpriteTexture = ScreenCapture.CaptureScreenshotAsTexture();
        textureHeight = SpriteTexture.height;
        textureWidth = SpriteTexture.width;
        StartCoroutine(SaveShotToGallery(fullPath));
    }

    private IEnumerator SaveShotToGallery(string path)
    {
        yield return new WaitForSeconds(1.0f);
        // var shotClone = Instantiate(shotTemplate);
        var imageComponent = shotTemplate.GetComponent<Image>();
        yield return new WaitForSeconds(0.5f);

        shotTemplate.transform.SetParent(panel.transform);
        shotTemplate.transform.position = panel.transform.position;
        shotTemplate.transform.localRotation = panel.transform.localRotation;
        shotTemplate.transform.localScale = panel.transform.localScale;

        // SpriteTexture = LoadTexture(path);
        Sprite NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height),
            new Vector2(0, 0));
        yield return new WaitForSeconds(0.5f);
        imageComponent.overrideSprite = NewSprite;
        shotTemplate.gameObject.SetActive(true);
    }

    private void DeleteTempImage()
    {
        
    }
    public void SavePhoto()
    {
        File.WriteAllBytes(fullPath, SpriteTexture.EncodeToPNG());
        shotTemplate.gameObject.SetActive(false);
        
    }

    public void DeletePhoto()
    {
        shotTemplate.gameObject.SetActive(false);

    }


    private void DestroyScrollImages()
    {
        var scrollImages = scrollViewContent.GetComponentsInChildren<Image>();
        foreach (var childImage in scrollImages)
        {
            Destroy(childImage.gameObject);
        }
    }

    private void DestroySavedImages()
    {
        var allFiles = Directory.GetFiles(folderPath);

        foreach (var file in allFiles)
        {
            File.Delete(file);
        }
    }

    public void ShowGallery()
    {
        // Debug.Log(folderPath);
        DestroyScrollImages();
        var allFiles = Directory.GetFiles(folderPath);

        foreach (var file in allFiles)
        {
            // Debug.Log(file);
            var shotClone = Instantiate(scrollViewShot);
            var imageComponent = shotClone.GetComponent<Image>();

            shotClone.transform.SetParent(scrollViewContent.transform);
            shotClone.transform.position = scrollViewContent.transform.position;
            shotClone.transform.localRotation = scrollViewContent.transform.localRotation;
            shotClone.transform.localScale = scrollViewContent.transform.localScale;

            var loadedTexture = LoadTexture(file);
            Sprite NewSprite = Sprite.Create(loadedTexture, new Rect(0, 0, loadedTexture.width, loadedTexture.height),
                new Vector2(0, 0));
            imageComponent.overrideSprite = NewSprite;
            shotClone.gameObject.SetActive(true);
        }
    }

    public Texture2D LoadTexture(string FilePath)
    {
        Texture2D Tex2D;
        byte[] FileData;


        FileData = File.ReadAllBytes(FilePath);
        Tex2D = new Texture2D(textureWidth, textureHeight); // Create new "empty" texture
        Tex2D.LoadImage(FileData); // Load the imagedata into the texture (size is set automatically)
        return Tex2D; // If data = readable -> return texture
        // Return null if load failed
    }
}