using UnityEngine;
using UnityEngine.UI;

public class PaddingIncludeIcon : MonoBehaviour {
	public float padding = 5f;
	public bool skipIcon = true;
	public void Awake () {
		Rect parentRect = transform.parent.GetComponent<RectTransform>().rect;
		float width = parentRect.width - 2f*padding;
		if(true == skipIcon) {
			width -= parentRect.height + padding;
		}
		GetComponent<RectTransform>().sizeDelta = new Vector2(width, 1f);
	}
}
