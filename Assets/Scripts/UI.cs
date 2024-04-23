using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button quitButton;

    private void Start() {
        GameManager.OnGameOver.AddListener(OnGameOver);
    }

    private void OnGameOver() {
        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    public void Retry() {
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);

        GameManager.Instance.StartGame();
        GameManager.Instance.gameObject.SetActive(true);
    }

    public void Quit() {
        Application.Quit();
    }
}
