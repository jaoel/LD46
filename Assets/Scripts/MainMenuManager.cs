using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private UIManager _uiManager;
    private void Start()
    {
    }

    private void Update()
    {
        
    }

    public void OnStartPressed()
    {
        _uiManager.Fade(2.0f, false, "", () =>
        {
            SceneManager.LoadScene("DevScene");
        });
    }

    public void OnExitPressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
}
