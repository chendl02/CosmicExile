using UnityEngine;

public class AboutGame : MonoBehaviour
{
    public GameObject AboutPanel; 

    public void ShowAbout()
    {
        AboutPanel.SetActive(true);
    }

    public void HideAbout()
    {
        AboutPanel.SetActive(false);
    }
}
