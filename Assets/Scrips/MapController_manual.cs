using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController_manual : MonoBehaviour
{
    public static MapController_manual Instance { get; set; }

    public GameObject mapParent;
    List<UnityEngine.UI.Image> mapImages;
    // UnityEngine.UI.Image[] mapImages;

    public Color highlightColor = Color.yellow;
    public Color dimColor = new Color(1f, 1f, 1f, 0.5f);

    public RectTransform playerIconTransform;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }

        mapImages = mapParent.GetComponentsInChildren<UnityEngine.UI.Image>().ToList();
    }


    public void HighlightArea(string areaName)
    {
        foreach (UnityEngine.UI.Image area in mapImages)
        {
            area.color = dimColor;
        }
        UnityEngine.UI.Image currentArea = mapImages.Find(x => x.name == areaName);

        if (currentArea != null)
        {
            currentArea.color = highlightColor;
            playerIconTransform.position = currentArea.GetComponent<RectTransform>().position;
        }
        else
        {
            Debug.LogWarning("area not found" + areaName);
        }

    }



}
