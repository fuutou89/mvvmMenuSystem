namespace mvvmMenuSystem
{
    using mvvmMenuSystem;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.IOC;
    using uFrame.Kernel;
    using uFrame.Kernel.Serialization;
    using uFrame.MVVM;
    using uFrame.MVVM.Bindings;
    using uFrame.MVVM.ViewModels;
    using UniRx;
    using UnityEngine;

    public delegate void OnLoadedDelegate(PanelViewModel root);
    public enum Bgm
    {
        /// <summary>
        /// When the scene changed, turn off BGM.
        /// </summary>
        NONE,

        /// <summary>
        /// When the scene changed, BGM will not be changed.
        /// </summary>
        SAME,

        /// <summary>
        /// When the scene changed, play a new BGM.
        /// </summary>
        PLAY,

        /// <summary>
        /// When the scene changed, turn off BGM.
        /// You will play BGM by your own code.
        /// You must to set the BgmName for SSController.
        /// </summary>
        CUSTOM
    }

    public class CallbackData
    {
        public float TimeOut { get; private set; }
        public NoParamCallback Callback { get; private set; }

        public CallbackData(float timeOut, NoParamCallback callBack)
        {
            TimeOut = timeOut;
            Callback = callBack;
        }
    }

    public partial class MenuRootViewModel : MenuRootViewModelBase
    {
        #region Delegate
        public delegate void OnScreenStartChangeDelegate(string sceneName);
        public delegate void OnSceneActivedDelegate(string sceneName);
        #endregion

        #region Event
        public OnScreenStartChangeDelegate onScreenStartChange;
        public OnSceneActivedDelegate onSceneFocus;
        #endregion

        public string m_LoadingPanelName;
        public string m_FirstScreenName;
        public string m_HomeScreenName;
        public Color m_DefaultShieldColor = new Color(1, 1, 1, 0f);
        public int m_SceneDistance = 0;
        public bool m_IsLoadAsync;
        public bool m_ClearOnLoad;
        public bool m_IsAllAdditive;
        public List<string> m_NotAdditivePanelList = new List<string>();

        public Stack<string> m_StackPopUp = new Stack<string>();						// Popup stack
        public Stack<string> m_CurrentStackScreen = new Stack<string>();				// Popup stack
        public List<GameObject> m_ListShield = new List<GameObject>();					// List Shield
        public Dictionary<string, PanelViewModel> m_DictAllPanel = new Dictionary<string, PanelViewModel>();	// Dictionary of loaded panel
        public Dictionary<string, Stack<string>> m_DictScreen = new Dictionary<string, Stack<string>>();	// Screen dict

        public int m_LoadingCount;			// Loading counter
        public int m_ShieldEmptyCount;		// Shield empty counter
        public bool m_IsBusy;				// Busy when scene is loading or scene-animation is playing
        public bool m_CanClose;				// Force able to close even busy
        public string m_GlobalBGM;			// Global BGM

        public Dictionary<string, OnLoadedDelegate> m_OnLoaded = new Dictionary<string, OnLoadedDelegate>();
    }
}
