using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Grid
{
    public int width;
    public int height;
    private float scale;
    public static GridCell[,] cells;
    public static List<GridCell> bombCells = new List<GridCell>();

    private int numOfBombs;

    public static Grid Instance { get; private set; }

    public Grid(int width, int height, float scale, Vector2 offset, int numOfBombs) {
        Instance = this;

        this.width = width;
        this.height = height;
        this.scale = scale;
        this.numOfBombs = numOfBombs;

        cells = new GridCell[width, height];

        // Create grid
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                GridCell cell = CreateGridCell(x, y, scale, offset);
                cells[x, y] = cell;
            }
        }

        // Set neighbours
        foreach (GridCell cell in cells) {
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    if (cell.x + x >= 0 && cell.x + x < width && cell.y + y >= 0 && cell.y + y < height) {
                        cell.neighbouringCells.Add(cells[cell.x + x, cell.y + y]);
                    }
                }
            }
        }
    }

    public void SetBombs() {
        for (int i = 0; i < numOfBombs; i++) {
            GridCell cell = cells[Random.Range(0, width), Random.Range(0, height)];
            if (bombCells.Contains(cell) || cell.GetCellType() == CellType.Empty) {
                i--;
            } else {
                bombCells.Add(cell);
                cell.SetCellType(CellType.Bomb);
            }
        }

        SetNumbers();
    }

    private void SetNumbers() {
        for (int i = 0; i < cells.Length; i++) {
            GridCell cell = cells[i % width, i / width];
            if (cell.GetCellType() != CellType.Bomb) {
                int count = 0;
                foreach (GridCell neighbour in cell.neighbouringCells) {
                    if (neighbour.GetCellType() == CellType.Bomb) {
                        count++;
                    }
                }
                cell.SetCellType((CellType)count);
            }
        }
    }

    private GridCell CreateGridCell(int x, int y, float scale, Vector2 offset) {
        string name = "GridCell " + x.ToString() + " " + y.ToString();
        GameObject cell = new GameObject(name, typeof(SpriteRenderer));
        cell.transform.parent = GameObject.Find("GridParent").transform;

        cell.transform.position = new Vector3(x, y) * scale + new Vector3(offset[0], offset[1]);
        cell.transform.localScale = Vector3.one * scale;

        GridCell gridCell = cell.AddComponent<GridCell>();
        gridCell.x = x;
        gridCell.y = y;

        cell.AddComponent<BoxCollider2D>();

        GameManager.gridCells.Add(gridCell);

        return gridCell;
    }

    public static void ShowAllBombs() {
        foreach (GridCell cell in bombCells) {
            cell.SetSprite(GameAssets.Instance.MineCellSprite);
        }
    }


}
