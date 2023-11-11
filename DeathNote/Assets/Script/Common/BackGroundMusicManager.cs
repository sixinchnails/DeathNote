using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 추가

public class BackGroundMusicManager : MonoBehaviour
{
    public static BackGroundMusicManager instance; // 싱글톤 인스턴스

    public AudioClip[] musicClips; // 씬에 따른 음악 클립을 여기에 할당
    private AudioSource audioSource;
    public Slider volumeSlider; // 볼륨 조절 슬라이더 참조


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        // 현재 활성화된 씬의 인덱스에 따라 음악을 재생합니다.
        PlayMusic(SceneManager.GetActiveScene().buildIndex);

        // 씬 전환 이벤트에 메소드를 구독시킵니다.
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            // PlayerPrefs에서 볼륨 설정 불러오기
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume");
            audioSource.volume = savedVolume;
            volumeSlider.value = savedVolume;
        }
        else
        {
            // 기본 볼륨 값 설정
            audioSource.volume = 1f; // 또는 원하는 기본 볼륨 값
            volumeSlider.value = 1f;
        }

        volumeSlider.onValueChanged.AddListener(SetVolume); // 슬라이더 값 변경 이벤트에 메소드 연결
    }

    // 씬이 로드될 때 호출될 메소드입니다.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusic(scene.buildIndex); // 씬 인덱스에 따라 음악을 재생합니다.

        // 배경음악 슬라이더 찾기
        GameObject backgroundMusicSliderObj = GameObject.FindGameObjectWithTag("BackgroundMusicSlider");
        if (backgroundMusicSliderObj != null)
        {
            Slider backgroundMusicSlider = backgroundMusicSliderObj.GetComponent<Slider>();
            // 이후에 배경음악 슬라이더에 관한 로직 수행
        }

        // 효과음 슬라이더 찾기
        GameObject soundEffectsSliderObj = GameObject.FindGameObjectWithTag("SoundEffectsSlider");
        if (soundEffectsSliderObj != null)
        {
            Slider soundEffectsSlider = soundEffectsSliderObj.GetComponent<Slider>();
            // 이후에 효과음 슬라이더에 관한 로직 수행
        }
    }


    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }


    public void PlayMusic(int sceneIndex)
    {
        if (audioSource != null && musicClips[sceneIndex] != null && audioSource.clip != musicClips[sceneIndex])
        {
            audioSource.clip = musicClips[sceneIndex];
            audioSource.Play();
        }
    }


    public void StopMusic()
    {
        audioSource.Stop();
    }
}