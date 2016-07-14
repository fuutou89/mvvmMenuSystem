using UnityEngine.UI;

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
    
    
    public class ShieldView : ShieldViewBase {
        
        protected override void InitializeViewModel(uFrame.MVVM.ViewModels.ViewModel model) {
            base.InitializeViewModel(model);
            // NOTE: this method is only invoked if the 'Initialize ViewModel' is checked in the inspector.
            // var vm = model as ShieldViewModel;
            // This method is invoked when applying the data from the inspector to the viewmodel.  Add any view-specific customizations here.
        }
        
        public override void Bind() {
            base.Bind();
            // Use this.Shield to access the viewmodel.
            // Use this method to subscribe to the view-model.
            // Any designer bindings are created in the base implementation.
        }

        public override void colorChanged(Color arg1)
        {
            base.colorChanged(arg1);
            switch (uiType)
            {
                case UIType.nGUI:
#if UI_NGUI
                    UISprite sprite = this.gameObject.GetComponentInChildren<UISprite>();
                    sprite.color = arg1;
#endif
                    break;
                case UIType.uGUI:
#if UI_UGUI
                    Image image = this.gameObject.GetComponentInChildren<Image>();
                    image.color = arg1;
#endif
                    break;
            }
        }

        public override void depthIndexChanged(float arg1)
        {
            switch (uiType)
            {
                case UIType.nGUI:
#if UI_NGUI
                    UIPanel panel = this.gameObject.GetComponentInChildren<UIPanel>();
                    panel.depth = (int)((arg1 + 1) * MenuConst.DEPTH_DISTANCE);
#endif
                    break;
                case UIType.uGUI:
#if UI_UGUI
                    Canvas cv = this.gameObject.GetComponentInChildren<Canvas>();
                    cv.sortingOrder = ((int)arg1 + 1) * MenuConst.DEPTH_DISTANCE;
                    cv.worldCamera = CameraHelper.UICamera;
#endif
                    break;
            }
        }
    }
}
