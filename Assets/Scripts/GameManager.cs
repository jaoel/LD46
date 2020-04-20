using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public static GameManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }


    public GameObject sparkEffectPrefab;

    [SerializeField]
    private List<PanelConfiguration> _panelConfigurations = new List<PanelConfiguration>();

    [SerializeField]
    private BreakableContainer _breakableContainer;

    [SerializeField]
    private int _maxDeadBreakables = 0;

    [SerializeField]
    private List<float> _timeLimits = new List<float>();

    [SerializeField]
    private int _maxLevels = 6;

    private float _timelimit = float.MaxValue;
    private float _startTime;

    [SerializeField]
    private float _deadTimelimit = 20.0f;

    [SerializeField]
    private GameObject _baseStation;

    [SerializeField]
    private UIManager _uiManager;

    [SerializeField]
    private float _minBreakInterval = 5.0f;

    [SerializeField]
    private float _maxBreakInterval = 10.0f;

    [SerializeField]
    private AudioSource _siren;
    [SerializeField]
    private AudioSource _scream;
    [SerializeField]
    private AudioSource _explosions;

    [SerializeField]
    private FirstPersonCamera _camera;

    [SerializeField]
    private List<string> _storyText = new List<string>();

    private List<BreakableComponent> _breakableComponents = new List<BreakableComponent>();
    private PanelConfiguration _panelConfiguration;

    private int _currentLevel;
    private bool _paused = false;
    private float _deadStartTime = 0.0f;
    private bool _dead = false;
    private float _timeSinceLastBreak = 0.0f;

    public int Day => _currentLevel + 1;
    public int FunctionalComponentCount => _breakableComponents.Where(x => x.State == BreakableState.FUNCTIONAL).Count();
    public int BrokenComponentCount => _breakableComponents.Where(x => x.State == BreakableState.BROKEN).Count();
    public int DeadComponentCount => _breakableComponents.Where(x => x.State == BreakableState.DEAD).Count();
    public bool Dying => _deadStartTime > 0.0f;
    public bool Dead => _dead;
    public float DayCompleteAmount => Mathf.Clamp01((Time.time - _startTime) / _timelimit);
    public float DeadTimer => Mathf.Max(0f, _deadTimelimit - (Time.time - _deadStartTime));

    private void Awake()
    {
        Physics.queriesHitTriggers = true;
        _currentLevel = 0;

        if (_instance == null) {
            _instance = this;
        }
    }

    private void OnDestroy() {
        if(_instance == this) {
            _instance = null;
        }
    }

    void Start()
    {
        StartGame();
    }

    public void InstantiateSparks(Vector3 position, Quaternion rotation) {
        GameObject instance = Instantiate(sparkEffectPrefab, position, rotation);
        Destroy(instance, 3f);
    }

    private void StartGame()
    {
        _dead = false;
        _paused = false;
        _deadStartTime = 0.0f;

        _currentLevel = 0;
        _uiManager.FadeText(4.0f, true, GetLevelText(), () =>
        {
            if (_panelConfiguration != null)
            {
                Destroy(_panelConfiguration.gameObject);
            }

            GenerateStation();
            Time.timeScale = 1.0f;
            _uiManager.Fade(2.0f, true, GetLevelText(), () =>
            {
                Physics.queriesHitTriggers = true;
                _startTime = Time.time;
            });
        });
    }

    void Update()
    {
        if (_dead)
        {
            _siren.volume = Mathf.Clamp(_siren.volume - 0.5f * Time.unscaledDeltaTime, 0.25f, 1.0f);

            if (Input.GetKeyDown(KeyCode.Escape)) {
                LoadMainMenu();
            }

            if (Input.GetKeyDown(KeyCode.Return) ||Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                AudioManager.Instance.PlayMusic("CalmGameplay");
                _uiManager.FadeText(1.0f, false, "", () =>
                {
                    Time.timeScale = 1.0f;
                    SceneManager.LoadScene("DevScene");
                });
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !_uiManager.Fading)
        {
            TogglePause();
        }

        if (_paused)
        {
            if (Input.GetKeyDown(KeyCode.Return)) {
                LoadMainMenu();
            }

            return;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            _startTime = float.MinValue; 
        }

        BreakSomething();

        if (Time.time - _startTime >= _timelimit)
        {
            HandleWin();
        }

        if (DeadComponentCount > _maxDeadBreakables)
        {
            if (_deadStartTime == 0f) {
                _deadStartTime = Time.time;
                AudioManager.Instance.PlayMusic("AlertGameplay");
                _camera.Shake(0.1f, 0.05f);
            }
            _siren.volume = Mathf.Clamp(_siren.volume + 1.0f * Time.deltaTime, 0.0f, 1.0f);
            if (Time.time > _deadStartTime + _deadTimelimit)
            {
                HandleLoss();
            }
        }
        else
        {
            if (_deadStartTime > 0.0f)
            {
                AudioManager.Instance.PlayMusic("CalmGameplay", true, 1.0f);
            }

            _siren.volume = Mathf.Clamp(_siren.volume - 1.0f * Time.deltaTime, 0.0f, 1.0f);
            _deadStartTime = 0.0f;
        }
    }

    public void LoadMainMenu() {
        Time.timeScale = 0.0f;
        _uiManager.Fade(4.0f, false, "I can't take this anymore! I quit!", () => {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("MainMenuScene");
        });
    }

    private void BreakSomething()
    {
        _timeSinceLastBreak += Time.deltaTime;

        if (_timeSinceLastBreak < _minBreakInterval)
        {
            return;
        }
        else
        {
            if (_timeSinceLastBreak >= _maxBreakInterval || UnityEngine.Random.Range(0.0f, 1.0f) <= _timeSinceLastBreak / _maxBreakInterval)
            {
                _timeSinceLastBreak = 0.0f;

                List<BreakableComponent> functionalBreakables = _breakableComponents
                    .Where(x => x.State == BreakableState.FUNCTIONAL).ToList();

                if (functionalBreakables.Count > 0)
                {
                    functionalBreakables[Random.Range(0, functionalBreakables.Count)].SetState(BreakableState.BROKEN);
                }
            }
        }
    }

    private void HandleWin()
    {
        Time.timeScale = 0.0f;
        _startTime = Time.time;

        _currentLevel++;

        if (_currentLevel > _maxLevels)
        {
            _uiManager.Fade(2.0f, false, "You have completed our game, yay\nPress Enter to return to Main Menu", () =>
            {
                Destroy(_panelConfiguration.gameObject);
                _paused = true;
            });
        }
        else
        {
            _uiManager.Fade(2.0f, false, "Time to go home...", () =>
            {
                Destroy(_panelConfiguration.gameObject);
                GenerateStation();

                _uiManager.FadeText(2.0f, false, "", () =>
                {
                    Time.timeScale = 1.0f;
                    _uiManager.FadeText(2.0f, true, GetLevelText(), () =>
                    {
                        _uiManager.Fade(2.0f, true, GetLevelText(), () =>
                        {
                            _startTime = Time.time;
                        });
                    });
                });
            });
        }
    }

    private string GetLevelText()
    {
        if (_maxLevels != int.MaxValue)
        {
            return "Day " + (_currentLevel + 1) + " of " + (_maxLevels + 1) + "...\n" + _storyText[_currentLevel];
        }
        else
        {
            return "Day " + (_currentLevel + 1) + "...";
        }
    }

    private void HandleLoss()
    {
        _camera.Shake(1.5f, 0.05f);
        AudioManager.Instance.PlayMusic("GameOver", true, 5.0f);
        _dead = true;

        if (UnityEngine.Random.Range(0, 100) < 10)
        {
            _scream.Play();
        }

        _explosions.Play();
        Time.timeScale = 0.0f;
        _startTime = Time.time;
        _deadStartTime = 0.0f;

        _uiManager.Fade(2.0f, false, "You are dead\nPress Escape to exit\nPress Enter or Mouse to restart");
    }

    private void TogglePause()
    {
        _paused = !_paused;
        _uiManager.TogglePause(_paused);
    }

    private void GenerateStation()
    {
        Verlet.VerletPhysicsManager.Clear();
        _dead = false;
        _breakableComponents = new List<BreakableComponent>();
        _timelimit = _timeLimits[Mathf.Clamp(_currentLevel, 0, _timeLimits.Count - 1)];

        if (_currentLevel >= 2)
        {
            _panelConfiguration = Instantiate(_panelConfigurations[Random.Range(2, _panelConfigurations.Count)], 
                _baseStation.transform);
        }
        else
        {
            _panelConfiguration = Instantiate(
            _panelConfigurations[Mathf.Clamp(_currentLevel, 0, _panelConfigurations.Count - 1)], _baseStation.transform);
        }

        _panelConfiguration.LargePanels.ForEach(x =>
        {
            CreateRandomBreakables(_panelConfiguration, PanelSize.LARGE, x);
        });

        _panelConfiguration.MediumPanels.ForEach(x =>
        {
            CreateRandomBreakables(_panelConfiguration, PanelSize.MEDIUM, x);
        });

        _panelConfiguration.SmallPanels.ForEach(x =>
        {
            CreateRandomBreakables(_panelConfiguration, PanelSize.SMALL, x);
        });

        _panelConfiguration.MediumLandscapePanels.ForEach(x =>
        {
            CreateRandomBreakables(_panelConfiguration, PanelSize.MEDIUM_LANDSCAPE, x);
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
                        if( random <= 8 && spaces == 4)
                        {
                            prefab = _breakableContainer.GetRandomLargeBreakable();
                            InstantiatePanel(prefab, parentPanel.transform.parent.gameObject,
                                parentPanel.transform.localPosition, parentPanel.transform.rotation);
                            spaces -= 4;
                        }
                        else if (random > 6 && random < 9 && spaces % 2 == 0)
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
                        if (random >= 3 && spaces == 2)
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
            case PanelSize.MEDIUM_LANDSCAPE: {
                    prefab = _breakableContainer.GetRandomMediumLandscapeBreakable();
                    InstantiatePanel(prefab, parentPanel.transform.parent.gameObject,
                        parentPanel.transform.localPosition, parentPanel.transform.rotation);
                }
                break;
        }

        Destroy(parentPanel);
    }
}
