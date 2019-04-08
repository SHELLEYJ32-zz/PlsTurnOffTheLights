using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCheckController : MonoBehaviour
{
    public RenderTexture lightCheckTexture;
    public float maxLightIntensity;
    public float lightLevel;

    void FixedUpdate()
    {
        RenderTexture tmpTexture = RenderTexture.GetTemporary(lightCheckTexture.width, lightCheckTexture.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
        Graphics.Blit(lightCheckTexture, tmpTexture);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = tmpTexture;

        Texture2D tmp2DTexture = new Texture2D(lightCheckTexture.width, lightCheckTexture.height);

        tmp2DTexture.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
        tmp2DTexture.Apply();

        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(tmpTexture);

        Color32[] colors = tmp2DTexture.GetPixels32();

        lightLevel = 0;

        for (int a = 0; a < colors.Length; a++)
        {
            lightLevel += (0.2126f * colors[a].r) + (0.7152f * colors[a].g) + (0.0722f * colors[a].b);

        }


    }

    public bool CheckLightIntensity()
    {
        if (lightLevel <= maxLightIntensity)
            return true;

        //Debug.Log(lightLevel);
        return false;
    }
}
