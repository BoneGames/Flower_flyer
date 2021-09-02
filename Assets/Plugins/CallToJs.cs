using System.Runtime.InteropServices;
using UnityEngine;

public class CallToJs : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void JavaScriptLibraryFunction();

    private void Start()
    {
        #if !UNITY_EDITOR
            JavaScriptLibraryFunction();
        #endif
    }
}
