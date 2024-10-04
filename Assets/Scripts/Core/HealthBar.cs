using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] GameObject rootCanvas;
    [SerializeField] Image foreground;

    [Range(0,1)]
    [SerializeField] float changeHealthRatio;

    // Update is called once per frame
    void Update()
    {
        //�p�G��q�ʤ��������0 or 1 �������
        if (Mathf.Approximately(health.GetHealthRatio(), 0) || Mathf.Approximately(health.GetHealthRatio(), 1))
        {
            rootCanvas.SetActive(false);
            return;
        }

        rootCanvas.SetActive(true);
        rootCanvas.transform.LookAt(Camera.main.transform.position);
        foreground.fillAmount = Mathf.Lerp(foreground.fillAmount, health.GetHealthRatio(), changeHealthRatio);
    }
}
