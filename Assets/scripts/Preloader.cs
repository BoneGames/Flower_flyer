using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preloader : MonoBehaviour
{
    public int currentSceneIndex;
    public string _sceneName;
    AsyncOperation _asyncOperation;
    void OnEnable()
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void OnSceneChanged(Scene cur, Scene next)
    {
        print($"{next.name}: Index: {next.buildIndex}");
        //currentSceneIndex = next.buildIndex;

        if(currentSceneIndex == 1)
        {
            StartCoroutine(LoadSceneAsyncProcess());
        }

        if (next.buildIndex == 2)
            Destroy(this.gameObject);
    }

    string output;
    bool canEnterGame = false;
    private IEnumerator LoadSceneAsyncProcess()
    {
        yield return null;
        output = "Start Preloading";
        Debug.Log(output);

        _asyncOperation = SceneManager.LoadSceneAsync(_sceneName);
        _asyncOperation.allowSceneActivation = false;

        while (_asyncOperation.progress < 0.9f)
        {
            print($"[scene]:{_sceneName} [load progress]: {_asyncOperation.progress}");

            yield return null;
        }
        canEnterGame = true;
        output = "Preloading Done";
        Debug.Log(output);
    }
    public bool onGUI = true;
    //private void OnGUI()
    //{
    //    if (onGUI)
    //        GUI.Label(new Rect(300, 10, 200, 40), output);
    //}

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.Return))
        {
            switch (currentSceneIndex)
            {
                case 0:
                    SceneManager.LoadScene(++currentSceneIndex);
                    break;
                case 1:
                    if (!canEnterGame)
                    {
                        print("cant load scene yet");
                        return;
                    }
                    ++currentSceneIndex;
                    //SceneManager.LoadScene(currentSceneIndex);
                    //    break;
                    //case 2:
                    //++currentSceneIndex;
                    output = "Activate Loaded Game";
                    print(output);
                    _asyncOperation.allowSceneActivation = true;
                    break;
            }
        }
    }
}