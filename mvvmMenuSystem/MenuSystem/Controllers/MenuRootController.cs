namespace mvvmMenuSystem
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;


    public class MenuRootController : MenuRootControllerBase
    {

        public override void InitializeMenuRoot(MenuRootViewModel viewModel)
        {
            base.InitializeMenuRoot(viewModel);
            // This is called when a MenuRootViewModel is created

            viewModel.m_LoadingPanelName = "Loading";
        }

        public override void LoadPanel(MenuRootViewModel viewModel, LoadPanelCommand arg)
        {
            base.LoadPanel(viewModel, arg);
        }
    }
}
