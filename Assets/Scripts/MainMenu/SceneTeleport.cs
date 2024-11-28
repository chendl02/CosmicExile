using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTeleport : MonoBehaviour
{
    public string sceneToLoad; // Name of the scene to load
    public string currentSceneName; // Name of current scene
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneToLoad);
    }


}
