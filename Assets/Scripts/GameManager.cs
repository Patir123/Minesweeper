using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static UnityEvent OnGameOver = new UnityEvent();

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float scale;
    [SerializeField] private Vector2 offset;
    [SerializeField] private int numOfBombs;

    public static bool FirstClick = true;
    public static bool GameOver = false;
    public static int openedCells = 0;
    private static int cellsToOpen;
    private static float defaultTimeForFlag = .5f;

    private static Grid grid;

    public static List<GridCell> gridCells = new List<GridCell>();

    public static GameManager Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StartGame();
    }

    public void StartGame() {
        GameOver = false;
        FirstClick = true;

        if (grid != null) {
            foreach (GridCell gridCell in gridCells) {
                Destroy(gridCell.gameObject);
            }

            gridCells.Clear();
        }

        grid = new Grid(width, height, scale, offset, numOfBombs);
        cellsToOpen = width * height - numOfBombs;
    }

    float timeForFlag = defaultTimeForFlag;
    private void Update() {
        print("Opened Cells: " + openedCells + " Cells to Open: " + cellsToOpen);
        if (openedCells == cellsToOpen) GameOver = true;

        if (GameOver) {
            OnGameOver?.Invoke();
            Grid.ShowAllBombs();
            gameObject.SetActive(false);
        } else if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
            if (hit.collider != null) {
                GridCell clickedGridCell = hit.collider.GetComponent<GridCell>();
                float timeRemaining = timeForFlag - Time.deltaTime;

                if (timeRemaining <= 0) {
                    timeForFlag = defaultTimeForFlag;
                    if (clickedGridCell.GetCellType() == CellType.Empty) return;

                    Handheld.Vibrate();
                    if (clickedGridCell.flagged) {
                        clickedGridCell.SetSprite(GameAssets.Instance.DefaultCellSprite);
                        clickedGridCell.flagged = false;
                    } else {
                        clickedGridCell.SetSprite(GameAssets.Instance.FlaggedCellSprite);
                        clickedGridCell.flagged = true;
                    }
                } 
                else if (touch.phase == TouchPhase.Ended && !clickedGridCell.flagged) {
                    timeForFlag = defaultTimeForFlag;
                    if (FirstClick) {
                        FirstClick = false;
                        clickedGridCell.SetCellType(CellType.Empty);

                        foreach (GridCell gridCell in clickedGridCell.neighbouringCells) {
                            gridCell.SetCellType(CellType.Empty);
                        }
                        grid.SetBombs();

                        clickedGridCell.ShowCell(new List<GridCell>());
                    } else if (clickedGridCell.GetCellType() == CellType.Bomb) {
                        clickedGridCell.SetSprite(GameAssets.Instance.MineCellSprite);
                        GameOver = true;
                    } else {
                        clickedGridCell.ShowCell(new List<GridCell>());
                    }
                }
                else {
                    timeForFlag -= Time.deltaTime;
                }
            }
        }
    }
}

public enum CellType {
    Empty,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Bomb,
    Default
}