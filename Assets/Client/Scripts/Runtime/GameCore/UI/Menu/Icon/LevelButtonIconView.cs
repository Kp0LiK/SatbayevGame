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
        private bool _isLock;
        private TaskType _taskType;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Initialize(TaskType taskType, Level levelData, int levelNumber)
        {
            _taskType = taskType;
            _levelIndex = levelNumber;
            _isLock = levelData.IsLock;

            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            _levelLabel.text = _levelIndex.ToString();

            _levelLabel.gameObject.SetActive(!_isLock);
            _lockImage.gameObject.SetActive(_isLock);
            _button.interactable = !_isLock;
        }

        private void OnButtonClick()
        {
            if (_isLock) return;

            // Save current task type and level index
            PlayerPrefs.SetInt("CurrentTaskType", (int)_taskType);
            PlayerPrefs.SetInt("CurrentLevelIndex", _levelIndex);
            PlayerPrefs.Save();

            // Load gameplay scene
            SceneManager.LoadScene("GameplayScene");
        }
    }
}