using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour
{
    public bool isPaused;
    public bool isEnd;
    public UIDocument UIDoc;
    public UIDocument endUI;
    
    private Button buttonResume;
    private Button buttonExit;

    private Button buttonRestart;
    
    // Start is called before the first frame update
    void Start()
    {
        PauseMenu(false);
        EndMenu(false);
        
        // Pause menu
        VisualElement root = UIDoc.rootVisualElement;
        
        
        buttonResume = root.Q<Button>("Resume");
        buttonExit = root.Q<Button>("Exit");
        
        buttonResume.clicked += () => PauseMenu(false);
        buttonExit.clicked += () => QuitGame();
        
        // End game menu
        VisualElement endRoot = endUI.rootVisualElement;
        
        buttonRestart = endRoot.Q<Button>("Restart");
        buttonExit = endRoot.Q<Button>("Exit");
        
        buttonRestart.clicked += () =>
        {
            SceneManager.LoadScene("MainScene");
        };
        buttonExit.clicked += () => QuitGame();
    }
    
    void PauseMenu(bool paused)
    {
        if (paused)
        {
            UIDoc.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        else
        {
            UIDoc.rootVisualElement.style.display = DisplayStyle.None;
        }
        PauseGame(paused);
    }

    void EndMenu(bool ended)
    {
        if (ended)
        {
            endUI.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        else
        {
            endUI.rootVisualElement.style.display = DisplayStyle.None;
        }
        PauseGame(ended);
    }

    void PauseGame(bool paused)
    {
        if (paused)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
        
        isPaused = paused;
        Time.timeScale = paused ? 0 : 1;
    }
    
    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                PauseMenu(false);
            }
            else
            {
                PauseMenu(true);
            }
        }

        if (isEnd)
        {
            EndMenu(true);
        }
    }
}