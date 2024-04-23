using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public int x;
    public int y;
    private Sprite sprite;
    [SerializeField] private CellType cellType = CellType.Default;

    public List<GridCell> neighbouringCells = new List<GridCell>();

    private void Awake() {
        ShowCell(new List<GridCell>());
    }

    public void SetSprite(Sprite sprite) {
        this.sprite = sprite;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetCellType(CellType celltype) {
        this.cellType = celltype;
    }

    public CellType GetCellType() {
        return cellType;
    }

    public void ShowCell(List<GridCell> gridCellsThatActivatedThis) {
        switch (cellType) {
            case CellType.Default:
                SetSprite(GameAssets.Instance.DefaultCellSprite);
                break;
            case CellType.Empty:
                SetSprite(GameAssets.Instance.ClickedCellSprite);

                if (!gridCellsThatActivatedThis.Contains(this)) {
                    gridCellsThatActivatedThis.Add(this);
                    foreach (GridCell neighbour in neighbouringCells) {
                        neighbour.ShowCell(gridCellsThatActivatedThis);
                        
                    }
                }
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
                SetSprite(GameAssets.Instance.MineCellSprite);
                break;
        }
    }
}
