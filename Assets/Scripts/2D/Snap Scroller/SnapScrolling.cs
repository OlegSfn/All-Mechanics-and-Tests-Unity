using UnityEngine;
using UnityEngine.UI;

public class SnapScrolling : MonoBehaviour
{
    [Range(0f, 20f)] public float snapSpeed;
    [Range(0f, 10f)]public float scaleOffset;
    [Range(1f, 20f)]public float scaleSpeed;

    [Header("Other Objects")]
    public ScrollRect scrollRect;
    public Transform content;

    private GameObject[] panels;
    private Vector2[] panelsPos;
    private Vector2[] panelsScale;

    private RectTransform contentRect;
    private Vector2 contentVector;

    private int selectedPanID;
    private bool isScrolling;

    private void Start()
    {
        panels = new GameObject[content.childCount];
        panelsPos = new Vector2[panels.Length];
        panelsScale = new Vector2[panels.Length];

		for (int i = 0; i < panels.Length; i++)
		{
            panels[i] = content.GetChild(i).gameObject;
            panelsPos[i] = panels[i].transform.position;
            panelsScale[i] = panels[i].transform.localScale;
            Debug.Log(panelsPos[i]);
        }
    }

    private void FixedUpdate()
    {
        if (contentRect.anchoredPosition.x >= panelsPos[0].x && !isScrolling || contentRect.anchoredPosition.x <= panelsPos[panelsPos.Length - 1].x && !isScrolling)
            scrollRect.inertia = false;

        float nearestPos = float.MaxValue;
        for (int i = 0; i < panels.Length; i++)
        {
            float distance = Mathf.Abs(contentRect.anchoredPosition.x - panelsPos[i].x);
            if (distance < nearestPos)
            {
                nearestPos = distance;
                selectedPanID = i;
            }

            float scale = Mathf.Clamp(1 / (distance / 40) * scaleOffset, 0.5f, 1f);
            panelsScale[i].x = Mathf.SmoothStep(panels[i].transform.localScale.x, scale + 0.3f, scaleSpeed * Time.fixedDeltaTime);
            panelsScale[i].y = Mathf.SmoothStep(panels[i].transform.localScale.y, scale + 0.3f, scaleSpeed * Time.fixedDeltaTime);
            panels[i].transform.localScale = panelsScale[i];
        }

        float scrollVelocity = Mathf.Abs(scrollRect.velocity.x);
        if (scrollVelocity < 400 && !isScrolling) scrollRect.inertia = false;
        if (isScrolling || scrollVelocity > 400) return;
        contentVector.x = Mathf.SmoothStep(contentRect.anchoredPosition.x, panelsPos[selectedPanID].x, snapSpeed * Time.fixedDeltaTime);
        contentRect.anchoredPosition = contentVector;
    }

    public void Scrolling(bool scroll)
    {
        isScrolling = scroll;
        if (scroll) scrollRect.inertia = true;
    }
}
