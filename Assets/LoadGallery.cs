using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



/*
유니티로 사진첩(갤러리) 열기 
*/
public class LoadGallery : MonoBehaviour
{
    public RawImage img;

    public void OnClickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo selected = new FileInfo(file);     // selected : 선택한 이미지 

            // 용랑 제한 (byte 단위)
            if (selected.Length > 50000000)
            {
                return;
            }

            // 불러오기 
            if (!string.IsNullOrEmpty(file))
            {
                // 불러오는 작업 
                StartCoroutine(LoadImage(file));
            }

        });
    }


    IEnumerator LoadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string fileName = Path.GetFileName(path).Split(".")[0];
        string savePath = Application.persistentDataPath + "/Image";

        // 경로가 존재하지 않으면 경로 생성 
        if (Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        // 지정한 저장 경로에 png 형식으로 불러온 fileData를 저장 
        File.WriteAllBytes(savePath + fileName + ".png", fileData);

        var temp = File.ReadAllBytes(savePath + fileName + ".png");

        // byte를 Texture2D로 변환 
        Texture2D tex = new Texture2D(0, 0);
        tex.LoadImage(temp);

        img.texture = tex;
        img.SetNativeSize();    // 이미지가 가지고 있는 원래 사이즈로 변환 
        ImageSizeSetting(img, 1000, 1000);
    }

    void ImageSizeSetting(RawImage img, float x, float y)
    {
        var imgX = img.rectTransform.sizeDelta.x;
        var imgY = img.rectTransform.sizeDelta.y;

        if (x / y > imgX / imgY)    // 이미지의 세로 길이가 더 길다 
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgX * (y / imgY));
        }
        else    // 가로길이가 더 길다 
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgY * (x / imgX));
        }
    }
}

