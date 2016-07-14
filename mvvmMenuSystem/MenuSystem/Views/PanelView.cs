namespace mvvmMenuSystem {
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

    public enum UIType
    {
        uGUI,
        nGUI
    }
    
    public class PanelView : PanelViewBase 
    {
        public UIType uiType = UIType.nGUI;

        protected override void InitializeViewModel(uFrame.MVVM.ViewModels.ViewModel model) {
            base.InitializeViewModel(model);
            // NOTE: this method is only invoked if the 'Initialize ViewModel' is checked in the inspector.
            // var vm = model as PanelViewModel;
            // This method is invoked when applying the data from the inspector to the viewmodel.  Add any view-specific customizations here.
        }
        
        public override void Bind() {
            base.Bind();
            // Use this.Panel to access the viewmodel.
            // Use this method to subscribe to the view-model.
            // Any designer bindings are created in the base implementation.
            this.Publish(new PanelLoadedEvent()
            {
                panelName = this.gameObject.name,
                panelViewModel = Panel
            });
        }

        public override void posIndexChanged(int arg1)
        {
            base.posIndexChanged(arg1);

            switch (uiType)
            {
                case UIType.nGUI:
                    this.transform.localPosition = Vector3.zero;
                    break;
                case UIType.uGUI:
                    this.transform.localPosition = Vector3.zero;
                    break;
            }
        }

        public override void depthIndexChanged(float arg1)
        {
            base.depthIndexChanged(arg1);

            switch (uiType)
            {
                case UIType.nGUI:
                    SetPanels(arg1);
                    break;
                case UIType.uGUI:
                    SetCanvas(arg1);
                    break;
            }
        }

        private void SetPanels(float depth)
        {
#if UI_NGUI
            // Sort by depth
            List<UIPanel> panels = new List<UIPanel>(this.gameObject.GetComponentsInChildren<UIPanel>(true));
            panels = panels.OrderBy(n => n.depth).ToList<UIPanel>();

            // Re-set depth
            int count = 0;
            foreach (UIPanel panel in panels)
            {
                panel.depth = Mathf.RoundToInt(depth * MenuConst.DEPTH_DISTANCE) + count + 1;
                count++;
            }
#endif
        }

        private void SetCanvas(float depth)
        {
#if UI_UGUI
            // Sort by sorting order
            List<Canvas> canvases = new List<Canvas>(this.gameObject.GetComponentsInChildren<Canvas>(true));
            canvases = canvases.OrderBy(n => n.sortingOrder).ToList<Canvas>();

            // Re-set depth
            int count = 0;
            foreach (Canvas canvas in canvases)
            {
                canvas.sortingOrder = Mathf.RoundToInt(depth * MenuConst.DEPTH_DISTANCE) + count + 1;
                count++;

                canvas.enabled = false;
                canvas.enabled = true;

                canvas.worldCamera = CameraHelper.UICamera;
            }
#endif
        }

        public override void isActiveChanged(bool arg1)
        {
            base.isActiveChanged(arg1);
            this.gameObject.SetActive(arg1);
        }

        public override void PlayPanelAnimationExecuted(PlayPanelAnimationCommand command)
        {
            base.PlayPanelAnimationExecuted(command);
            StartCoroutine(IEPlayAnimation(command.panelAnimType, command.onAniFinish));
        }

        private IEnumerator IEPlayAnimation(AnimType animType, NoParamCallback callback = null)
        {
            MenuMotion ani = this.gameObject.GetComponentInChildren<MenuMotion>();
            if (ani != null && animType != AnimType.NO_ANIM)
            {
                ani.Reset(animType);

                yield return null;

                NoParamCallback play = null;
                float time = 0;

                switch (animType)
                {
                    case AnimType.SHOW:
                        play = ani.PlayShow;
                        time = ani.TimeShow();
                        break;
                    case AnimType.SHOW_BACK:
                        play = ani.PlayShowBack;
                        time = ani.TimeShowBack();
                        break;
                    case AnimType.HIDE:
                        play = ani.PlayHide;
                        time = ani.TimeHide();
                        break;
                    case AnimType.HIDE_BACK:
                        play = ani.PlayHideBack;
                        time = ani.TimeHideBack();
                        break;
                }

                if (play != null)
                {
                    play();
                }
                yield return StartCoroutine(Pause(time));
                yield return new WaitForEndOfFrame();

                OnAnimationFinish();

                if (callback != null)
                    callback();

            }
            else
            {
                if (animType == AnimType.NO_ANIM && ani != null)
                {
                    MoveAnimTransformPosition(ani, 0);
                }

                if (callback != null)
                    callback();
            }
        }

        private IEnumerator Pause(float time)
        {
            float pauseEndTime = Time.realtimeSinceStartup + time;
            while (Time.realtimeSinceStartup < pauseEndTime)
            {
                yield return 0;
            }
            yield return 0;
        }

        /// <summary>
        /// We should bring this scene to somewhere far when it awake.
        /// Then the animation will automatically bring it back at next frame.
        /// This trick remove flicker at the first frame.
        /// </summary>
        /// <param name="scene">Scene.</param>
        public void BringAnimationToVeryFar()
        {
            MenuMotion motion = this.transform.GetComponentInChildren<MenuMotion>();

            if (motion != null)
            {
                MoveAnimTransformPosition(motion, 99999);
            }
        }

        private void MoveAnimTransformPosition(MenuMotion an, float x)
        {
            switch (uiType)
            {
                case UIType.nGUI:
                    an.transform.localPosition = new Vector3(x, 0, 0);
                    break;
                case UIType.uGUI:
                    an.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
                    break;
            }
        }

        /// <summary>
        /// Raises the animation finish event.
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        protected virtual void OnAnimationFinish()
        {
            //Debug.Log("If you have the problem with NGUI which display not correctly when animation finish, you can SetDirty() to all UIPanel in UIPanel.list in this event for a refresh.");
        }

        public override void DestroyPanelExecuted(DestroyPanelCommand command)
        {
            base.DestroyPanelExecuted(command);
            Destroy(this.gameObject);
        }
    }
}
