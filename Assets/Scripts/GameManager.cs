using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static UnityEvent OnGameOver = new();
    public static UnityEvent OnGameWin = new();

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float scale;
    [SerializeField] private Vector2 offset;
    [SerializeField] private int numOfBombs;

    private static bool FirstClick = true;
    private static int openedCells = 0;
    private static int cellsToOpen;
    private static Grid grid;

    private static List<GridCell> gridCells = new();
    public static GameManager Instance { get; private set; }

    private const float Default_Time_For_Flag = .5f;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        StartGame();
        OnGameOver.AddListener(GameOver);
        OnGameWin.AddListener(GameWin);
    }

    public void StartGame() {
        FirstClick = true;

        if (grid != null) {
            foreach (GridCell gridCell in gridCells) {
                Destroy(gridCell.gameObject);
            }

            gridCells.Clear();
            openedCells = 0;
        }

        grid = new Grid(width, height, scale, offset, numOfBombs);
        cellsToOpen = width * height - numOfBombs;
    }

    float timeForFlag = Default_Time_For_Flag;
    private bool placedAFlagInThisHolding = false;
    private void Update() {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Vector2.zero);
            if (hit.collider != null) {
                GridCell clickedGridCell = hit.collider.GetComponent<GridCell>();
                float timeRemaining = timeForFlag - Time.deltaTime;

                if (timeRemaining <= 0 && !placedAFlagInThisHolding) {
                    timeForFlag = Default_Time_For_Flag;
                    placedAFlagInThisHolding = true;
                    if (clickedGridCell.IsOpened()) return;

                    Handheld.Vibrate();
                    if (clickedGridCell.IsFlagged()) {
                        Sprite defaultCellSprite = GameAssets.Instance.DefaultCellSprite;
                        clickedGridCell.SetSprite(defaultCellSprite);
                        clickedGridCell.SetFlagged(false);
                        Grid.RemoveFlagCell(clickedGridCell);
                    } else {
                        Sprite flagCellSprite = GameAssets.Instance.FlaggedCellSprite;
                        clickedGridCell.SetSprite(flagCellSprite);
                        clickedGridCell.SetFlagged(true);
                        Grid.AddFlagCell(clickedGridCell);
                    }
                } 
                else if (touch.phase == TouchPhase.Ended && !clickedGridCell.IsFlagged() && !placedAFlagInThisHolding) {
                    timeForFlag = Default_Time_For_Flag;
                    if (FirstClick) {
                        FirstClick = false;
                        
                        List<GridCell> gridCellsToSkipWhenSettingBombs = new();

                        clickedGridCell.SetCellType(CellType.Empty);
                        gridCellsToSkipWhenSettingBombs.Add(clickedGridCell);

                        // open neighbouring cells (set them empty)
                        foreach (GridCell gridCell in clickedGridCell.GetNeighbouringCells()) {
                            gridCell.SetCellType(CellType.Empty);
                            gridCellsToSkipWhenSettingBombs.Add(gridCell);
                        }
                        grid.SetBombs(gridCellsToSkipWhenSettingBombs);

                        clickedGridCell.ShowCell();
                    } else if (clickedGridCell.GetCellType() == CellType.Bomb) {
                        Sprite bombCellSprite = GameAssets.Instance.BombCellSprite;
                        clickedGridCell.SetSprite(bombCellSprite);
                        OnGameOver?.Invoke();
                    } else {
                        clickedGridCell.ShowCell();
                    }
                }
                else {
                    timeForFlag -= Time.deltaTime;
                }
            }
        } else if (Input.touchCount == 0) {
            placedAFlagInThisHolding = false;
            timeForFlag = Default_Time_For_Flag;
        }

        if (openedCells == cellsToOpen) OnGameWin?.Invoke();
    }

    private void GameOver() {
        Grid.ShowAllBombs();
        gameObject.SetActive(false);
    }

    private void GameWin() {
        gameObject.SetActive(false);
    }

    public static void IncrementOpenedCells() {
        openedCells++;
    }

    public static List<GridCell> GetGridCells() {
        return gridCells;
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