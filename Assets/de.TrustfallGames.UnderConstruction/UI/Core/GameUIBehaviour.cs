using System;
using de.TrustfallGames.UnderConstruction.Core.CoreManager;
using de.TrustfallGames.UnderConstruction.Core.spawnManager;
using de.TrustfallGames.UnderConstruction.UI.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace de.TrustfallGames.UnderConstruction.UI.Core {
    public class GameUIBehaviour : MonoBehaviour {
        [SerializeField] private GameObject _popupScore;
        [SerializeField] private GameObject _hoverScore;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private TextMeshProUGUI _destructCount;
        [SerializeField] private TextMeshProUGUI _comboCounter;
        [SerializeField] private Button _baggerButtonLeft;
        [SerializeField] private Button _baggerButtonRight;
    
        private GameManager _gameManager;
        private int baseScore;
        private bool destructorButtonsAreSwapped;

        void Start() {
            _gameManager = GameManager.GetManager();
            baseScore = _gameManager.Settings.BasePoint;

            ChangeScore(0);
            ChangeComboCounter(0, ApartmentColorType.None);
            ChangeDestructorCount(0);
        }

        public void ChangeScore(int newScore) { _scoreText.SetText(newScore.ToString()); }

        public void ChangeDestructorCount(int newCount)
        {
            if (!(newCount > 0))
            {
                _baggerButtonLeft.interactable = false;
                _baggerButtonRight.interactable = false;
            }
            else
            {
                _baggerButtonLeft.interactable = true;
                _baggerButtonRight.interactable = true;
            }

            _destructCount.SetText(newCount.ToString());
        }

        public void ChangeComboCounter(int newCounter, ApartmentColorType apartmentColorType) {
            _comboCounter.SetText("Combo! X" + newCounter.ToString());

            if (newCounter <= 0) {
                _comboCounter.color = new Color(0, 0, 0, 0);
            } else {
                _comboCounter.color = _gameManager.MapManager.ApartmentColor.GetColor(apartmentColorType);
            }
        }

        public void PopScoreWithMultiplier(int multiplier) {
            GameObject popup = GameObject.Instantiate(_popupScore);

            popup.transform.SetParent(gameObject.transform, false);

            TextMeshProUGUI scoreOutput = popup.GetComponent<TextMeshProUGUI>();
            scoreOutput.SetText(multiplier.ToString() + " X " + baseScore);
        }

        private void OnDesctructibleButtonPressed(String direction) {
            _gameManager.MapManager.SpawnDesctructible(
                                                       direction.Equals(
                                                                        "vertical",
                                                                        StringComparison.CurrentCultureIgnoreCase) ?
                                                           DestructibleDirection.vertical :
                                                           DestructibleDirection.horizontal);
        }
    
        #region Button Logic for Destructors
    
        public void DestructorButtonClickLeft()
        {
            if(destructorButtonsAreSwapped)
                OnDesctructibleButtonPressed("vertical");
            else
                OnDesctructibleButtonPressed("horizontal");
        }
    
        public void DestructorButtonClickRight()
        {
            if(!destructorButtonsAreSwapped)
                OnDesctructibleButtonPressed("vertical");
            else
                OnDesctructibleButtonPressed("horizontal");
        }

        public void SwapDestructorButtonListeners()
        {
            destructorButtonsAreSwapped = !destructorButtonsAreSwapped;
        }
    
        #endregion

        public void ShowPopUpAtPosition(Vector3 position, string text) {
            Vector2 viewportPos = Camera.main.WorldToViewportPoint(position);
            var size = GetComponent<RectTransform>().sizeDelta;
            Vector2 screenPosition = new Vector2(((viewportPos.x * size.x) - size.x * 0.5f), 
                ((viewportPos.y * size.y) - size.y * 0.5f));
            GameObject popup = Instantiate(_hoverScore, transform, false);
            popup.GetComponent<HoverScore>().Init(text);

            popup.GetComponent<RectTransform>().anchoredPosition = screenPosition;
        }
    }
}