using Metaverse;
using UnityEngine;

public class DesktopControl : MonoBehaviour, IDesktopControls
{
    public static DesktopControl instance;

    private CanvasButtonsHandler CanvasButtonsHandler;
    // private ReactScreen ReactScreen;
    private XanaChatSystem XanaChatSystem;
    private AvatarManager AvatarManager;
    //   private XanaVoiceChat XanaVoiceChat;
    private PlayerControllerNew PlayerControllerNew;
    public GameObject freelookCamera;


    public KeyCode Exit;
    //  public KeyCode Dance;
    //  public KeyCode Emote;
    public KeyCode Cam_Selfie;
    public KeyCode Chat;
    public KeyCode Chat_Window;
    public KeyCode Invite;
    public KeyCode Settings;
    public KeyCode Help;
    //  public KeyCode ThirdPersonCam;

    public bool ExitPressed;
    public GameObject ExitPanel, settingPanel;
    public GameObject movementClose;
    bool idleOnce;
    public bool sceneFocus;
    private void Start() => Init();


    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            //  DontDestroyOnLoad(this);

        }
        //else
        //{
        //    DestroyImmediate(this);
        //}

        idleOnce = false;
    }

    private void Update()
    {
        Controls();
        ExitToggle();
        //Debug.Log("sceneFocus = "+ sceneFocus);
        //if (!sceneFocus)
        //{
        //    ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<Animator>().enabled = false;
        //    if (ReferrencesForDynamicMuseum.instance.playerControllerNew.GetComponent<PlayerControllerNew>() != null)
        //    {
        //        ReferrencesForDynamicMuseum.instance.playerControllerNew.GetComponent<PlayerControllerNew>().sprintSpeed = 0f;
        //        // ReferrencesForDynamicMuseum.instance.playerControllerNew.GetComponent<PlayerControllerNew>().enabled = false;
        //    }

        //    Debug.Log("OnApplicationFocus = " + sceneFocus);
        //}
        //else if (sceneFocus)
        //{
        //    ReferrencesForDynamicMuseum.instance.m_34player.GetComponent<Animator>().enabled = true;
        //    if (ReferrencesForDynamicMuseum.instance.playerControllerNew.GetComponent<PlayerControllerNew>() != null)
        //    {
        //        //ReferrencesForDynamicMuseum.instance.playerControllerNew.GetComponent<PlayerControllerNew>().enabled = true;
        //        ReferrencesForDynamicMuseum.instance.playerControllerNew.GetComponent<PlayerControllerNew>().sprintSpeed = 4f;
        //    }

        //}
    }

    private void ExitToggle()
    {
        if (ExitPanel.activeSelf)
        {
            ExitPressed = true;
        }
        else
        {
            ExitPressed = false;
        }
    }

    public void EnablePlayerController()
    {
        movementClose.GetComponent<PlayerControllerNew>().animator.Play("Dwarf Idle");
    }

    public void Controls()
    {
        if (XanaChatSystem.InputFieldChat.isFocused)
        {
            movementClose.GetComponent<PlayerControllerNew>().sprintSpeed = 0f;
            if (movementClose.GetComponent<PlayerControllerNew>().animator.GetCurrentAnimatorStateInfo(0).IsName("NormalStatus"))
            {
                //movementClose.GetComponent<PlayerControllerNew>().animator.Play("Dwarf Idle");
            }
            if (movementClose.GetComponent<PlayerControllerNew>().animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")
                || movementClose.GetComponent<PlayerControllerNew>().animator.GetCurrentAnimatorStateInfo(0).IsName("Falling")
                || movementClose.GetComponent<PlayerControllerNew>().animator.GetCurrentAnimatorStateInfo(0).IsName("LandSoft"))
            {
                // movementClose.GetComponent<PlayerControllerNew>().enabled = false;
            }
            else
            {
                movementClose.GetComponent<PlayerControllerNew>().animator.Play("Dwarf Idle");

            }
            if (movementClose.GetComponent<PlayerControllerNew>().animator.GetCurrentAnimatorStateInfo(0).IsName("Dwarf Idle"))
            {
                movementClose.GetComponent<PlayerControllerNew>().enabled = false;

            }






            PlayerControllerNew.m_IsMovementActive = false;
            // movementClose.GetComponent<PlayerControllerNew>().enabled = false;

            // Debug.Log("IsFocused = ");
        }
        else
        {
            if (ExitPanel.activeSelf || settingPanel.activeSelf)
            {
                movementClose.GetComponent<PlayerControllerNew>().sprintSpeed = 0f;
                movementClose.GetComponent<PlayerControllerNew>().enabled = false;
                Debug.Log(" settingPanel.activeSelf = " + settingPanel.activeSelf);
            }
            else
            {
                movementClose.GetComponent<PlayerControllerNew>().sprintSpeed = 4f;
                movementClose.GetComponent<PlayerControllerNew>().enabled = true;
                freelookCamera.GetComponent<CameraLook>().enabled = true;
            }

            PlayerControllerNew.m_IsMovementActive = true;
            //Debug.Log("!IsFocused = "+ movementClose.GetComponent<PlayerControllerNew>());
        }
        if (ExitPressed ? PlayerControllerNew.m_IsMovementActive = false : PlayerControllerNew.m_IsMovementActive = true)
            if (SelfieController.Instance.m_IsSelfieFeatureActive ? PlayerControllerNew.m_IsMovementActive = false : PlayerControllerNew.m_IsMovementActive = true)

                if (Input.GetKeyDown(Exit))
                {
                    CanvasButtonsHandler.OnExitButtonClick();
                    //ExitToggle();
                    //Debug.Log("Exit was pressed.");

                }
                else if (Input.GetKeyDown(Settings) && !XanaChatSystem.InputFieldChat.isFocused)
                {
                    CanvasButtonsHandler.OnSettingButtonClick();
                    //Debug.Log("setting was pressed.");
                }
                //else if (Input.GetKeyDown(ThirdPersonCam))
                //{
                //    CanvasButtonsHandler.OnSwitchCameraClick();
                //    //Debug.Log("switch camera was pressed.");
                //}
                else if (Input.GetKeyDown(Cam_Selfie) && PlayerControllerNew.horizontal == 0 && PlayerControllerNew.vertical == 0 && !PlayerControllerNew.IsJumping && !XanaChatSystem.InputFieldChat.isFocused && !settingPanel.activeSelf && !ExitPanel.activeSelf)
                {
                    CanvasButtonsHandler.OnSelfiBtnClick();
                    //Debug.Log("selfie camera was pressed.");
                }
                else if (Input.GetKeyDown(Help) && !XanaChatSystem.InputFieldChat.isFocused && !settingPanel.activeSelf && !ExitPanel.activeSelf)
                {
                    CanvasButtonsHandler.OnHelpButtonClick(true);
                    //Debug.Log("help was pressed.");
                }
                //else if (Input.GetKeyDown(Dance))
                //{
                //    CanvasButtonsHandler.OnOpenAnimationPanel();
                //    //Debug.Log("OnOpenAnimationPanel was pressed.");
                //}
                else if (Input.GetKeyDown(Invite) && !XanaChatSystem.InputFieldChat.isFocused && !settingPanel.activeSelf && !ExitPanel.activeSelf)
                {
                    CanvasButtonsHandler.OnInviteClick();
                    //Debug.Log("OnInviteClick was pressed.");
                }
                //else if (Input.GetKeyDown(Emote))
                //{
                //    ReactScreen.OpenPanel();
                //    //Debug.Log("OpenPanel was pressed.");
                //}
                else if (Input.GetKeyDown(Chat_Window) && !XanaChatSystem.InputFieldChat.isFocused && !settingPanel.activeSelf && !ExitPanel.activeSelf)
                {
                    XanaChatSystem.OpenCloseChatDialog();
                    //Debug.Log("chat was pressed.");
                }
                else if (Input.GetKeyDown(Chat))
                {
                    XanaChatSystem.InputFieldChat.ActivateInputField();
                    //if (movementClose.GetComponent<PlayerControllerNew>().animator.GetCurrentAnimatorStateInfo(0).IsName("Dwarf Idle"))
                    //{
                    //    movementClose.GetComponent<PlayerControllerNew>().enabled = false;
                    //}
                    /*else*/
                    if (!idleOnce)
                    {
                        //movementClose.GetComponent<PlayerControllerNew>().animator.Play("Dwarf Idle");
                        //movementClose.GetComponent<PlayerControllerNew>().enabled = false;
                        //idleOnce = true;
                    }

                    //Debug.Log("chat was pressed.");
                }
        //else if (Input.GetKeyDown(KeyCode.L))
        //{
        //    AvatarManager.OffSelfie();
        //    //Debug.Log("OffSelfie was pressed.");
        //}
        //else if (Input.GetKeyDown(KeyCode.M))
        //{
        //    XanaVoiceChat.TurnOffMic();
        //    //Debug.Log("TurnOffMic was pressed.");
        //}     

    }
    UnityEngine.EventSystems.PointerEventData eventData;
    private void Init()
    {
        CanvasButtonsHandler ??= CanvasButtonsHandler.inst;
        //   ReactScreen ??= ReactScreen.Instance;
        XanaChatSystem ??= XanaChatSystem.instance;
        AvatarManager ??= AvatarManager.Instance;
        //   XanaVoiceChat ??= XanaVoiceChat.instance;
        PlayerControllerNew ??= PlayerControllerNew.Instacne;
    }

    private void OnApplicationFocus(bool focus)
    {
       
    }
}
