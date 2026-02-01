using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameOver : MonoBehaviour
{
    
    public void SetUp()
    {
        gameObject.SetActive(true);
    }
    public void RestartButton()
    {
        // this should restart the game scene
        SceneManager.LoadScene("SampleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
