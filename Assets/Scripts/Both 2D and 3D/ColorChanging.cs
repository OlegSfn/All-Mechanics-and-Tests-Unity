using UnityEngine;

public class ColorChanging : MonoBehaviour
{
    private Material mat;
    [SerializeField] private Color[] colors;
    [SerializeField] [Range(0.1f, 100f)] private float lerpSpeed, colorChangingSpeed;
    private float t;
    private int colorIndex = 0;

    void Awake()
    {
        mat = GetComponent<MeshRenderer>().materials[0];
    }

    void Update()
    {
        mat.color = Color.Lerp(mat.color, colors[colorIndex], lerpSpeed * Time.deltaTime);

        t = Mathf.Lerp(t, 1f, colorChangingSpeed * Time.deltaTime);
        if (t > .9f)
        {
            t = 0f;
            colorIndex++;
            colorIndex = (colorIndex >= colors.Length) ? 0 : colorIndex;
        }
    }
}
