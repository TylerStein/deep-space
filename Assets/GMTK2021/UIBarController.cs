using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarController : MonoBehaviour
{
    public int maxTicks = 6;
    public Color visibleTickColor = Color.white;
    public Color invisibleTickColor = new Color(0f, 0f, 0f, 0f);
    public RectTransform layoutContainer;
    public GameObject tickSource;
    public List<Image> tickElements = new List<Image>();

    // Update is called once per frame
    public void SetActiveTicks(int count) {
        if (count > maxTicks || count < 0) throw new UnityException("Invalid Tick Count: " + count);
        for (int i = 0; i < maxTicks; i++) {
            if (i < count) tickElements[i].color = visibleTickColor;
            else tickElements[i].color = invisibleTickColor;
        }
    }

    public void SetMaxTicks(int count) {
        maxTicks = count;
        tickSource.SetActive(true);
        for (int i = 0; i < maxTicks; i++) {
            GameObject obj = Instantiate(tickSource, layoutContainer);
            Image img = obj.GetComponent<Image>();
            img.color = visibleTickColor;
            tickElements.Add(img);
        }
        tickSource.SetActive(false);
    }
}
