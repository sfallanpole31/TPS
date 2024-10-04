using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] Image healthImage;
     Health playerHealth;

    float currentHealth;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();


    }
    // Update is called once per frame
    void Update()
    {
        
        healthImage.fillAmount = Mathf.Lerp(healthImage.fillAmount,playerHealth.GetHealthRatio(),0.1f);
    }
}
