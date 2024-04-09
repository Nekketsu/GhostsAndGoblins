using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{
    public Text TextScore;
    public Text TextTimeRemaining;
    public Image ImgLife0;
    public Image ImgLife1;
    public Image ImgLife2;

    private void Update()
    {
        this.TextScore.text = GameManager.Player.Score.ToString();
        this.TextTimeRemaining.text = GameManager.CurrentLevel.GetRemainingTimeFormatted();

        this.ImgLife0.enabled = GameManager.Player.Lives >= 1;
        this.ImgLife1.enabled = GameManager.Player.Lives >= 2;
        this.ImgLife2.enabled = GameManager.Player.Lives >= 3;
    }
}
