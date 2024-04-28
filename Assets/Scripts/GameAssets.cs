using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public Sprite DefaultCellSprite;
    public Sprite ClickedCellSprite;
    public Sprite FlaggedCellSprite;
    public Sprite WrongFlagCellSprite;
    public Sprite BombCellSprite;
    public Sprite CorrectBombCellSprite;
    public Sprite OneCellSprite;
    public Sprite TwoCellSprite;
    public Sprite ThreeCellSprite;
    public Sprite FourCellSprite;
    public Sprite FiveCellSprite;
    public Sprite SixCellSprite;
    public Sprite SevenCellSprite;
    public Sprite EightCellSprite;
}
