using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class RectController : MonoBehaviour
{

    public bool setAspectRatio = false;

    public enum ScaleMode
    {
        SpriteAuto,
        CustomSprite,
        CustomRect
    }
    public ScaleMode mode = ScaleMode.SpriteAuto;

    RectTransform thisRectTrans;
    Image image;

    public bool scaleWithRes = false;
    public bool scaleByWidth = false;
    public Vector2 aspectRatio = Vector2.one;
    public float screenPercentage = 1.0f;
    private float _aspectRatio = 1f;

    public Rect LastFramesRect;
    public Vector2 offsetMin;
    public Vector2 offsetMax;

    private bool initialized = false;

    void Start()
    {
        StartCoroutine(InitializeRoutine());
    }

    public IEnumerator InitializeRoutine()
    {
        while (transform.parent == null) { yield return null; }

        thisRectTrans = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        offsetMin = thisRectTrans.offsetMin;
        offsetMax = thisRectTrans.offsetMax;
        initialized = true;
        SetAspectRatioOfRectTransform();
        yield return null;
    }

    void Update()
    {
        if (initialized && setAspectRatio && thisRectTrans.rect != LastFramesRect)
        {
            SetAspectRatioOfRectTransform(scaleWithRes);
            offsetMin = thisRectTrans.offsetMin;
            offsetMax = thisRectTrans.offsetMax;
        }

        LastFramesRect = thisRectTrans.rect;
    }

    public void SetAspectRatioOfRectTransform(bool isBasedOnScreenRes = false)
    {
        //Debug.Log(gameObject.name + " scaleByWidth: " + scaleByWidth + " isBasedOnRes: " + isBasedOnScreenRes);
        if (!thisRectTrans)
        {
            thisRectTrans = GetComponent<RectTransform>();
        }
        if (mode != ScaleMode.CustomRect)
        {
            if (image != null && image.sprite != null)
			{
				_aspectRatio = image.sprite.textureRect.width / image.sprite.textureRect.height;
				Debug.Log(image.sprite.textureRect.width + " x " + image.sprite.textureRect.height);
			}
            if (mode == ScaleMode.SpriteAuto)
                scaleByWidth = _aspectRatio >= 1;
        }
        else
        {
            _aspectRatio = aspectRatio.x / aspectRatio.y;
        }
        float x, y;
        Vector2 centerOfRectOffsetMin, centerOfRectOffsetMax;

        Vector2 pivotPoint = thisRectTrans.pivot;

        float oldHeight = thisRectTrans.rect.height;
        float oldWidth = thisRectTrans.rect.width;

        x = thisRectTrans.offsetMin.x + oldWidth * pivotPoint.x;
        y = thisRectTrans.offsetMin.y + oldHeight * pivotPoint.y;
        centerOfRectOffsetMin = new Vector2(x, y);

        x = thisRectTrans.offsetMax.x + -(oldWidth * (1f - pivotPoint.x));
        y = thisRectTrans.offsetMax.y + -(oldHeight * (1f - pivotPoint.y));
        centerOfRectOffsetMax = new Vector2(x, y);
        if (gameObject.name == "PuzzleInstructions")
            //Debug.Log("Screen Width: " + Screen.width + " Screen Height: " + Screen.height);

        oldWidth = (isBasedOnScreenRes && scaleByWidth) ? Screen.width * screenPercentage : thisRectTrans.rect.width;
        oldHeight = (isBasedOnScreenRes && !scaleByWidth) ? Screen.height * screenPercentage : thisRectTrans.rect.height;

        if (scaleByWidth)
        {
            float newHeight = oldWidth / _aspectRatio;
            x = centerOfRectOffsetMin.x - (oldWidth * pivotPoint.x);
            y = centerOfRectOffsetMin.y - (newHeight * pivotPoint.y);
            thisRectTrans.offsetMin = new Vector2(x, y);

            x = centerOfRectOffsetMax.x + (oldWidth * (1f - pivotPoint.x));
            y = centerOfRectOffsetMax.y + (newHeight * (1f - pivotPoint.y));
            thisRectTrans.offsetMax = new Vector2(x, y);
        }
        else //scaleByHeight
        {
            float newWidth = oldHeight * _aspectRatio;
            x = centerOfRectOffsetMin.x - (newWidth * pivotPoint.x);
            y = centerOfRectOffsetMin.y - (oldHeight * pivotPoint.y);
            thisRectTrans.offsetMin = new Vector2(x, y);

            x = centerOfRectOffsetMax.x + (newWidth * (1f - pivotPoint.x));
            y = centerOfRectOffsetMax.y + (oldHeight * (1f - pivotPoint.y));
            thisRectTrans.offsetMax = new Vector2(x, y);
        }

        if (isBasedOnScreenRes)
        {
            setAspectRatio = true;
            LastFramesRect = thisRectTrans.rect;
        }
    }


    public void SetAspectRatioOfRectTransform(RectTransform parentRect, float parentPercentage, bool p_ScaleByWidth, float parentWidth, float parentHeight)
    {
        if (!thisRectTrans)
        {
            thisRectTrans = GetComponent<RectTransform>();
        }
        if (mode != ScaleMode.CustomRect)
        {
            if (image != null && image.sprite != null)
                _aspectRatio = image.sprite.textureRect.width / image.sprite.textureRect.height;
            if (mode == ScaleMode.SpriteAuto)
                scaleByWidth = _aspectRatio >= 1;
        }
        else
        {
            _aspectRatio = aspectRatio.x / aspectRatio.y;
        }
        float x, y;
        Vector2 centerOfRectOffsetMin, centerOfRectOffsetMax;

        float oldHeight = thisRectTrans.rect.height;
        float oldWidth = thisRectTrans.rect.width;

        x = thisRectTrans.offsetMin.x + oldWidth / 2;
        y = thisRectTrans.offsetMin.y + oldHeight / 2;
        centerOfRectOffsetMin = new Vector2(x, y);

        x = thisRectTrans.offsetMax.x + -(oldWidth / 2);
        y = thisRectTrans.offsetMax.y + -(oldHeight / 2);
        centerOfRectOffsetMax = new Vector2(x, y);

        oldWidth = (parentRect && p_ScaleByWidth) ? parentWidth * parentPercentage : thisRectTrans.rect.width;
        oldHeight = (parentRect && !p_ScaleByWidth) ? parentHeight * parentPercentage : thisRectTrans.rect.height;

        if (scaleByWidth)
        {
            float newHeight = oldWidth / _aspectRatio;
            x = centerOfRectOffsetMin.x - (oldWidth / 2);
            y = centerOfRectOffsetMin.y - (newHeight / 2);
            thisRectTrans.offsetMin = new Vector2(x, y);

            x = centerOfRectOffsetMax.x + (oldWidth / 2);
            y = centerOfRectOffsetMax.y + (newHeight / 2);
            thisRectTrans.offsetMax = new Vector2(x, y);
        }
        else //scaleByHeight
        {
            float newWidth = oldHeight * _aspectRatio;
            x = centerOfRectOffsetMin.x - (newWidth / 2);
            y = centerOfRectOffsetMin.y - (oldHeight / 2);
            thisRectTrans.offsetMin = new Vector2(x, y);

            x = centerOfRectOffsetMax.x + (newWidth / 2);
            y = centerOfRectOffsetMax.y + (oldHeight / 2);
            thisRectTrans.offsetMax = new Vector2(x, y);
        }
    }
}
