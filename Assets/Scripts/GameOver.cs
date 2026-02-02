using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameOver : MonoBehaviour
{
    public ShootingController shootingController;

    public void SetUp()
    {
        gameObject.SetActive(true);
    }
    public void RestartButton()
    {
        // this should restart the game scene
        //SceneManager.LoadScene("SampleScene");
        transform.parent.gameObject.SetActive(false);
        shootingController.runGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
