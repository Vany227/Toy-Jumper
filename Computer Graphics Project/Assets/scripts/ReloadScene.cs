using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public Animator animator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {

            animator.SetTrigger("isDead");
        }
    }

    //private void ResetLevel() {
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
}
