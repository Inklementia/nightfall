using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMainMenu : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private Animator musicTransition;

    public void MainMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(TransitionFade());
    }

    IEnumerator TransitionFade()
    {
        transition.SetTrigger("start");
        musicTransition.SetTrigger("fadeOut");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(0);
    }
}
