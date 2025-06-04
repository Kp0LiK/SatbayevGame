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
        private TaskType _taskType;
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void Initialize(TaskType taskType, int levelNumber, bool isOpen)
        {
            _taskType = taskType;
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
            LevelLoadParams.TaskType = _taskType;
            LevelLoadParams.LevelIndex = _levelIndex;
            SceneManager.LoadScene("GameplayScene");
        }
    }
}