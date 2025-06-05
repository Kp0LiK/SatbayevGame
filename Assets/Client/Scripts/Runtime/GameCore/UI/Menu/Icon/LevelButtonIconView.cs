using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Client
{
    public class LevelButtonIconView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelLabel;
        [SerializeField] private Image _lockImage;
        [SerializeField] private Button _button;

        private int _levelIndex;
        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Initialize(int levelNumber, bool isOpen)
        {
            _levelIndex = levelNumber;
            UpdateVisuals(isOpen);
        }

        private void UpdateVisuals(bool isOpen)
        {
            _levelLabel.text = _levelIndex.ToString();

            _levelLabel.gameObject.SetActive(isOpen);
            _lockImage.gameObject.SetActive(!isOpen);
            _button.interactable = isOpen;
        }

        private void OnButtonClick()
        {
            GameplayManager.Instance.SetCurrentLevelIndex(_levelIndex);
            SceneManager.LoadScene("GameplayScene");
        }
    }
}