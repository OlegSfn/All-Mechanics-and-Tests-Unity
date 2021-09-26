using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandController : MonoBehaviour
{
    [SerializeField] private CustomRenderTexture customRenderTexture;
    [SerializeField] private Material textureMat;
    [SerializeField] private Color sandColor;
    [SerializeField] private float drawRadius;

    void Awake()
    {
        customRenderTexture.Initialize();
        textureMat.SetFloat("_drawRadius", drawRadius);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
		{
            textureMat.SetVector("_drawPos", Camera.main.ScreenToViewportPoint(Input.mousePosition));
            textureMat.SetColor("_drawColor", sandColor);
		}
		else
		{
            textureMat.SetVector("_drawPos", -Vector2.one);
        }
    }
}
