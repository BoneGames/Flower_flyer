using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private Text transitionState;
    [SerializeField] private Text boostState;
    [SerializeField] private Text soundZones;
   
    // active portal cams
    public Dictionary<string, Text> portalCamsActive = new Dictionary<string, Text>();
    public Transform portalCamsParent;

    // skybox
    public Text skyboxName;
    
    public void SetTransitionStateLabel(string state)
    {
        transitionState.text = state;
    }

    public void ShowSoundZones(string[] sounds)
    {
        soundZones.text = string.Join("\n", sounds);
    }

    public void SetBoostStateLabel(string state)
    {
        boostState.text = state;
    }

    public void SetSkyboxName(string name)
    {
        skyboxName.text = name;
    }

    public void SetActiveCamDisplay(string cam, bool enabled)
    {
        portalCamsActive[cam].color = enabled ?  Color.green : Color.red;
    }

    public void RegisterPortalCam(string dir)
    {
        Text activeCamDisplay = null;
        foreach (Transform t in portalCamsParent)
        {
            if(t.name == dir)
            {
                activeCamDisplay = t.GetComponent<Text>();
                break;
            }
        }
        activeCamDisplay.text = dir.Substring(0, 1);
        portalCamsActive.Add(dir, activeCamDisplay);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
