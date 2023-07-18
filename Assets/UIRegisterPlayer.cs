using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIRegisterPlayer : MonoBehaviour
{
    ProgressionManager progressionManager;

    public TMP_InputField inputPlayerName;
    public Button btnSubmit;

    public GameObject registerCont;
    public GameObject menuCont;

    public TextMeshProUGUI textRegisteredName;

    public UnityEvent OnLoginPage;
    public UnityEvent OnMenuPage;

    private void Awake()
    {
        btnSubmit.interactable = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        progressionManager = ProgressionManager.instance;

        if(progressionManager.playerName != "")
        {
            registerCont.gameObject.SetActive(false);
            menuCont.gameObject.SetActive(true);

            SetRegisteredName();
            OnMenuPage?.Invoke();
        }
        else
        {
            registerCont.gameObject.SetActive(true);
            menuCont.gameObject.SetActive(false);

            inputPlayerName.onValueChanged.AddListener(OnChangeInputName);
            OnLoginPage?.Invoke();
        }
    }

    void OnChangeInputName(string input)
    {
        if(input != "")
        {
            btnSubmit.interactable = true;
        }
        else
        {
            btnSubmit.interactable = false;
        }
    }

    public void SubmitPlayerName()
    {
        progressionManager.SetPlayerName(inputPlayerName.text);

        registerCont.gameObject.SetActive(false);
        menuCont.gameObject.SetActive(true);

        SetRegisteredName();
        OnMenuPage?.Invoke();
    }

    void SetRegisteredName()
    {
        textRegisteredName.text = $"Halo, {progressionManager.playerName}";
    }
}