using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Panel")]
    public GameObject mainMenuPanel;
    public List<GameObject> allPanels;

    private Stack<GameObject> panelHistory = new Stack<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ShowPanel(mainMenuPanel);
    }

    public void ShowPanel(GameObject panelToShow)
    {
       foreach (GameObject panel in allPanels)
        {
            panel.SetActive(false);
        }

       if (panelToShow != null)
            panelHistory.Push(panelToShow);
    }

    public void CLoseCurrentPanel()
    {
        if (panelHistory.Count > 0)
        {
            panelHistory.Pop().SetActive(false);
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
