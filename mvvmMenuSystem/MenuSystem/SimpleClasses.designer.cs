// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace mvvmMenuSystem {
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
    
    
    public class DestoryInactivePanelsCommandBase : object {
        
        private List<String> _exceptList;
        
        public List<String> exceptList {
            get {
                return _exceptList;
            }
            set {
                _exceptList = value;
            }
        }
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class HideLoadingCommandBase : object {
        
        private Boolean _isForceHide;
        
        public Boolean isForceHide {
            get {
                return _isForceHide;
            }
            set {
                _isForceHide = value;
            }
        }
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            jsonObject.Add("isForceHide", new JSONData(this.isForceHide));
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
            if (node["isForceHide"] != null) {
                this.isForceHide = node["isForceHide"].AsBool;
            }
        }
    }
    
    public class OpenScreenCommandBase : OpenPanelCommand {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class PanelLoadedEventBase : object {
        
        private String _panelName;
        
        private PanelViewModel _panelViewModel;
        
        public String panelName {
            get {
                return _panelName;
            }
            set {
                _panelName = value;
            }
        }
        
        public PanelViewModel panelViewModel {
            get {
                return _panelViewModel;
            }
            set {
                _panelViewModel = value;
            }
        }
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            jsonObject.Add("panelName", new JSONData(this.panelName));
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
            if (node["panelName"] != null) {
                this.panelName = node["panelName"].AsString;
            }
        }
    }
    
    public class OpenSubScreenCommandBase : OpenPanelCommand {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class OpenMainMenuCommandBase : OpenPanelCommand {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class ShowLoadingCommandBase : object {
        
        private Single _alpha;
        
        private Single _timeOut;
        
        private Single _delay;
        
        private String _callBack;
        
        public Single alpha {
            get {
                return _alpha;
            }
            set {
                _alpha = value;
            }
        }
        
        public Single timeOut {
            get {
                return _timeOut;
            }
            set {
                _timeOut = value;
            }
        }
        
        public Single delay {
            get {
                return _delay;
            }
            set {
                _delay = value;
            }
        }
        
        public String callBack {
            get {
                return _callBack;
            }
            set {
                _callBack = value;
            }
        }
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            jsonObject.Add("alpha", new JSONData(this.alpha));
            jsonObject.Add("timeOut", new JSONData(this.timeOut));
            jsonObject.Add("delay", new JSONData(this.delay));
            jsonObject.Add("callBack", new JSONData(this.callBack));
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
            if (node["alpha"] != null) {
                this.alpha = node["alpha"].AsFloat;
            }
            if (node["timeOut"] != null) {
                this.timeOut = node["timeOut"].AsFloat;
            }
            if (node["delay"] != null) {
                this.delay = node["delay"].AsFloat;
            }
            if (node["callBack"] != null) {
                this.callBack = node["callBack"].AsString;
            }
        }
    }
    
    public class LockScreenEventBase : object {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class CreateLoadingCommandBase : object {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class HideMainMenuCommandBase : object {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class PanelUnloadEventBase : object {
        
        private String _panelName;
        
        public String panelName {
            get {
                return _panelName;
            }
            set {
                _panelName = value;
            }
        }
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            jsonObject.Add("panelName", new JSONData(this.panelName));
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
            if (node["panelName"] != null) {
                this.panelName = node["panelName"].AsString;
            }
        }
    }
    
    public class ClosePanelCommandBase : object {
        
        private Boolean _immediate;
        
        public Boolean immediate {
            get {
                return _immediate;
            }
            set {
                _immediate = value;
            }
        }
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            jsonObject.Add("immediate", new JSONData(this.immediate));
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
            if (node["immediate"] != null) {
                this.immediate = node["immediate"].AsBool;
            }
        }
    }
    
    public class OpenPanelCommandBase : object {
        
        private String _onDeactive;
        
        private object _data;
        
        private String _panelName;
        
        public String onDeactive {
            get {
                return _onDeactive;
            }
            set {
                _onDeactive = value;
            }
        }
        
        public object data {
            get {
                return _data;
            }
            set {
                _data = value;
            }
        }
        
        public String panelName {
            get {
                return _panelName;
            }
            set {
                _panelName = value;
            }
        }
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            jsonObject.Add("onDeactive", new JSONData(this.onDeactive));
            jsonObject.Add("panelName", new JSONData(this.panelName));
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
            if (node["onDeactive"] != null) {
                this.onDeactive = node["onDeactive"].AsString;
            }
            if (node["panelName"] != null) {
                this.panelName = node["panelName"].AsString;
            }
        }
    }
    
    public class OpenPopupCommandBase : OpenPanelCommand {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class PreLoadPanelCommandBase : object {
        
        private String _panelName;
        
        public String panelName {
            get {
                return _panelName;
            }
            set {
                _panelName = value;
            }
        }
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            jsonObject.Add("panelName", new JSONData(this.panelName));
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
            if (node["panelName"] != null) {
                this.panelName = node["panelName"].AsString;
            }
        }
    }
    
    public class ResetPanelCommandBase : object {
        
        private object _data;
        
        public object data {
            get {
                return _data;
            }
            set {
                _data = value;
            }
        }
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class ShowMainMenuCommandBase : object {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class UnLockScreenEventBase : object {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
    
    public class GoHomeScreenCommandBase : object {
        
        public virtual string Serialize() {
            var jsonObject = new JSONClass();
            return jsonObject.ToString();
        }
        
        public virtual void Deserialize(string json) {
            var node = JSON.Parse(json);
        }
    }
}
