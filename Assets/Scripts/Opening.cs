using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    public GameObject openingCanvas;
    public DialogueEntity openingDialogue;

    public GameObject[] endingCanvas;
    public DialogueEntity[] endingDialogue;
    public Sprite[] endingSprite;

    private void Start()
    {
        StartCoroutine(DoStart());
    }

    IEnumerator DoStart()
    {
        yield return new WaitUntil(() => InGameTracker.instance != null);
        openingDialogue.ExecuteInteractable();
        InGameTracker.instance.state = GameState.Dialogue;
    }

    public void FadeOutToPlay()
    {
        CanvasGroup cg = openingCanvas.GetComponent<CanvasGroup>();

        DOTween.To(() => cg.alpha, x => cg.alpha = x, 0, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            InGameTracker.instance.state = GameState.Playing;
            openingCanvas.SetActive(false);
        });
    }

    public void EndingDialogue(int endingIndex)
    {
        InGameTracker.instance.state = GameState.Stop;
        endingCanvas[endingIndex].SetActive(true);
        endingCanvas[endingIndex].GetComponent<Image>().sprite = endingSprite[endingIndex];
        endingCanvas[endingIndex].GetComponentInChildren<Button>().onClick.AddListener(GoToMainMenu);
        CanvasGroup cg = endingCanvas[endingIndex].GetComponent<CanvasGroup>();

        DOTween.To(() => cg.alpha, x => cg.alpha = x, 1, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            endingDialogue[endingIndex].ExecuteInteractable();
            InGameTracker.instance.state = GameState.Stop;
            
        });
    }

    void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
