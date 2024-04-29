using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private const string GRID_PARENT = "GridParent";

    private int width;
    private int height;
    private float scale;
    private static GridCell[,] cells;
    private static List<GridCell> bombCells = new();
    private static List<GridCell> flagCells = new();
    private int numOfBombs;

    public static Grid Instance { get; private set; }

    public Grid(int width, int height, float scale, int numOfBombs) {
        Instance = this;

        this.width = width;
        this.height = height;
        this.scale = scale;
        this.numOfBombs = numOfBombs;

        cells = new GridCell[width, height];

        // Create grid
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GridCell cell = CreateGridCell(x, y, scale);
                cells[x, y] = cell;
            }
        }

        // Set neighbours
        foreach (GridCell cell in cells) {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if (cell.GetX() + x >= 0 &&
                        cell.GetX() + x < width &&
                        cell.GetY() + y >= 0 &&
                        cell.GetY() + y < height
                        ) {
                        cell.GetNeighbouringCells().Add(cells[cell.GetX() + x, cell.GetY() + y]);
                    }
                }
            }
        }

        // fill list of every cell
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < height; j++) {
                gridCellsThatCanBeBombs.Add(cells[i, j]);
            }
        }

        bombCells.Clear();
        flagCells.Clear();
    }

    private List<GridCell> gridCellsThatCanBeBombs = new();
    public void SetBombs(List<GridCell> gridCellsToSkip) {
        // create list of cells that can be bombs
        List<GridCell> gridCellsWithoutBombs = new(gridCellsThatCanBeBombs);
        foreach (GridCell gridCell in gridCellsToSkip) {
            gridCellsWithoutBombs.Remove(gridCell);
        }

        // set bombs
        for (int i = 0; i < numOfBombs; i++) {
            int randomIndex = Random.Range(0, gridCellsWithoutBombs.Count);
            GridCell gridCell = gridCellsWithoutBombs[randomIndex];
            gridCellsWithoutBombs.RemoveAt(randomIndex);
            gridCell.SetCellType(CellType.Bomb);
            bombCells.Add(gridCell);
        }

        SetNumbers();
    }

    private void SetNumbers() {
        for (int x = 0; x < cells.GetLength(0); x++) {
            for (int y = 0; y < cells.GetLength(1); y++) {
                GridCell gridCell = cells[x, y];
                if (gridCell.GetCellType() == CellType.Bomb) continue;

                int count = 0;
                foreach (GridCell neighbour in gridCell.GetNeighbouringCells()) {
                    if (neighbour.GetCellType() == CellType.Bomb) count++;
                }

                gridCell.SetCellType((CellType)count);
            }
        }
    }

    private GridCell CreateGridCell(int x, int y, float scale) {
        string name = "GridCell " + x.ToString() + " " + y.ToString();

        Transform parent = GameObject.Find(GRID_PARENT).transform != null ?
            GameObject.Find(GRID_PARENT).transform :
            new GameObject(GRID_PARENT).transform;

        Vector3 position = new Vector3(x, y) * scale;

        GameObject gridCellGO = new(name);

        gridCellGO.transform.parent = parent;
        gridCellGO.transform.localScale = Vector3.one * scale;
        gridCellGO.transform.localPosition = position;

        GridCell gridCell = gridCellGO.AddComponent<GridCell>();
        gridCell.InitGridCell(x, y, scale);

        GameManager.GetGridCells().Add(gridCell);
        return gridCell;
    }

    public static void ShowAllBombs() {
        foreach (GridCell cell in bombCells) {
            if (cell.IsFlagged()) cell.SetSprite(GameAssets.Instance.CorrectBombCellSprite);
            else cell.ShowCell();
        }

        foreach (GridCell cell in flagCells) {
            if (cell.GetCellType() != CellType.Bomb) cell.SetSprite(GameAssets.Instance.WrongFlagCellSprite);
        }
    }

    public static void AddFlagCell(GridCell gridCell) {
        flagCells.Add(gridCell);
    }

    public static void RemoveFlagCell(GridCell gridCell) {
        flagCells.Remove(gridCell);
    }
}
