using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxer : MonoBehaviour
{
    public List<Material> skyboxes = new List<Material>();
    [SerializeField] private int currentSkybox;
    private UI ui;

    private void Awake()
    {
        ui = FindObjectOfType<UI>();
    }
    private void Start()
    {
        currentSkybox = 0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            UpdateCurrentIndex(-1);
            RenderSettings.skybox = skyboxes[currentSkybox];
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            UpdateCurrentIndex(1);
            RenderSettings.skybox = skyboxes[currentSkybox];
        }
    }

    void UpdateCurrentIndex(int dir)
    {
        int total = currentSkybox + dir;
        if (total >= skyboxes.Count)
        {
            currentSkybox = 0;
        }
        else if (total < 0)
        {
            currentSkybox = skyboxes.Count - 1;
        }
        else
        {
            currentSkybox = total;
        }
        // set skybox label here (with new text component)

        ui.SetSkyboxName(skyboxes[currentSkybox].name);
    }
}
