using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<PanelConfiguration> _panelConfigurations = new List<PanelConfiguration>();

    [SerializeField]
    private BreakableContainer _breakableContainer;

    [SerializeField]
    private GameObject _baseStation;

    private void Awake()
    {
        Physics.queriesHitTriggers = true;
    }
    void Start()
    {
        GenerateStation();
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
        PanelConfiguration panelConfig = Instantiate(
            _panelConfigurations[UnityEngine.Random.Range(0, _panelConfigurations.Count)], _baseStation.transform);

        panelConfig.LargePanels.ForEach(x =>
        {
            CreateRandomBreakables(panelConfig, PanelSize.LARGE, x);
        });

        panelConfig.MediumPanels.ForEach(x =>
        {
            CreateRandomBreakables(panelConfig, PanelSize.MEDIUM, x);
        });

        panelConfig.SmallPanels.ForEach(x =>
        {
            CreateRandomBreakables(panelConfig, PanelSize.SMALL, x);
        });
    }

    private GameObject InstantiatePanel(GameObject prefab, GameObject parent, Vector3 position, Quaternion rotation)
    {
        GameObject instantiated = Instantiate(prefab, parent.transform, false);
        instantiated.transform.rotation = rotation;
        instantiated.transform.localPosition = position;

        return instantiated;
    }

    private void CreateRandomBreakables(PanelConfiguration panelConfig, PanelSize size, GameObject parentPanel)
    {
        GameObject prefab = null;
        Bounds bounds = parentPanel.GetComponentInChildren<Renderer>().bounds;
        switch (size)
        {
            case PanelSize.LARGE:
                {
                    int spaces = 4;
                    while(spaces > 0)
                    {
                        int random = UnityEngine.Random.Range(0, 10);
                        if( random <= 5 && spaces == 4)
                        {
                            prefab = _breakableContainer.GetRandomLargeBreakable();
                            InstantiatePanel(prefab, parentPanel.transform.parent.gameObject,
                                parentPanel.transform.localPosition, parentPanel.transform.rotation);
                            spaces -= 4;
                        }
                        else if (random > 5 && random < 8 && spaces % 2 == 0)
                        {
                            prefab = _breakableContainer.GetRandomMediumBreakable();
                            Vector3 offset = new Vector3(bounds.size.x / 4.0f, 0.0f, 0.0f);
                            offset = spaces == 4 ? offset * -1.0f : offset;
                            InstantiatePanel(prefab, parentPanel.transform.parent.gameObject,
                                parentPanel.transform.localPosition - offset, parentPanel.transform.rotation).name = spaces.ToString();
                            spaces -= 2;
                        }
                        else
                        {
                            prefab = _breakableContainer.GetRandomSmallBreakable();

                            float yMultiplied = parentPanel.transform.rotation == Quaternion.identity ? 4.0f : 2.0f;
                            Vector3 offset = new Vector3(bounds.size.x / 4.0f, bounds.size.y / yMultiplied, 0.0f);

                            offset.x = spaces > 2 ? offset.x * -1.0f : offset.x;
                            offset.y = spaces % 2 == 0 ? offset.y * -1.0f : offset.y;
                            GameObject smallPanel = InstantiatePanel(prefab, parentPanel.transform.parent.gameObject,
                                parentPanel.transform.localPosition - offset, parentPanel.transform.rotation);
                            smallPanel.name = spaces.ToString();
                            spaces -= 1;
                        }
                    }
                }
                break;
            case PanelSize.MEDIUM:
                {
                    int spaces = 2;
                    while(spaces > 0)
                    {
                        int random = UnityEngine.Random.Range(0, 10);
                        if (random >= 5 && spaces == 2)
                        {
                            prefab = _breakableContainer.GetRandomMediumBreakable();
                            InstantiatePanel(prefab, parentPanel.transform.parent.gameObject, 
                                parentPanel.transform.localPosition, parentPanel.transform.rotation);
                            spaces -= 2;
                        }
                        else
                        {
                            prefab = _breakableContainer.GetRandomSmallBreakable();

                            float yMultiplied = parentPanel.transform.rotation == Quaternion.identity ? 4.0f : 2.0f;
                            Vector3 offset = new Vector3(0.0f, bounds.size.y / yMultiplied, 0.0f);
                            offset.y = spaces % 2 == 0 ? offset.y * -1.0f : offset.y;

                            InstantiatePanel(prefab, parentPanel.transform.parent.gameObject,
                                parentPanel.transform.localPosition + offset, parentPanel.transform.rotation);
                            spaces -= 1;
                        }
                    }
                }
                break;
            case PanelSize.SMALL:
                {
                    prefab = _breakableContainer.GetRandomSmallBreakable();
                    InstantiatePanel(prefab, parentPanel.transform.parent.gameObject,
                        parentPanel.transform.localPosition, parentPanel.transform.rotation);
                }
                break;
            default:
                break;
        }

        Destroy(parentPanel);
    }
}
