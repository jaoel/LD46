using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<PanelConfiguration> _panelConfigurations = new List<PanelConfiguration>();

    [SerializeField]
    private BreakableContainer _breakableContainer;

    private void Awake()
    {
        Physics.queriesHitTriggers = true;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //large
    //1 large
    //2 medium

    //medium
    //2 small

    private void GenerateStation()
    {
        PanelConfiguration panelConfig = _panelConfigurations[UnityEngine.Random.Range(0, _panelConfigurations.Count)];

        panelConfig.LargePanels.ForEach(x =>
        {
            int spaces = 4;
            while(spaces > 0)
            {
                GameObject newBreakable = GetRandomBreakable(ref spaces);
            }
        });

        panelConfig.MediumPanels.ForEach(x =>
        {
            int spaces = 2;
        });

        panelConfig.SmallPanels.ForEach(x =>
        {
            int spaces = 1;
        });
    }

    private GameObject GetRandomBreakable(ref int spacesLeft)
    {
        GameObject result = null;

        if (spacesLeft >= 4)
        {
            int random = UnityEngine.Random.Range(0, 10);
            
            if (random <= 5)
        }

        return result;
    }
}
