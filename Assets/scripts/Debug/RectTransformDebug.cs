using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class RectTransformDebug : MonoBehaviour
{
	public Vector2 offsetMin;
	public Vector2 offsetMax;

	private RectTransform thisRectTrans;
	private Rect lastFrameRect;

	void Start()
	{
		thisRectTrans = GetComponent<RectTransform>();
		lastFrameRect = thisRectTrans.rect;
		offsetMin = thisRectTrans.offsetMin;
		offsetMax = thisRectTrans.offsetMax;
	}

	void Update()
	{
		if (thisRectTrans.rect != lastFrameRect)
		{
			offsetMax = thisRectTrans.offsetMax;
			offsetMin = thisRectTrans.offsetMin;

			lastFrameRect = thisRectTrans.rect;
		}
	}
}
