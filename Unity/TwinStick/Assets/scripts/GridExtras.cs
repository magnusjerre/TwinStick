using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridExtras : MonoBehaviour {

	public RectTransform rectTransform;
	public GridLayoutGroup glg;

	private int maxElements;
	public int MaxElements {
		get { return maxElements; }
		private set { maxElements = value; }
	}

	// Use this for initialization
	void Awake () {
		int maxHorizontalElements = (int) (rectTransform.rect.width / glg.cellSize.x);
		int maxVerticalElements = (int)(rectTransform.rect.height / glg.cellSize.y);
		MaxElements = maxHorizontalElements * maxVerticalElements;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
