﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace OpenDental.CallFireService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SMSServiceHttpBinding", Namespace="http://api.campaign.dialer.skyyconsulting.com")]
    public partial class SMSService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback sendSMSCampaignOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SMSService() {
            this.Url = global::OpenDental.Properties.Settings.Default.OpenDental_CallFireService_SMSService;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event sendSMSCampaignCompletedEventHandler sendSMSCampaignCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace="http://api.campaign.dialer.skyyconsulting.com", ResponseNamespace="http://api.campaign.dialer.skyyconsulting.com", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("out")]
        public long sendSMSCampaign([System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string key, [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)] string[] numbers, [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)] string campaignName) {
            object[] results = this.Invoke("sendSMSCampaign", new object[] {
                        key,
                        numbers,
                        campaignName});
            return ((long)(results[0]));
        }
        
        /// <remarks/>
        public void sendSMSCampaignAsync(string key, string[] numbers, string campaignName) {
            this.sendSMSCampaignAsync(key, numbers, campaignName, null);
        }
        
        /// <remarks/>
        public void sendSMSCampaignAsync(string key, string[] numbers, string campaignName, object userState) {
            if ((this.sendSMSCampaignOperationCompleted == null)) {
                this.sendSMSCampaignOperationCompleted = new System.Threading.SendOrPostCallback(this.OnsendSMSCampaignOperationCompleted);
            }
            this.InvokeAsync("sendSMSCampaign", new object[] {
                        key,
                        numbers,
                        campaignName}, this.sendSMSCampaignOperationCompleted, userState);
        }
        
        private void OnsendSMSCampaignOperationCompleted(object arg) {
            if ((this.sendSMSCampaignCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.sendSMSCampaignCompleted(this, new sendSMSCampaignCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    public delegate void sendSMSCampaignCompletedEventHandler(object sender, sendSMSCampaignCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.7.3056.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class sendSMSCampaignCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal sendSMSCampaignCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public long Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((long)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591