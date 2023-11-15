// Unity 엔진과 UI, 씬 관리 관련 기능을 사용하기 위한 네임스페이스.
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackGroundMusicManager : MonoBehaviour
{
    // 이 클래스의 유일한 인스턴스를 저장하는 정적 변수.
    public static BackGroundMusicManager instance;

    // 씬별로 재생할 배경음악 클립을 저장하는 배열.
    public AudioClip[] musicClips;
    // 오디오를 재생하는 컴포넌트에 대한 참조.
    private AudioSource audioSource;
    // 사용자가 조정할 수 있는 볼륨 슬라이더 UI에 대한 참조.
    public Slider volumeSlider;
    // 현재 음소거 상태인지를 저장하는 변수.
    private bool isMuted;
    // 음소거 되기 이전의 볼륨 값을 저장하는 변수.
    private float previousVolume;
    // 볼륨 버튼의 이미지 컴포넌트에 대한 참조.
    public Image volumeButtonImage;
    // 볼륨이 켜져 있을 때 표시될 이미지.
    public Sprite volumeSprite;
    // 음소거 상태일 때 표시될 이미지.
    public Sprite muteSprite;

    void Awake()
    {
        // 인스턴스가 아직 할당되지 않았다면 이 오브젝트를 싱글톤으로 설정합니다.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환시에도 오브젝트가 파괴되지 않도록 합니다.
            audioSource = GetComponent<AudioSource>(); // 오디오 소스 컴포넌트를 가져옵니다.
        }
        else if (instance != this) // 이미 인스턴스가 존재한다면 중복을 방지하기 위해 자기 자신을 파괴합니다.
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 게임 시작 시 첫 씬의 음악을 재생합니다.
        PlayMusic(SceneManager.GetActiveScene().buildIndex);

        // 씬 전환 이벤트에 대한 구독을 설정합니다.
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 저장된 볼륨 값이 있다면 그 값을 사용해 오디오 볼륨을 설정합니다.
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume");
            audioSource.volume = savedVolume;
            volumeSlider.value = savedVolume; // 슬라이더의 UI도 업데이트합니다.
        }
    }

    // 새 씬이 로드될 때 호출되는 메소드입니다.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateVolumeSlider(); // 볼륨 슬라이더를 업데이트합니다.
        PlayMusic(scene.buildIndex); // 새 씬에 맞는 음악을 재생합니다.

        // 음소거 버튼을 찾아서 이미지와 리스너를 업데이트합니다.
        GameObject muteButtonObj = GameObject.FindGameObjectWithTag("MuteButton");
        if (muteButtonObj != null)
        {
            volumeButtonImage = muteButtonObj.GetComponent<Image>();
            UpdateMuteButtonImage(); // 음소거 버튼 이미지를 업데이트합니다.

            // Button 컴포넌트를 가져와서 음소거 토글 메소드를 리스너로 설정합니다.
            Button muteButton = muteButtonObj.GetComponent<Button>();
            if (muteButton != null)
            {
                muteButton.onClick.RemoveAllListeners(); // 기존 리스너를 모두 제거합니다.
                muteButton.onClick.AddListener(ToggleMute); // 음소거 토글 메소드를 리스너로 추가합니다.
            }
        }
        else
        {
            Debug.LogError("Mute button with 'MuteButton' tag not found in the scene.");
        }
    }

    void OnDestroy()
    {
        // 더 이상 필요하지 않으므로 이벤트 구독을 취소합니다.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void PlayMusic(int sceneIndex)
    {
        Debug.Log("노래 시작해주는 함수");
        if (musicClips[sceneIndex] != null && audioSource.clip != musicClips[sceneIndex])
        {
            audioSource.clip = musicClips[sceneIndex];
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        Debug.Log("볼륨 조절해주는 함수");

        if (audioSource != null)
        {
            audioSource.volume = volume;
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();

            // 볼륨이 0이면 음소거로 간주하고 이미지를 음소거 이미지로 변경합니다.
            if (volume == 0)
            {
                if (!isMuted)
                {
                    // 볼륨이 0이 되어 음소거 상태가 되었으므로 이전 볼륨을 저장합니다.
                    previousVolume = volume;
                    isMuted = true;
                }
                volumeButtonImage.sprite = muteSprite; // 음소거 이미지로 변경
            }
            else
            {
                // 볼륨이 0보다 크면 음소거 상태를 해제하고 이미지를 볼륨 이미지로 변경합니다.
                if (isMuted)
                {
                    // 이전에 음소거 상태였다면 음소거를 해제합니다.
                    isMuted = false;
                }
                volumeButtonImage.sprite = volumeSprite; // 볼륨 이미지로 변경
            }
        }
        else
        {
            Debug.LogError("audioSource is null");
        }
    }

    private void UpdateVolumeSlider()
    {
        volumeSlider = GameObject.FindGameObjectWithTag("BackgroundMusicSlider")?.GetComponent<Slider>();
        if (volumeSlider != null)
        {
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                float savedVolume = PlayerPrefs.GetFloat("MusicVolume");
                volumeSlider.value = savedVolume;
            }
            volumeSlider.onValueChanged.RemoveAllListeners(); // 기존 리스너 제거
            volumeSlider.onValueChanged.AddListener(SetMusicVolume); // 새 리스너 추가
        }
        else
        {
            Debug.LogError("Volume slider not found");
        }
    }

    // 음소거 토글 메소드
    public void ToggleMute()
    {
        if (isMuted)
        {
            // 음소거 해제
            audioSource.volume = previousVolume;
            isMuted = false;
            volumeButtonImage.sprite = volumeSprite; // 볼륨 이미지로 변경
        }
        else
        {
            // 음소거 설정
            previousVolume = audioSource.volume;
            audioSource.volume = 0;
            isMuted = true;
            volumeButtonImage.sprite = muteSprite; // 음소거 이미지로 변경
        }
    }

    private void UpdateMuteButtonImage()
    {
        if (volumeButtonImage != null)
        {
            volumeButtonImage.sprite = isMuted ? muteSprite : volumeSprite;
        }
    }
}