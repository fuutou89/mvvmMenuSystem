namespace mvvmMenuSystem
{
    using mvvmMenuSystem;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.IOC;
    using uFrame.Kernel;
    using uFrame.MVVM;
    using UniRx;
    using UnityEngine;

    #region Delgate
    public delegate void PanelCallBackDelegate(PanelViewModel viewModel);
    public delegate void NoParamCallback();
    #endregion

    public class MenuService : MenuServiceBase
    {

        [Inject("MenuRoot")]
        public MenuRootViewModel MenuRoot;

        [Inject]
        public PanelController panelController;
        [Inject]
        public ShieldController shieldController;

        /// <summary>
        /// This method is invoked whenever the kernel is loading
        /// Since the kernel lives throughout the entire lifecycle  of the game, this will only be invoked once.
        /// </summary>
        public override void Setup()
        {
            base.Setup();
            // Use the line below to subscribe to events
            // this.OnEvent<MyEvent>().Subscribe(myEventInstance => { TODO });
        }

        #region Event Handler

        /// <sumarry>
        // This method is executed when using this.Publish(new PanelLoadedEvent())
        /// </sumarry>
        public override void PanelLoadedEventHandler(PanelLoadedEvent data)
        {
            base.PanelLoadedEventHandler(data);

            if (!MenuRoot.m_OnLoaded.ContainsKey(data.panelName)) return;

            data.panelViewModel.panelName = data.panelName;
            MenuRoot.m_OnLoaded[data.panelName](data.panelViewModel);
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new PanelUnloadEvent())
        /// </sumarry>
        public override void PanelUnloadEventHandler(PanelUnloadEvent data)
        {
            base.PanelUnloadEventHandler(data);

            if (MenuRoot.m_OnLoaded.ContainsKey(data.panelName))
            {
                MenuRoot.m_OnLoaded.Remove(data.panelName);
            }
        }
        #endregion

        #region Command Handler

        /// <sumarry>
        // This method is executed when using this.Publish(new CreateLoadingCommand())
        /// </sumarry>
        public override void CreateLoadingCommandHandler(CreateLoadingCommand data)
        {
            base.CreateLoadingCommandHandler(data);

            if (string.IsNullOrEmpty(MenuRoot.m_LoadingPanelName))
            {
                LoadFirstScreen();
                return;
            }

            if (MenuRoot.loadingTop == null)
            {
                PanelViewModel loadingPanel = panelController.CreatePanel();
                loadingPanel.posIndex = MenuConst.SHIELD_TOP_INDEX;
                loadingPanel.depthIndex = MenuConst.SHIELD_TOP_INDEX;
                MenuRoot.loadingTop = loadingPanel;
                LoadFirstScreen();
            }
        }

        private void LoadFirstScreen()
        {
            if (!string.IsNullOrEmpty(MenuRoot.m_FirstScreenName))
            {
                StartCoroutine(IEScreen(MenuRoot.m_FirstScreenName));
            }
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new OpenScreenCommand())
        /// </sumarry>
        public override void OpenScreenCommandHandler(OpenScreenCommand cmd)
        {
            base.OpenScreenCommandHandler(cmd);
            StartCoroutine(IEScreen(cmd.panelName, cmd.data, cmd.onActive, cmd.onDeactive));
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new OpenSubScreenCommand())
        /// </sumarry>
        public override void OpenSubScreenCommandHandler(OpenSubScreenCommand cmd)
        {
            base.OpenSubScreenCommandHandler(cmd);
            StartCoroutine(IESubScreen(cmd.panelName, cmd.data, cmd.onActive, cmd.onDeactive));
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new OpenPopupCommand())
        /// </sumarry>
        public override void OpenPopupCommandHandler(OpenPopupCommand cmd)
        {
            base.OpenPopupCommandHandler(cmd);
            StartCoroutine(IEPopUp(cmd.panelName, cmd.data, cmd.onActive, cmd.onDeactive));
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new OpenMainMenuCommand())
        /// </sumarry>
        public override void OpenMainMenuCommandHandler(OpenMainMenuCommand cmd)
        {
            base.OpenMainMenuCommandHandler(cmd);
            StartCoroutine(IEMainMenu(cmd.panelName, cmd.data, cmd.onActive, cmd.onDeactive));
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new ClosePanelCommand())
        /// </sumarry>
        public override void ClosePanelCommandHandler(ClosePanelCommand cmd)
        {
            base.ClosePanelCommandHandler(cmd);
            StartCoroutine(IEClose(cmd.immediate, cmd.callback));
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new ResetPanelCommand())
        /// </sumarry>
        public override void ResetPanelCommandHandler(ResetPanelCommand cmd)
        {
            base.ResetPanelCommandHandler(cmd);
            string menuName = DestroyCurrentStack();
            StartCoroutine(IEScreen(menuName, cmd.data, cmd.onActive, cmd.onDeactive));
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new ShowMainMenuCommand())
        /// </sumarry>
        public override void ShowMainMenuCommandHandler(ShowMainMenuCommand data)
        {
            base.ShowMainMenuCommandHandler(data);
            if (MenuRoot.mainMenu != null)
            {
                MenuRoot.mainMenu.isActive = true;
            }
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new HideMainMenuCommand())
        /// </sumarry>
        public override void HideMainMenuCommandHandler(HideMainMenuCommand data)
        {
            base.HideMainMenuCommandHandler(data);
            if (MenuRoot.mainMenu != null)
            {
                MenuRoot.mainMenu.isActive = false;
            }
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new GoHomeScreenCommand())
        /// </sumarry>
        public override void GoHomeScreenCommandHandler(GoHomeScreenCommand data)
        {
            base.GoHomeScreenCommandHandler(data);
            if (!string.IsNullOrEmpty(MenuRoot.m_HomeScreenName))
            {
                if (MenuRoot.m_CurrentStackScreen.Peek() == MenuRoot.m_HomeScreenName)
                {
                    Quit();
                }
                else
                {
                    OpenScreen(MenuRoot.m_HomeScreenName);
                }
            }
            else
            {
                Quit();
            }
        }


        /// <sumarry>
        // This method is executed when using this.Publish(new DestoryInactivePanelsCommand())
        /// </sumarry>
        public override void DestoryInactivePanelsCommandHandler(DestoryInactivePanelsCommand cmd)
        {
            base.DestoryInactivePanelsCommandHandler(cmd);
            // Screen stacks
            foreach (var screens in MenuRoot.m_DictScreen)
            {
                string sn = screens.Key;
                if (IsNotInExcept(sn, cmd.exceptList) && (screens.Value != null && screens.Value.Count != 0))
                {
                    DestroyPanelsFrom(sn);
                }
            }

            // Rest panels
            string[] restList = new string[MenuRoot.m_DictAllPanel.Keys.Count];
            MenuRoot.m_DictAllPanel.Keys.CopyTo(restList, 0);

            foreach (var sn in restList)
            {
                if (IsNotInExcept(sn, cmd.exceptList))
                {
                    DestroyPanelsFrom(sn);
                }
            }
        }


        /// <sumarry>
        // This method is executed when using this.Publish(new PreLoadPanelCommand())
        /// </sumarry>
        public override void PreLoadPanelCommandHandler(PreLoadPanelCommand cmd)
        {
            base.PreLoadPanelCommandHandler(cmd);

            string pName = cmd.panelName;

            MenuRoot.LoadPanel.OnNext(new LoadPanelCommand()
            {
                panelName = pName,
            });

            MenuRoot.m_OnLoaded.Add(pName, (PanelViewModel pViewModel) =>
            {
                // Add to Menus
                MenuRoot.Panels.Add(pViewModel);

                // Add to dictionary
                MenuRoot.m_DictAllPanel.Add(pName, pViewModel);

                // DeActive
                pViewModel.isActive = false;

                // Event
                OnPanelLoad(pViewModel);
            });
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new ShowLoadingCommand())
        /// </sumarry>
        public override void ShowLoadingCommandHandler(ShowLoadingCommand data)
        {
            base.ShowLoadingCommandHandler(data);
            if (MenuRoot.loadingTop == null) return;

            ShieldTopOn(data.alpha);
            MenuRoot.loadingTop.isActive = true;

            MenuRoot.m_LoadingCount++;

            if (data.timeOut > 0)
            {
                StartCoroutine("IEHideLoading", new CallbackData(data.timeOut, data.callBack));
            }
        }

        /// <sumarry>
        // This method is executed when using this.Publish(new HideLoadingCommand())
        /// </sumarry>
        public override void HideLoadingCommandHandler(HideLoadingCommand cmd)
        {
            base.HideLoadingCommandHandler(cmd);

            HideLoading(cmd.isForceHide);
        }

        #endregion

        #region Private Method
        /// <summary>
        /// Hides the loading indicator. If you called n-times ShowLoading, you must to call n-times HideLoading to hide.
        /// </summary>
        /// <param name="isForceHide">If set to <c>true</c> is force hide without counting.</param>
        public void HideLoading(bool isForceHide = false)
        {
            if (MenuRoot.loadingTop == null || !MenuRoot.loadingTop.isActive) return;

            MenuRoot.m_LoadingCount--;

            if (MenuRoot.m_LoadingCount == 0 || isForceHide)
            {
                MenuRoot.m_LoadingCount = 0;
                ShieldTopOff();
                MenuRoot.loadingTop.isActive = false;
            }

            StopCoroutine("IEHideLoading");
        }

        private void BackToScreen()
        {
            StartCoroutine(IEBackScreen());
        }

        private IEnumerator IEScreen(string panelName, object data = null, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            yield return StartCoroutine(IEWaitForNotBusy());
            OpenScreen(panelName, data, onActive, onDeactive);
        }

        private IEnumerator IESubScreen(string panelName, object data = null, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            yield return StartCoroutine(IEWaitForNotBusy());
            OpenScreenAdd(panelName, data, false, onActive, onDeactive);
        }

        private IEnumerator IEPopUp(string panelName, object data = null, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            yield return StartCoroutine(IEWaitForNotBusy());

            if (IsPanelActive(panelName))
            {
                MenuRoot.m_CanClose = true;
            }

            while (IsPanelActive(panelName))
            {
                yield return new WaitForEndOfFrame();
            }
            MenuRoot.m_IsBusy = true;

            OpenPopUp(panelName, data, false, onActive, onDeactive);
        }

        private IEnumerator IEMainMenu(string panelName, object data = null, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            yield return new WaitForEndOfFrame();
            OpenMainMenu(panelName, onActive, onDeactive);
        }

        private IEnumerator IEClose(bool immediate = false, NoParamCallback callback = null)
        {
            if (MenuRoot.m_CanClose)
            {
                MenuRoot.m_CanClose = false;
                CloseAny(immediate, callback);
            }
            else
            {
                yield return StartCoroutine(IEWaitForNotBusy());
                CloseAny(immediate, callback);
            }
        }

        private bool IsPanelActive(string panelName)
        {
            string pName = panelName;

            if (MenuRoot.m_DictAllPanel.ContainsKey(pName))
            {
                return MenuRoot.m_DictAllPanel[pName].isActive;
            }

            return false;
        }

        private IEnumerator IEBackScreen()
        {
            yield return StartCoroutine(IEWaitForNotBusy());

            // Close all pop up
            CloseAllPopUp();

            // Close screens
            int n = MenuRoot.m_CurrentStackScreen.Count;
            if (n == 2)
            {
                CloseAny();
            }
            else
            {
                while (MenuRoot.m_CurrentStackScreen.Count > 1)
                {
                    CloseAny(true);
                }
            }
        }

        private IEnumerator IEWaitForNotBusy()
        {
            while (MenuRoot.m_IsBusy)
            {
                yield return new WaitForEndOfFrame();
            }

            MenuRoot.m_IsBusy = true;
        }

        private IEnumerator IEHideLoading(CallbackData callbackData)
        {
            yield return new WaitForSeconds(callbackData.TimeOut);

            HideLoading();

            if (callbackData.Callback != null)
            {
                callbackData.Callback();
            }
        }

        private void OpenScreen(string panelName, object data = null, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            string pName = panelName;

            // Check valid
            if (!CanOpenScreen(pName))
            {
                MenuRoot.m_IsBusy = false;
                return;
            }

            // Prev Stack screen
            Stack<string> prevStack = MenuRoot.m_CurrentStackScreen;

            // Set screen stack to current
            string top = StackScreen(pName);

            // onScreenStartChange
            if (MenuRoot.onScreenStartChange != null)
            {
                MenuRoot.onScreenStartChange(panelName);
            }

            // Hide All
            if (MenuRoot.m_ClearOnLoad)
            {
                HideAll(prevStack);
            }

            if (pName == top)// First time load
            {
                OpenPanel(pName, 0, 0, data, true, string.Empty,
                    //Animation End Call
                    () =>
                    {
                        if (!MenuRoot.m_ClearOnLoad)
                        {
                            HideAll(prevStack);
                        }
                    },
                    //Loaded Call
                    null,
                    onActive, onDeactive);
            }
            else // Not first time
            {
                if (!MenuRoot.m_ClearOnLoad)
                {
                    HideAll(prevStack);
                }

                // Just active screen
                ActiveAPanel(top);

                // Show and BGM
                PanelViewModel panel = GetPanel(top);
                if (panel != null)
                {
                    ShowAndBGMChangeOpen(top, panel.CurrentBGM);
                }

                MenuRoot.m_IsBusy = false;
            }
        }

        private void OpenScreenAdd(string panelName, object data = null, bool imme = false, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            string pName = panelName;

            // Check valid
            if (!CanOpenSubScreen(pName))
            {
                MenuRoot.m_IsBusy = false;
                return;
            }

            // Bot Scene
            string bot = StackScreenBottom(MenuRoot.m_CurrentStackScreen);
            if (string.IsNullOrEmpty(bot))
            {
                Debug.LogWarning("Main screen is not exist, can't add this screen above!");
                return;
            }

            // Next index
            int ip = -2 - MenuRoot.m_CurrentStackScreen.Count;
            float ic = 0.3f + (float)MenuRoot.m_CurrentStackScreen.Count / MenuConst.DEPTH_DISTANCE;

            // Prev Scene
            string preSn = MenuRoot.m_CurrentStackScreen.Peek();
            PanelViewModel panel = GetPanel(preSn);

            // Cur BGM
            string curBGM = panel.CurrentBGM;

            // Thread 1
            OpenPanel(pName, ip, ic, data, imme, curBGM,
                () =>
                {
                    // Set isCache of this scene same the isCache of bottom scene of stack screen
                    PanelViewModel newPanel = GetPanel(pName);
                    newPanel.isCache = true;

                    // Push stack
                    MenuRoot.m_CurrentStackScreen.Push(pName);
                },
                () =>
                {
                    // Thread 2
                    if (panel != null)
                    {
                        panel.FocusPanel.OnNext(new FocusPanelCommand() { isFocus = false });
                    }
                    // Animation
                    AnimType animType = (imme) ? AnimType.NO_ANIM : AnimType.HIDE_BACK;

                    panel.PlayPanelAnimation.OnNext(new PlayPanelAnimationCommand()
                    {
                        panelAnimType = animType,
                        onAniFinish = () =>
                        {
                            DeactiveAPanel(preSn);
                        }
                    });
                }, onActive, onDeactive);
        }

        private void OpenPopUp(string menuName, object data = null, bool imme = false, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            string mn = menuName;

            // Check valid
            if (!CanOpenPopUp(mn))
            {
                MenuRoot.m_IsBusy = false;
                return;
            }

            // Count
            int c = MenuRoot.m_StackPopUp.Count + 1;

            // Next index
            int ip = c;
            float ic = c;

            // Prev Scene
            PanelViewModel menu = null;
            string curBGM = null;

            // Highest popup
            if (MenuRoot.m_StackPopUp.Count >= 1)
            {
                string preSn = MenuRoot.m_StackPopUp.Peek();

                menu = GetPanel(preSn);
            }
            // Or highest screen
            else if (MenuRoot.m_CurrentStackScreen != null && MenuRoot.m_CurrentStackScreen.Count >= 1)
            {
                string preSn = MenuRoot.m_CurrentStackScreen.Peek();

                menu = GetPanel(preSn);

            }

            if (menu != null)
            {
                // Cur BGM
                curBGM = menu.CurrentBGM;

                // Shield
                ShieldOn(MenuRoot.m_StackPopUp.Count, MenuRoot.m_DefaultShieldColor);
            }
            else
            {
                ShieldOn(0, MenuRoot.m_DefaultShieldColor);
            }

            // Push stack
            MenuRoot.m_StackPopUp.Push(mn);

            OpenPanel(mn, ip, ic, data, imme, curBGM, () =>
            {
                if (menu != null)
                {
                    menu.FocusPanel.OnNext(new FocusPanelCommand());
                }
            }, null, onActive, onDeactive);
        }

        private void OpenMainMenu(string menuName, PanelCallBackDelegate onAcitive = null, PanelCallBackDelegate onDeactive = null)
        {
            string mn = menuName;

            // Next index
            int posIndex = -1;
            float canvasIndex = 0.8f;

            OpenPanel(mn, posIndex, canvasIndex, null, true, string.Empty, null, () => { MenuRoot.mainMenu = (MainMenuViewModel)MenuRoot.m_DictAllPanel[menuName]; }, onAcitive, onDeactive);
        }

        private bool CanOpenPopUp(string menuName)
        {
            if (MenuRoot.m_StackPopUp.Contains(menuName))
            {
                Debug.LogWarning("This popup was added to stack!");
                return false;
            }

            return true;
        }

        private bool CanOpenScreen(string menuName)
        {
            foreach (var pair in MenuRoot.m_DictScreen)
            {
                string bot = StackScreenBottom(pair.Value);

                bool isCurStack = (MenuRoot.m_CurrentStackScreen == pair.Value);
                bool isInStack = pair.Value.Contains(menuName);

                if (menuName == bot && isCurStack)
                {
                    if (MenuRoot.m_CurrentStackScreen.Count > 1)
                    {
                        BackToScreen();
                    }

                    return false;
                }

                if (menuName != bot && isInStack)
                {
                    Debug.LogWarning("This screen was added to stack!");
                    return false;
                }
            }

            return true;
        }

        private bool CanOpenSubScreen(string menuName)
        {
            foreach (var pair in MenuRoot.m_DictScreen)
            {
                bool isInStack = pair.Value.Contains(menuName);

                if (isInStack)
                {
                    Debug.LogWarning("This screen was added to stack!");
                    return false;
                }
            }

            return true;
        }

        private void HideAll(Stack<string> stackScreen)
        {
            // Close all pop up
            CloseAllPopUp();

            // Get is cache
            bool isClearStackScreen = false;
            string bot = StackScreenBottom(stackScreen);
            if (!string.IsNullOrEmpty(bot))
            {
                PanelViewModel menu = GetPanel(bot);

                bool isAdditive = (MenuRoot.m_IsAllAdditive || !MenuRoot.m_NotAdditivePanelList.Contains(bot));
                bool isNoCache = (menu != null && !menu.isCache);

                if (!isAdditive || isNoCache)
                {
                    isClearStackScreen = true;
                }
            }

            // Close top screen of this stack
            if (stackScreen.Count >= 1)
            {
                string top = stackScreen.Peek();
                ClosePanel(top, true, null, true);
            }

            // Clear if not cache
            if (isClearStackScreen)
            {
                if (!string.IsNullOrEmpty(bot))
                {
                    DestroyPanelsFrom(bot);
                }
                stackScreen.Clear();
            }
        }

        private void CloseAllPopUp()
        {
            while (MenuRoot.m_StackPopUp.Count >= 1)
            {
                ClosePopUp(true);
            }
        }

        private void ClosePopUp(bool imme, NoParamCallback callback = null)
        {
            string curPanel = MenuRoot.m_StackPopUp.Pop();
            string prePanel = string.Empty;

            if (MenuRoot.m_StackPopUp.Count >= 1)
            {
                prePanel = MenuRoot.m_StackPopUp.Peek();
            }
            else
            {
                if (MenuRoot.m_CurrentStackScreen != null && MenuRoot.m_CurrentStackScreen.Count >= 1)
                {
                    prePanel = MenuRoot.m_CurrentStackScreen.Peek();
                }
            }

            ClosePanel(curPanel, imme, () =>
            {
                // Shield Off
                ShieldOff();

                // Show & BGM change
                ShowAndBGMChangeClose(prePanel);

                // Callback
                if (callback != null)
                {
                    callback();
                }
            });
        }

        private void CloseScreen(bool imme, NoParamCallback callback = null)
        {
            string curPanel = MenuRoot.m_CurrentStackScreen.Pop();
            string prePanel = string.Empty;

            // Check if has prev scene
            if (MenuRoot.m_CurrentStackScreen.Count > 0)
            {
                prePanel = MenuRoot.m_CurrentStackScreen.Peek();
            }

            // Thread 1 (current scene animation)
            ClosePanel(curPanel, imme, () =>
            {
                if (callback != null)
                {
                    callback();
                }
            });

            // Thread 2 (previous scene animation)
            if (!string.IsNullOrEmpty(prePanel))
            {
                // Active
                ActiveAPanel(prePanel);

                // Animation
                AnimType animType = (imme) ? AnimType.NO_ANIM : AnimType.SHOW_BACK;

                PanelViewModel panel = GetPanel(prePanel);
                panel.PlayPanelAnimation.OnNext(new PlayPanelAnimationCommand()
                {
                    panelAnimType = animType,
                    onAniFinish = () =>
                    {
                        ShowAndBGMChangeClose(prePanel);
                    }
                });
            }
            else
            {
                // Do nothing or application quit
            }
        }

        private string StackScreen(string panelName)
        {
            // Check Exist
            if (!MenuRoot.m_DictScreen.ContainsKey(panelName))
            {
                MenuRoot.m_DictScreen.Add(panelName, new Stack<string>());
            }
            MenuRoot.m_CurrentStackScreen = MenuRoot.m_DictScreen[panelName];

            // If empty, push then return panelName
            if (MenuRoot.m_CurrentStackScreen.Count == 0)
            {
                MenuRoot.m_CurrentStackScreen.Push(panelName);
                return panelName;
            }

            // If not empty, return top of stack
            return MenuRoot.m_CurrentStackScreen.Peek();
        }

        private string StackScreenBottom(Stack<string> stack)
        {
            if (stack == null)
                return null;

            string r = null;
            foreach (string s in stack)
            {
                r = s;
            }
            return r;
        }

        private ShieldViewModel CreateShield(int i)
        {
            ShieldViewModel shield = shieldController.CreateShield();
            shield.isActive = true;
            shield.posIndex = i;
            shield.depthIndex = i;
            return shield;
        }

        private void ShowEmptyShield()
        {
            if (MenuRoot.shieldEmpty == null)
            {
                MenuRoot.shieldEmpty = CreateShield(MenuConst.SHIELD_TOP_INDEX - 2);
            }
            else
            {
                MenuRoot.shieldEmpty.isActive = true;
            }

            MenuRoot.shieldEmpty.color = new Color(0, 0, 0, 0);
            MenuRoot.m_ShieldEmptyCount++;

            // Lock
            LockTopScene();
        }

        private void HideEmptyShield()
        {
            if (MenuRoot.shieldEmpty != null)
            {
                MenuRoot.m_ShieldEmptyCount--;
                if (MenuRoot.m_ShieldEmptyCount == 0)
                {
                    MenuRoot.shieldEmpty.isActive = false;

                    // Unlock
                    UnlockTopScene();
                }
            }
        }

        protected void ShieldTopOn(float alpha)
        {
            if (MenuRoot.shieldTop == null)
            {
                MenuRoot.shieldTop = CreateShield(MenuConst.SHIELD_TOP_INDEX - 1);
            }
            else
            {
                MenuRoot.shieldTop.isActive = true;
            }

            Color color = MenuRoot.m_DefaultShieldColor;
            color.a = alpha;
            MenuRoot.shieldTop.color = color;

            // Lock
            LockTopScene();
        }

        protected void ShieldTopOff()
        {
            if (MenuRoot.shieldTop != null)
            {
                MenuRoot.shieldTop.isActive = false;
                UnlockTopScene();
            }
        }

        protected void ShieldOn(int i, Color color)
        {
            if (i < 0) return;

            if (MenuRoot.Shields.Count <= i)
            {
                // Create shield
                ShieldViewModel sh = CreateShield(i);
                sh.isActive = true;

                // Add to List
                MenuRoot.Shields.Add(sh);
            }
            else
            {
                MenuRoot.Shields[i].isActive = true;
            }

            MenuRoot.Shields[i].color = color;

            // Lock
            LockTopScene();
        }

        protected void ShieldOff()
        {
            int i = MenuRoot.m_StackPopUp.Count;

            if (i < 0) return;

            if (MenuRoot.Shields.Count >= i)
                MenuRoot.Shields[i].isActive = false;

            // Unlock
            UnlockTopScene();
        }

        protected void LockTopScene()
        {
            this.Publish(new LockScreenEvent());
        }

        protected void UnlockTopScene()
        {
            if (IsShieldOn()) return;
            this.Publish(new UnLockScreenEvent());
        }

        private bool IsShieldOn()
        {
            for (int i = 0; i < MenuRoot.Shields.Count; i++)
            {
                if (MenuRoot.Shields[i].isActive) return true;
            }

            if (MenuRoot.m_ShieldEmptyCount > 0) return true;

            if (MenuRoot.shieldTop != null && MenuRoot.shieldTop.isActive) return true;

            return false;
        }

        private void SetPosition(string panelName, int posIndex)
        {
            PanelViewModel panel = GetPanel(panelName);
            panel.posIndex = posIndex;
        }

        private void SetCanvases(string panelName, float canvasIndex)
        {
            PanelViewModel panel = GetPanel(panelName);
            panel.depthIndex = canvasIndex;
        }

        private void OpenPanel(string panelName, int posIndex, float canvasIndex, object data, bool imme, string curBGM,
            NoParamCallback onAnimEnded = null, NoParamCallback onLoaded = null, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            string pName = panelName;

            // Show Empty shield
            ShowEmptyShield();

            LoadOrActive(pName, () =>
            {
                if (onLoaded != null)
                {
                    onLoaded();
                }

                // Set canvas and position
                SetCanvases(pName, canvasIndex);
                SetPosition(pName, posIndex);

                // On Set
                PanelViewModel panel = GetPanel(pName);
                if (panel != null)
                {
                    panel.SetPanelData.OnNext(new SetPanelDataCommand()
                    {
                        panelData = data
                    });
                }

                // Animation
                AnimType animType = (imme) ? AnimType.NO_ANIM : AnimType.SHOW;
                panel.PlayPanelAnimation.OnNext(new PlayPanelAnimationCommand()
                {
                    panelAnimType = animType,
                    onAniFinish = () =>
                    {
                        // Show & BGM change
                        ShowAndBGMChangeOpen(pName, curBGM);

                        // Hide empty shield
                        HideEmptyShield();

                        // Call back
                        if (onAnimEnded != null) onAnimEnded();

                        // No busy
                        MenuRoot.m_IsBusy = false;
                    }
                });
            }, onActive, onDeactive);
        }

        private void ClosePanel(string panelName, bool imme, NoParamCallback onAnimEnded = null, bool forceDontDestroy = false)
        {
            string pName = panelName;
            AnimType animType = (imme) ? AnimType.NO_ANIM : AnimType.HIDE;

            PanelViewModel panel = GetPanel(pName);

            if (panel != null)
            {
                // Show Empty shield
                ShowEmptyShield();

                // Hide
                panel.FocusPanel.OnNext(new FocusPanelCommand() { isFocus = false });
                panel.HidePanel.OnNext(new HidePanelCommand());

                panel.PlayPanelAnimation.OnNext(new PlayPanelAnimationCommand()
                {
                    panelAnimType = animType,
                    onAniFinish = () =>
                    {
                        // Deactive
                        DeactiveAPanel(pName);

                        // Destroy or Cache
                        if (!forceDontDestroy)
                        {
                            DestroyOrCache(panel);
                        }

                        // Hide empty shield
                        HideEmptyShield();

                        // Next Step
                        if (onAnimEnded != null) onAnimEnded();

                        // No Busy
                        MenuRoot.m_IsBusy = false;
                    }
                });
            }
        }

        private void CloseAny(bool imme = false, NoParamCallback callback = null)
        {
            if (MenuRoot.m_StackPopUp.Count >= 1)
            {
                ClosePopUp(imme, callback);
            }
            else
            {
                if (MenuRoot.m_CurrentStackScreen.Count > 1)
                {
                    CloseScreen(imme, callback);
                }
                else
                {
                    if (MenuRoot.m_CurrentStackScreen.Count == 1)
                    {
                        // Do Nothing
                    }
                    MenuRoot.m_IsBusy = false;
                }
            }
        }

        private void LoadOrActive(string panelName, NoParamCallback onLoaded, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            string pName = panelName;

            if (MenuRoot.m_DictAllPanel.ContainsKey(pName))
            {
                // Callback
                SetActiveDeactiveCallback(pName, onActive, onDeactive);

                // Active
                ActiveAPanel(pName);

                // Event
                onLoaded();
            }
            else
            {
                MenuRoot.LoadPanel.OnNext(new LoadPanelCommand()
                {
                    panelName = pName,
                });

                MenuRoot.m_OnLoaded.Add(panelName, (PanelViewModel pViewModel) =>
                {
                    // Add to Menus
                    MenuRoot.Panels.Add(pViewModel);

                    // Add to dictionary
                    MenuRoot.m_DictAllPanel.Add(pName, pViewModel);

                    // Callback
                    SetActiveDeactiveCallback(pName, onActive, onDeactive);

                    // Active
                    ActiveAPanel(pName);

                    // Event
                    OnPanelLoad(pViewModel);
                    onLoaded();
                });
            }
        }

        private void SetActiveDeactiveCallback(string panelName, PanelCallBackDelegate onActive = null, PanelCallBackDelegate onDeactive = null)
        {
            PanelViewModel panel = GetPanel(panelName);
            if (panel != null)
            {
                panel.onActive = onActive;
                panel.onDeactive = onDeactive;
            }
        }

        private void DestroyOrCache(PanelViewModel panel)
        {
            bool isAdditive = (MenuRoot.m_IsAllAdditive || !MenuRoot.m_NotAdditivePanelList.Contains(panel.panelName));
            bool isNoCache = (panel != null && !panel.isCache);

            if (!isAdditive || isNoCache)
            {
                OnPanelUnLoad(panel);
                MenuRoot.m_DictAllPanel.Remove(panel.panelName);
                this.Publish(new PanelUnloadEvent() { panelName = panel.panelName });
                panel.ExecuteDestroyPanel();
            }
        }

        private void DestoryPanel(string panelName)
        {
            PanelViewModel panel = GetPanel(panelName);
            if (panel != null)
            {
                // Unload
                OnPanelUnLoad(panel);

                MenuRoot.m_DictAllPanel.Remove(panel.panelName);
                this.Publish(new PanelUnloadEvent() { panelName = panel.panelName });

                // Destory
                panel.ExecuteDestroyPanel();
            }
        }

        public string DestroyCurrentStack()
        {
            Stack<string> stack = MenuRoot.m_CurrentStackScreen;
            string s = null;

            while (stack != null && stack.Count >= 1)
            {
                s = stack.Pop();
                DestoryPanel(s);
            }

            MenuRoot.m_CurrentStackScreen = new Stack<string>();

            return s;
        }

        public void DestroyPanelsFrom(string panelName)
        {
            string pName = panelName;

            // Check if in the dict
            if (!MenuRoot.m_DictAllPanel.ContainsKey(pName))
            {
                Debug.LogWarning("No exist scene to destroy: " + pName);
                return;
            }

            // Check in pop up stack
            if (MenuRoot.m_StackPopUp.Contains(pName))
            {
                Debug.LogWarning("This pop up is active now: " + pName);
                return;
            }

            // Check in screen screen stacks
            foreach (var screens in MenuRoot.m_DictScreen)
            {
                Stack<string> stack = screens.Value;

                if (stack.Contains(pName))
                {
                    // Not in current stack
                    if (stack != MenuRoot.m_CurrentStackScreen)
                    {
                        if (stack.Count >= 1)
                        {
                            string s = stack.Pop();

                            while (string.Compare(s, pName) != 0)
                            {
                                DestoryPanel(s);
                                s = stack.Pop();
                            }
                            DestoryPanel(pName);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Can't destroy this scene. It's in an active stack: " + pName);
                        return;
                    }

                    // Break because found the stack contain scene.
                    break;
                }
            }

            // If not in any stack, just destroy
            if (MenuRoot.m_DictAllPanel.ContainsKey(pName))
            {
                DestoryPanel(pName);
                return;
            }
        }

        private void ActiveAPanel(string panelName)
        {
            PanelViewModel pViewModel = GetPanel(panelName);

            if (pViewModel != null)
            {
                if (pViewModel.onActive != null)
                {
                    pViewModel.onActive(pViewModel);
                }
                pViewModel.isActive = true;
            }
        }

        private void DeactiveAPanel(string panelName)
        {
            PanelViewModel pViewModel = GetPanel(panelName);
            if (pViewModel != null)
            {
                if (pViewModel.onDeactive != null)
                {
                    pViewModel.onDeactive(pViewModel);
                }
                pViewModel.isActive = false;
            }
        }

        private void ShowAndBGMChangeOpen(string panelName, string curBGM)
        {
            // Prev Scene
            PanelViewModel panel = GetPanel(panelName);

            if (panel != null)
            {
                panel.FocusPanel.OnNext(new FocusPanelCommand() { isFocus = true });
                panel.ShowPanel.OnNext(new ShowPanelCommand());
            }

            // On Scene Showed
            if (MenuRoot.onSceneFocus != null)
            {
                MenuRoot.onSceneFocus(panelName);
            }

            // BGM change
            if (panel != null)
            {
                BGMSceneOpen(curBGM, panel);
            }
        }

        private void ShowAndBGMChangeClose(string panelName)
        {
            // Prev Scene
            PanelViewModel panel = GetPanel(panelName);

            // On Show
            if (panel != null)
            {
                panel.FocusPanel.OnNext(new FocusPanelCommand() { isFocus = true });
            }

            // On Scene Showed
            if (MenuRoot.onSceneFocus != null)
            {
                MenuRoot.onSceneFocus(panelName);
            }

            // BGM change
            if (panel != null)
            {
                BGMSceneClose(panel);
            }
        }

        protected void BGMSceneOpen(string curBGM, PanelViewModel panel)
        {
            if (!string.IsNullOrEmpty(MenuRoot.m_GlobalBGM))
            {
                PlayBGM(MenuRoot.m_GlobalBGM);
            }
            else
            {
                switch (panel.BGMType)
                {
                    case BGM.NONE:
                        StopBGM();
                        break;

                    case BGM.PLAY:
                        panel.CurrentBGM = panel.BGMName;
                        if (!string.IsNullOrEmpty(panel.BGMName))
                        {
                            PlayBGM(panel.BGMName);
                        }
                        break;

                    case BGM.SAME:
                        panel.CurrentBGM = curBGM;
                        if (!string.IsNullOrEmpty(curBGM))
                        {
                            PlayBGM(curBGM);
                        }
                        break;
                    case BGM.CUSTOM:
                        StopBGM();
                        panel.CurrentBGM = panel.BGMName;
                        break;
                }
            }
        }

        protected void BGMSceneClose(PanelViewModel panel)
        {
            if (!string.IsNullOrEmpty(MenuRoot.m_GlobalBGM))
            {
                // Do nothing
            }
            else
            {
                switch (panel.BGMType)
                {
                    case BGM.NONE:
                        StopBGM();
                        break;

                    case BGM.PLAY:
                    case BGM.SAME:
                    case BGM.CUSTOM:
                        if (!string.IsNullOrEmpty(panel.CurrentBGM))
                        {
                            PlayBGM(panel.CurrentBGM);
                        }
                        break;
                }
            }
        }

        private PanelViewModel GetPanel(string panelName)
        {
            if (MenuRoot.m_DictAllPanel.ContainsKey(panelName))
            {
                return MenuRoot.m_DictAllPanel[panelName];
            }
            return null;
        }

        private bool IsNotInExcept(string sn, List<string> exceptList)
        {
            bool isOk = true;
            for (int i = 0; i < exceptList.Count; i++)
            {
                if (string.Compare(exceptList[i], sn) == 0)
                {
                    isOk = false;
                    break;
                }
            }

            return isOk;
        }

        public bool IsPanelInAnyStack(string panelName)
        {
            bool isInStack = false;

            if (MenuRoot.m_CurrentStackScreen != null)
            {
                isInStack = MenuRoot.m_CurrentStackScreen.Contains(panelName);
            }

            isInStack = (isInStack || MenuRoot.m_StackPopUp.Contains(panelName));

            return isInStack;
        }
        #endregion

        #region TODO
        /// <summary>
        /// Raises the scene load event.
        /// </summary>
        protected virtual void OnPanelLoad(PanelViewModel panel)
        {

        }

        /// <summary>
        /// Raises the scene unload event.
        /// </summary>
        protected virtual void OnPanelUnLoad(PanelViewModel panel)
        {

        }

        /// <summary>
        /// Play the BGM. Override it.
        /// </summary>
        /// <param name="bgmName">Bgm name.</param>
        protected virtual void PlayBGM(string bgmName)
        {
            //Debug.LogWarning("Play BGM: " + bgmName + ". You have to override function: PlayBGM");
        }

        /// <summary>
        /// Stops the BGM. Override it
        /// </summary>
        protected virtual void StopBGM()
        {
            //Debug.LogWarning("Stop BGM. You have to override function: StopBGM");
        }

        protected virtual void Quit()
        {
            Debug.Log("Quit: Please override this");
        }

        /// <summary>
        /// Set the global bgm. All scenes will have same BGM until ClearGlobalBgm() called.
        /// </summary>
        /// <param name="bgmName">Bgm name.</param>
        public void SetGlobalBgm(string bgmName)
        {
            MenuRoot.m_GlobalBGM = bgmName;
        }

        /// <summary>
        /// Clears the global bgm.
        /// </summary>
        public void ClearGlobalBgm()
        {
            MenuRoot.m_GlobalBGM = string.Empty;
        }
#endregion
    }
}
