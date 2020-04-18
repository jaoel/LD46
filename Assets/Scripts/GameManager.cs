using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<PanelConfiguration> _panelConfigurations = new List<PanelConfiguration>();

    [SerializeField]
    private BreakableContainer _breakableContainer;

    [SerializeField]
    private int _maxDeadBreakables = 0;

    [SerializeField]
    private float _timelimit = 20.0f;
    private float _startTime;

    [SerializeField]
    private float _deadTimelimit = 20.0f;
    private float _deadTimePassed = 0.0f;

    private bool _paused = false;

    [SerializeField]
    private GameObject _baseStation;

    [SerializeField]
    private Canvas _UI;

    private List<BreakableComponent> _breakableComponents = new List<BreakableComponent>();

    public int BrokenComponentCount => _breakableComponents.Where(x => x.State == BreakableState.BROKEN).Count();
    public int DeadComponentCount => _breakableComponents.Where(x => x.State == BreakableState.DEAD).Count();

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            _breakableComponents[0].SetState(BreakableState.BROKEN);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (_paused)
        {
            return;
        }

        if (Time.time - _startTime >= _timelimit)
        {
            Debug.Log("U WIN LOL");
        }

        if (DeadComponentCount > _maxDeadBreakables)
        {
            _deadTimePassed += Time.deltaTime;

            if (_deadTimePassed >= _deadTimelimit)
            {
                Debug.Log("U R DED");
            }
        }
        else
        {
            _deadTimePassed = 0.0f;
        }
    }

    private void TogglePause()
    {
        _paused = !_paused;

        _UI.gameObject.SetActive(_paused);
        if (_paused)
        {
            Time.timeScale = 0.0f;
            Physics.queriesHitTriggers = false;
        }
        else
        {
            Time.timeScale = 1.0f;
            Physics.queriesHitTriggers = true;
        }
    }

    private void GenerateStation()
    {
        _breakableComponents = new List<BreakableComponent>();
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

        _startTime = Time.time;
    }

    private GameObject InstantiatePanel(GameObject prefab, GameObject parent, Vector3 position, Quaternion rotation)
    {
        GameObject instantiated = Instantiate(prefab, parent.transform, false);
        instantiated.transform.rotation = rotation;
        instantiated.transform.localPosition = position;

        _breakableComponents.Add(instantiated.GetComponent<BreakableComponent>());

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
                                parentPanel.transform.localPosition - offset, parentPanel.transform.rotation);
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
        }

        Destroy(parentPanel);
    }
}
