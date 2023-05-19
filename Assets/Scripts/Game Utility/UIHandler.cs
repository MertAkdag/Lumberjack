using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Slider = UnityEngine.UI.Slider;
using GoogleMobileAds.Api;
using GoogleMobileAds;



namespace Game_Utility
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] private Canvas gameOverCanvas;
        [SerializeField] private Slider progressSlider;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button quitButton;
        [SerializeField] private Image fillImage;
        [SerializeField] private Image leftChopImage;
        [SerializeField] private Image rightChopImage;
        [SerializeField] private Image musicButton;
        [SerializeField] private Image soundButton;
        [SerializeField] private Sprite musicOnImage;
        [SerializeField] private Sprite musicOffImage;
        [SerializeField] private Sprite soundOnImage;
        [SerializeField] private Sprite soundOffImage;

        private ScoreHandler _scoreHandler;
        private DifficultyHandler _difficultyHandler;
        private AudioHandler _audioHandler;

        private const float FadeDuration = 2f;
        private const float MoveDuration = 1f;
        private const float MoveDistance = 0.9f;
    
        private Vector3 _levelTextOriginalPos;
        private Vector3 _levelTextShowUpPos;

        private bool _isGameStarted;
        
        [Header("Reklam")]
        public InterstitialAd interstitial;
        public string adUnitId = "ca-app-pub-8095700878802270/9578949847";   

        
        private void Start()
        {
            
            GameEvents.OnStartGame += StartGame;
            GameEvents.OnCutTheLog += UpdateScoreText;
            GameEvents.OnIncreaseDifficulty += UpdateLevelText;
            GameEvents.OnDecreaseDifficulty += UpdateLevelText;
            GameEvents.OnGameOver += GameOver;
        
            _scoreHandler = gameObject.GetComponent<ScoreHandler>();
            _difficultyHandler = gameObject.GetComponent<DifficultyHandler>();
            _audioHandler = gameObject.GetComponent<AudioHandler>();

            _levelTextOriginalPos = levelText.transform.position;
            _levelTextShowUpPos = new Vector3(_levelTextOriginalPos.x, _levelTextOriginalPos.y - MoveDistance, _levelTextOriginalPos.z);

            scoreText.text = _scoreHandler.CurrentScore.ToString();
        
            PlayChopSignAnimation();
        }

        private void OnDestroy()
        {
            GameEvents.OnStartGame -= StartGame;
            GameEvents.OnCutTheLog -= UpdateScoreText;
            GameEvents.OnIncreaseDifficulty -= UpdateLevelText;
            GameEvents.OnDecreaseDifficulty -= UpdateLevelText;
            GameEvents.OnGameOver -= GameOver;
        }

        private void Update()
        {
            progressSlider.value = _difficultyHandler.ProgressBar;
        }

        private void StartGame()
        {
            _isGameStarted = true;
            UpdateLevelText();

            uiCanvas.enabled = true;
            leftChopImage.gameObject.SetActive(false);
            rightChopImage.gameObject.SetActive(false);
            musicButton.gameObject.SetActive(false);
            soundButton.gameObject.SetActive(false);
            quitButton.gameObject.SetActive(false);
        }
    
        private void PlayChopSignAnimation()
        {
            leftChopImage.rectTransform.DOMoveX(leftChopImage.rectTransform.position.x - 1, 0.35f).SetLoops(-1, LoopType.Yoyo);
            rightChopImage.rectTransform.DOMoveX(rightChopImage.rectTransform.position.x + 1, 0.35f).SetLoops(-1, LoopType.Yoyo);
        }

        private void UpdateScoreText()
        {
            var score = _scoreHandler.CurrentScore.ToString();
            scoreText.text = score;
        }

        private void UpdateLevelText()
        {
            if (!_isGameStarted) return;
        
            var level = _difficultyHandler.Level.ToString();
            levelText.text = "level " + level;
        
            StartCoroutine(PlayLevelUpAnimation());
        }
    

        private IEnumerator PlayLevelUpAnimation()
        {
            fillImage.DOColor(Color.white, 0.5f).SetEase(Ease.InFlash, 10, 0);
        
            levelText.rectTransform.DOMove(_levelTextShowUpPos, MoveDuration);
            levelText.DOFade(1, FadeDuration);
            yield return new WaitForSeconds(FadeDuration);

            levelText.DOFade(0, FadeDuration);
            yield return new WaitForSeconds(FadeDuration / 2);
        
            levelText.rectTransform.DOMove(_levelTextOriginalPos, MoveDuration);
        }
    
        private void GameOver()
        {
            var highScore = ScoreHandler.GetHighScore().ToString();
            highScoreText.text = "high score: " + highScore;
            gameOverCanvas.enabled = true;
            uiCanvas.enabled = false;
            if (Random.Range(0,2) == 0)
            {
                print("reklam gösteriliyor mq");
                RequestInterstatial();

                if (interstitial.IsLoaded())
                {
                    interstitial.Show();
                }
                Time.timeScale = 0;

            }
        }

        private void RequestInterstatial()
        {
                interstitial = new InterstitialAd(adUnitId);
                AdRequest request = new AdRequest.Builder().Build();
                interstitial.LoadAd(request);
        }

        public void OnMusicButtonClick()
        {
            if (_audioHandler.IsMusicOn())
            {
                _audioHandler.SetMusicActive(false);
                musicButton.sprite = musicOffImage;
            }
            else
            {
                _audioHandler.SetMusicActive(true);
                musicButton.sprite = musicOnImage;
            }
        }

        public void OnSoundButtonClick()
        {
            if (_audioHandler.IsSoundOn())
            {
                _audioHandler.SetSoundActive(false);
                soundButton.sprite = soundOffImage;
            }
            else
            {
                _audioHandler.SetSoundActive(true);
                soundButton.sprite = soundOnImage;
            }
        }
    
        public void OnRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }

        public void OnQuit()
        {
                Application.Quit();
        }
        
        
    }
}
