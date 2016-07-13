namespace mvvmMenuSystem {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class PanelController : PanelControllerBase {
        
        public override void InitializePanel(PanelViewModel viewModel) {
            base.InitializePanel(viewModel);
            // This is called when a PanelViewModel is created
        }

        public override void FocusPanel(PanelViewModel viewModel, FocusPanelCommand arg)
        {
            base.FocusPanel(viewModel, arg);
        }


        public override void HidePanel(PanelViewModel viewModel) {
            base.HidePanel(viewModel);
        }
        
        public override void ShowPanel(PanelViewModel viewModel) {
            base.ShowPanel(viewModel);
        }
        
        public override void PlayPanelAnimation(PanelViewModel viewModel, PlayPanelAnimationCommand arg)
        {
            base.PlayPanelAnimation(viewModel, arg);
        }

        public override void SetPanelData(PanelViewModel viewModel, SetPanelDataCommand arg)
        {
            base.SetPanelData(viewModel, arg);
        }

        public override void DestroyPanel(PanelViewModel viewModel)
        {
            base.DestroyPanel(viewModel);
        }
    }
}
