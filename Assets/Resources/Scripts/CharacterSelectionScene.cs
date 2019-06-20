using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class CharacterSelectionScene : MonoBehaviour
{
    public Player shou;
    public Player rikko;
    public Player emily;

    public Animator transitionAnimator;
    public void CharacterSelected()
    {
        if (EventSystem.current.currentSelectedGameObject.name.Equals("Shou"))
        {
            GameScene.player = shou;
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("Rikko"))
        {
            GameScene.player = rikko;
        }
        else if (EventSystem.current.currentSelectedGameObject.name.Equals("Emily"))
        {
            GameScene.player = emily;
        }
        else
        {
            Debug.LogError("CharacterSelected(): Button name is invalid! Defaulting to Shou...");
            GameScene.player = shou;
        }

        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        transitionAnimator.SetBool("SceneEnd", true);
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("GameScene");
    }
}
