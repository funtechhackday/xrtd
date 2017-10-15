using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{

    public delegate void StageReadyAction();
    public static event StageReadyAction onStageReady;

    public delegate void TryAgainAction();
    public static event TryAgainAction onTryAgainClicked;

    public delegate void PlayButtonAction();
    public static event PlayButtonAction onPlayButtonClick;

    private int _stageCount;

    GameObject StageInfoPanel;
    GameObject MainMenuPanel;

    public static MainMenuHandler inst;
    private void Awake()
    {
        if (inst == null)
            inst = this;
    }

    // Use this for initialization
    void Start()
    {
        _stageCount = 0;
        StageInfoPanel = this.gameObject.transform.GetChild(0).gameObject;
        MainMenuPanel = this.gameObject.transform.GetChild(1).gameObject;

        StageInfoPanel.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => { tryAgain(); });
        StageInfoPanel.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => { Application.Quit(); });

        MainMenuPanel.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => { if (onPlayButtonClick != null) { onPlayButtonClick(); } });

        MainMenuHandler.onPlayButtonClick += MenuUIHandler_onPlayButtonClick;

    }

    private void MenuUIHandler_onPlayButtonClick()
    {
        Debug.Log("START");
        StartCoroutine(ChangeStageCoroutine(5));

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator ShowStageInfoCoroutine(float delay)
    {
        MainMenuPanel.SetActive(false);
        QuestionButtonsHandle(true);
        ++_stageCount;
        StageInfoPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = string.Format("Stage {0}", _stageCount);
        StageInfoPanel.SetActive(true);
        yield return new WaitForSeconds(delay);
        StageInfoPanel.SetActive(false);
    }

    public void ShowStageInfo()
    {
        MainMenuPanel.SetActive(false);
        QuestionButtonsHandle(true);
        ++_stageCount;
        StageInfoPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = string.Format("Stage {0}", _stageCount);
        StageInfoPanel.SetActive(true);
    }

    public void HideStageInfo()
    {
        StageInfoPanel.SetActive(false);
    }

    public void ShowGameOverInfo(string info)
    {
        MainMenuPanel.SetActive(false);
        StageInfoPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = string.Format("{0}", info);
        StageInfoPanel.SetActive(true);
        QuestionButtonsHandle(true);

    }

    public void ResetStage()
    {
        _stageCount = 0;
    }

    public IEnumerator ChangeStageCoroutine(int delay)
    {
        StageInfoPanel.SetActive(true);
        QuestionButtonsHandle(true);
        ShowStageInfo();
        yield return new WaitForSeconds(2.0f);
        while (delay > 0)
        {
            StageInfoPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = string.Format("Ready in {0}", delay);
            delay--;
            Debug.Log(delay);
            yield return new WaitForSeconds(1.0f);
        }
        if (onStageReady != null)
        {
            onStageReady();
        }
        StageInfoPanel.SetActive(false);
    }

    public void ShowMainMenuOnly()
    {
        foreach (Transform menuPanel in this.gameObject.transform)
        {
            menuPanel.gameObject.SetActive(false);
        }
        MainMenuPanel.SetActive(true);
    }

    private void tryAgain()
    {
        QuestionButtonsHandle(true);
        StageInfoPanel.SetActive(false);
        ResetStage();
        if (onTryAgainClicked != null)
        {
            onTryAgainClicked();
        }
    }

    private void QuestionButtonsHandle(bool needHide)
    {
        if (needHide)
        {
            StageInfoPanel.transform.GetChild(1).gameObject.SetActive(false);
            StageInfoPanel.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            StageInfoPanel.transform.GetChild(1).gameObject.SetActive(true);
            StageInfoPanel.transform.GetChild(2).gameObject.SetActive(true);
        }
    }




}