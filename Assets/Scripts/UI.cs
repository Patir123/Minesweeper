using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverParent;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI youWinText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;

    private void Start() {
        GameManager.OnGameOver.AddListener(OnGameOver);
        GameManager.OnGameWin.AddListener(OnGameWin);
    }

    private void OnGameOver() {
        gameOverParent.SetActive(true);
    }

    private void OnGameWin() {
        gameOverParent.SetActive(true);
        youWinText.gameObject.SetActive(true);
    }

    public void Retry() {
        gameOverParent.SetActive(false);
        youWinText.gameObject.SetActive(false);

        GameManager.Instance.StartGame();
        GameManager.Instance.gameObject.SetActive(true);
    }

    public void Quit() {
        Application.Quit();
    }
}
