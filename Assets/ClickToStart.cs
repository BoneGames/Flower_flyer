using UnityEngine.SceneManagement;
using UnityEngine;

public class ClickToStart : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(1);
        }
    }
}
