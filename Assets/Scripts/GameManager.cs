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
    }

    private void Update() {
        if (GameOver) {
            OnGameOver?.Invoke();
            gameObject.SetActive(false);
        }
        else if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null) {
                GridCell clickedGridCell = hit.collider.GetComponent<GridCell>();
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