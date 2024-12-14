using System.Collections.Generic;
using UnityEngine;

public class LineRendererHandler
{
    private static List<LineRendererHandler> instances = new List<LineRendererHandler>();

    private LineRenderer lineRenderer;
    private Color lineColor;
    private float emissionIntensity;

    private const float defaultWidth = 0.1f;
    private const float widthCoefficient = 0.005f;

    public static void setWidthMap(float orthographicSize)
    {
        foreach (var instance in instances)
        {
            instance.SetLineWidth(orthographicSize * widthCoefficient);
        }
    }

    public static void setWidthDefault()
    {
        foreach (var instance in instances)
        {
            instance.SetLineWidth(defaultWidth);
        }
    }

    public LineRendererHandler(GameObject parentObject, Color color, float intensity = 1.0f)
    {
        instances.Add(this);
        lineColor = color;
        emissionIntensity = intensity;

        if (parentObject.GetComponent<LineRenderer>() == null)
        {
            lineRenderer = parentObject.AddComponent<LineRenderer>();
        }
        else
        {
            lineRenderer = parentObject.GetComponent<LineRenderer>();
        }

        InitializeLineRenderer();
    }

    private void InitializeLineRenderer()
    {
#if UNITY_EDITOR
        if (lineRenderer.sharedMaterial == null)
        {
            lineRenderer.sharedMaterial = new Material(Shader.Find("Unlit/Color"));
        }
        lineRenderer.sharedMaterial.color = lineColor;
        lineRenderer.sharedMaterial.EnableKeyword("_EMISSION");
        lineRenderer.sharedMaterial.SetColor("_EmissionColor", lineColor * emissionIntensity);
#else
        if (lineRenderer.material == null)
        {
            lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        }
        lineRenderer.material.color = lineColor;
        lineRenderer.material.EnableKeyword("_EMISSION");
        lineRenderer.material.SetColor("_EmissionColor", lineColor * emissionIntensity);
#endif
        SetLineWidth(defaultWidth);
        lineRenderer.loop = false; // Ä¬ÈÏ²»±ÕºÏ
    }

    public void SetPositions(Vector3[] positions)
    {
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
    }

    public void SetLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    public void SetColor(Color color)
    {
        lineColor = color;
#if UNITY_EDITOR
        lineRenderer.sharedMaterial.color = lineColor;
        lineRenderer.sharedMaterial.SetColor("_EmissionColor", lineColor * emissionIntensity);
#else
        lineRenderer.material.color = lineColor;
        lineRenderer.material.SetColor("_EmissionColor", lineColor * emissionIntensity);
#endif
    }

    public void Enable(bool enabled)
    {
        lineRenderer.enabled = enabled;
    }


    public bool isEnable()
    {
        return lineRenderer.enabled;
    }

    public void SetLoop(bool loop)
    {
        lineRenderer.loop = loop;
    }
}
