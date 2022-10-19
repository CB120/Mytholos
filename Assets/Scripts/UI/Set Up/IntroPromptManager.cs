using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

// This script fades in a canvas group, delays for some time, fades it out, and then loads a specified scene
// It receives player input to fast forward to fading out

public class IntroPromptManager : MonoBehaviour
{
    [SerializeField] float fadeInOutTime;
    [SerializeField] float readTextTime;
    [SerializeField] CanvasGroup body;
    [SerializeField] string nameOfNextScene;

    float timer;

    void Start()
    {
        body.alpha = 0.0f;
    }

    void Update()
    {
        if (timer < fadeInOutTime)
            body.alpha = timer / fadeInOutTime;
        else if (timer < fadeInOutTime + readTextTime)
            body.alpha = 1.0f;
        else if (timer < 2 * fadeInOutTime + readTextTime)
            body.alpha = 1 - ((timer - (fadeInOutTime + readTextTime)) / fadeInOutTime);
        else
        {
            body.alpha = 0.0f;
            SceneManager.LoadScene(nameOfNextScene);
        }

        timer += Time.deltaTime;
    }

    public void SkipIntro(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (timer >= fadeInOutTime + readTextTime) return;

        timer = fadeInOutTime + readTextTime;

        if (timer < fadeInOutTime)
            timer += fadeInOutTime - timer;
    }
}
