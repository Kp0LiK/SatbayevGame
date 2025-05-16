using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client
{
    public class LevelButtonIconView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelLabel;
        [SerializeField] private Image _lockImage;

        private Button _button;

        private int _levelIndex;
        private bool _isLock;

        public bool IsLock => _isLock;

        public void UpdateLevel(Level levelData, int levelNumber)
        {
            _levelIndex = levelNumber;
            _isLock = levelData.IsLock;
            
            CheckLevel(_isLock);
        }

        private void CheckLevel(bool isLock)
        {
            _levelLabel.text = _levelIndex.ToString();
            
            _levelLabel.gameObject.SetActive(!isLock);
            _lockImage.gameObject.SetActive(isLock);
        }
    }
}