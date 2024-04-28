using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class GridCell : MonoBehaviour
{
    private int x;
    private int y;
    private float scale;
    [SerializeField] private CellType cellType = CellType.Default;
    private bool opened;
    private bool flagged;
    

    private List<GridCell> neighbouringCells = new();

    public void InitGridCell(int x, int y, float scale) {
        this.x = x;
        this.y = y;
        this.scale = scale;
    
        SetSprite(GameAssets.Instance.DefaultCellSprite);
    }

    private void Awake() {
        InitBoxCollider2D();
    }

    public void SetSprite(Sprite sprite) {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetCellType(CellType celltype) {
        this.cellType = celltype;
    }

    public CellType GetCellType() {
        return cellType;
    }

    public void ShowCell() {
        ShowCell(new());
    }

    public void ShowCell(List<GridCell> gridCellsThatActivatedThis) {
        if (!opened) GameManager.IncrementOpenedCells();
        opened = true;
        switch (cellType) {
            case CellType.Empty:
                SetSprite(GameAssets.Instance.ClickedCellSprite);

                ShowEmptyCellLogic(gridCellsThatActivatedThis);
                break;
            case CellType.One:
                SetSprite(GameAssets.Instance.OneCellSprite);
                break;
            case CellType.Two:
                SetSprite(GameAssets.Instance.TwoCellSprite);
                break;
            case CellType.Three:
                SetSprite(GameAssets.Instance.ThreeCellSprite);
                break;
            case CellType.Four:
                SetSprite(GameAssets.Instance.FourCellSprite);
                break;
            case CellType.Five:
                SetSprite(GameAssets.Instance.FiveCellSprite);
                break;
            case CellType.Six:
                SetSprite(GameAssets.Instance.SixCellSprite);
                break;
            case CellType.Seven:
                SetSprite(GameAssets.Instance.SevenCellSprite);
                break;
            case CellType.Eight:
                SetSprite(GameAssets.Instance.EightCellSprite);
                break;
            case CellType.Bomb:
                SetSprite(GameAssets.Instance.BombCellSprite);
                break;
        }
    }

    private void ShowEmptyCellLogic(List<GridCell> gridCellsThatActivatedThis) {
        if (!gridCellsThatActivatedThis.Contains(this)) {
            gridCellsThatActivatedThis.Add(this);
            foreach (GridCell neighbour in neighbouringCells) {
                neighbour.ShowCell(gridCellsThatActivatedThis);
            }
        }
    }

    private void InitBoxCollider2D() {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(1, 1);
    }

    public int GetX() {
        return x;
    }

    public int GetY() {
        return y;
    }

    public void SetY(int y) {
        this.y = y;
    }

    public bool IsFlagged() {
        return flagged;
    }

    public void SetFlagged(bool flagged) {
        this.flagged = flagged;
    }

    public List<GridCell> GetNeighbouringCells() {
        return neighbouringCells;
    }

    public bool IsOpened() {
        return opened;
    }
}