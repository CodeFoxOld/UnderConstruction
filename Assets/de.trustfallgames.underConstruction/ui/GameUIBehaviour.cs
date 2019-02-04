using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using TMPro;
using UnityEngine;

public class GameUIBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _popupScore;
    private TextMeshProUGUI _scoreText;
    private GameManager _gameManager;
    private int baseScore;
    
    void Start()
    {
        _gameManager = GameManager.GetManager();
        _scoreText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        baseScore = _gameManager.Settings.BasePoint;
        
        ChangeScore(0);
    }
    
    public void ChangeScore(int newScore)
    {
        _scoreText.SetText(newScore.ToString());
    }

    public void PopScoreWithMultiplier(int multiplier)
    {
        GameObject popup = GameObject.Instantiate(_popupScore);
        
        popup.transform.SetParent(GameObject.Find("UI(Clone)").transform, false);
        popup.transform.position = new Vector3(0, -200, 0);

        TextMeshProUGUI scoreOutput = popup.GetComponent<TextMeshProUGUI>();
        scoreOutput.SetText(multiplier.ToString() + " X " + baseScore);  
    }
}
