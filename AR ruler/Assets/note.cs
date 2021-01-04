using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class note : MonoBehaviour
{

    [SerializeField]
    private Image icon;

    public int count=1;

  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onclick()
    {
        if(count==0)
        {
            icon.gameObject.SetActive(true);
            count=1;
        }
        else
        {
            icon.gameObject.SetActive(false);
            count=0;
        }
    }
}
