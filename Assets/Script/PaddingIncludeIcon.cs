using UnityEngine;
using UnityEngine.UI;

public class PaddingIncludeIcon : MonoBehaviour {
	public float padding = 5f;
	public bool skipIcon = true;
	private bool layoutFinished = false;
	private RectTransform parentRectTransform;
	public void Awake () {
		parentRectTransform= transform.parent.GetComponent<RectTransform>();
		if(0 != parentRectTransform.rect.width) {
			Layout();
		}
	}
	public void Update () {
		if((false == layoutFinished) && (0 != parentRectTransform.rect.width)) {
			Layout();
		}
	}
	private void Layout() {
		float width = parentRectTransform.rect.width - 2f*padding;
		if(true == skipIcon) {
			width -= parentRectTransform.rect.height + padding;
		}
		GetComponent<RectTransform>().sizeDelta = new Vector2(width, 1f);
		layoutFinished = true;
	}
}
