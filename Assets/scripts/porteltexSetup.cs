//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class porteltexSetup : MonoBehaviour
//{
//    [System.Serializable]
//    public struct Cam_Mat
//    {
//        public string direction;
//        public Camera portalCam;
//        public Material material;
//        public Text active;
//    }

//    public List<Cam_Mat> camMats = new List<Cam_Mat>();

//    private void Awake()
//    {
//        foreach(Cam_Mat cM in camMats)
//        {
//            Setup(cM);
//        }
//    }

//    void Setup(Cam_Mat cM)
//    {
//        if (cM.portalCam.targetTexture != null)
//        {
//            cM.portalCam.targetTexture.Release();
//        }
//        cM.portalCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
//        cM.material.mainTexture = cM.portalCam.targetTexture;
//    }

//    public void ActivatePortalCam(List<string> tagsToRender)
//    {
//        foreach (Cam_Mat c in camMats)
//        {
//            c.portalCam.enabled = tagsToRender.Contains(c.portalCam.tag) ? true : false;
//            c.active.text = c.direction + ": " + c.portalCam.enabled;
//        }
//    }
//}
