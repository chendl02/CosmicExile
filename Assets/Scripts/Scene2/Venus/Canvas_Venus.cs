using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Venus : MonoBehaviour
{
    // Start is called before the first frame update
    public List<ThunderArea> thunderAreaList;
    public List<Text> markers;
    public Camera mainCamera; // Ö÷ÉãÏñ»ú
    void Start()
    {
        thunderAreaList = new List<ThunderArea>();
        mainCamera = GameObject.Find("My_Camera").GetComponent<Camera>(); ;
        GameObject[] thunderAreas = GameObject.FindGameObjectsWithTag("ThunderArea");
        foreach (GameObject thunderArea in thunderAreas)
        {
            ThunderArea area = thunderArea.GetComponent<ThunderArea>(); if (area != null)
            {
                thunderAreaList.Add(area);
                GameObject newMarkerObject = new GameObject("Marker"); 
                newMarkerObject.transform.SetParent(transform); 
                Text newMarker = newMarkerObject.AddComponent<Text>(); 
                newMarker.text = "ThunderArea"; 
                newMarker.font = Resources.Load<Font>("Consolas"); 
                newMarker.color = Color.red; 
                newMarker.fontSize = 24;
                newMarker.alignment = TextAnchor.MiddleCenter;
                markers.Add(newMarker); 
            }
            }
    }

    // Update is called once per frame
    void Update()
    {
        if (mainCamera == null) { Debug.LogError("Main camera is null!"); return; } 
        if (thunderAreaList == null) { Debug.LogError("ThunderArea list is null!"); return; }
        for (int i = 0; i < thunderAreaList.Count; i++) 
        {
            if (thunderAreaList[i] == null) { Debug.LogError("ThunderArea at index " + i + " is null!"); continue; }
            Vector3 screenPos = mainCamera.WorldToScreenPoint(thunderAreaList[i].transform.position);
            screenPos.y += 20;
            markers[i].rectTransform.position = screenPos;
        }
    }
}
