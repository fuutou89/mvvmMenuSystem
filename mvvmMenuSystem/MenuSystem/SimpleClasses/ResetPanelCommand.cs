namespace mvvmMenuSystem
{
    using mvvmMenuSystem;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using uFrame.Json;
    using uFrame.Kernel;
    using uFrame.Kernel.Serialization;
    using uFrame.MVVM;
    using uFrame.MVVM.Bindings;


    public class ResetPanelCommand : ResetPanelCommandBase
    {
        public PanelCallBackDelegate onActive = null;
        public PanelCallBackDelegate onDeactive = null;
    }
}
