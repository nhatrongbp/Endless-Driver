using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animator;
    // Update is called once per frame
    public void LoadLevel(string str)
    {
        StartCoroutine(LoadLevelCO(str));
    }

    IEnumerator LoadLevelCO(string str){
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(str);
    }
}
