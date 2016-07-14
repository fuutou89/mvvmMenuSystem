namespace mvvmMenuSystem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Kernel;
    using uFrame.Kernel.Serialization;
    using uFrame.MVVM;
    using uFrame.MVVM.Bindings;
    using uFrame.MVVM.Services;
    using uFrame.MVVM.ViewModels;
    using UniRx;
    using UnityEngine;


    public class MenuRootView : MenuRootViewBase
    {

        public UIType uiType = UIType.nGUI;
        private GameObject m_Scenes;			// Scene container object
        private GameObject m_Shields;			// Shield container object

        protected override void InitializeViewModel(uFrame.MVVM.ViewModels.ViewModel model)
        {
            base.InitializeViewModel(model);
            // NOTE: this method is only invoked if the 'Initialize ViewModel' is checked in the inspector.
            // var vm = model as MenuRootViewModel;
            // This method is invoked when applying the data from the inspector to the viewmodel.  Add any view-specific customizations here.
        }

        public override void Bind()
        {
            base.Bind();
            // Use this.MenuRoot to access the viewmodel.
            // Use this method to subscribe to the view-model.
            // Any designer bindings are created in the base implementation.

            m_Scenes = new GameObject("Scenes");
            m_Shields = new GameObject("Shields");

            m_Scenes.transform.SetParent(this.transform);//.parent = this.transform;
            m_Scenes.transform.localScale = Vector3.one;
            m_Scenes.transform.localPosition = Vector3.zero;

            m_Shields.transform.SetParent(this.transform);//parent = this.transform;
            m_Shields.transform.localScale = Vector3.one;
            m_Shields.transform.localPosition = Vector3.zero;

            this.Publish(new CreateLoadingCommand());
            this.Publish(new OpenMainMenuCommand() { panelName = "MainMenu" });
        }

        public override void shieldTopChanged(ShieldViewModel arg1)
        {
            base.shieldTopChanged(arg1);
            if (arg1 != null)
            {
                CreateShield((int)arg1.depthIndex, arg1);
            }
        }

        public override void shieldEmptyChanged(ShieldViewModel arg1)
        {
            base.shieldEmptyChanged(arg1);
            if (arg1 != null)
            {
                CreateShield((int)arg1.depthIndex, arg1);
            }
        }

        public override void ShieldsOnAdd(ShieldViewModel arg1)
        {
            base.ShieldsOnAdd(arg1);
            CreateShield((int)arg1.depthIndex, arg1);
        }

        private void CreateShield(int i, ShieldViewModel sVM)
        {
            // Instantiate from resources
            GameObject sh = null;

            switch (uiType)
            {
                case UIType.nGUI:
                    sh = Instantiate(Resources.Load("nGUIShield")) as GameObject;
                    break;
                case UIType.uGUI:
                    sh = Instantiate(Resources.Load("uGUIShield")) as GameObject;
                    break;
            }

            sh.name = "Shield" + i;
            sh.transform.SetParent(m_Shields.transform, true);
            sh.transform.localScale = Vector3.one;

            ShieldView sView = sh.GetComponent<ShieldView>();
            sView.ViewModelObject = sVM;
        }

        public override void loadingTopChanged(PanelViewModel arg1)
        {
            base.loadingTopChanged(arg1);
            if (arg1 != null)
            {
                StartCoroutine(SyncLoadLoadingTop(arg1));                
            }
        }

        private IEnumerator SyncLoadLoadingTop(PanelViewModel obj)
        {
            yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(MenuRoot.m_LoadingPanelName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            //GameObject[] roots = UnityEngine.SceneManagement.SceneManager.GetSceneByName(MenuRoot.m_LoadingSceneName).GetRootGameObjects();
            GameObject menuRoot = GameObject.Find(MenuRoot.m_LoadingPanelName);//roots[0];
            menuRoot.transform.SetParent(this.transform);
            menuRoot.transform.localScale = Vector3.one;
            menuRoot.name = menuRoot.name + "Top";

            PanelView menuView = menuRoot.GetComponent<PanelView>();
            menuView.ViewModelObject = obj;

            UnityEngine.SceneManagement.SceneManager.UnloadScene(MenuRoot.m_LoadingPanelName);
        }

        public override void LoadPanelExecuted(LoadPanelCommand command)
        {
            base.LoadPanelExecuted(command);

            if (MenuRoot.m_DictAllPanel.ContainsKey(command.panelName))
            {
                Debug.LogWarning("Loaded this scene before. Please check again.");
                return;
            }

            StartCoroutine(SyncLoadMenu(command));
        }

        private IEnumerator SyncLoadMenu(LoadPanelCommand command)
        {
            yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(command.panelName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            //GameObject[] roots = UnityEngine.SceneManagement.SceneManager.GetSceneByName(command.menuName).GetRootGameObjects();

            GameObject menuHolder = GameObject.Find(command.panelName);//roots[0];
            menuHolder.transform.SetParent(this.m_Scenes.transform);
            menuHolder.transform.localScale = Vector3.one;
            PanelView menuView = menuHolder.GetComponent<PanelView>();
            menuView.BringAnimationToVeryFar();

            UnityEngine.SceneManagement.SceneManager.UnloadScene(command.panelName);
        }
    }
}
