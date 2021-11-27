using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{

    // Indicator color
    public Color indicatorColor = Color.white;
    
    // Start is called before the first frame update
    void Start()
    {
        // Add indicator to yourself
        IndicatorManager.instance.AddIndicator(
            gameObject,
            indicatorColor
        );
    }

}
