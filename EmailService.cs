﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.3053
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace WCFService
{
    using System.Runtime.Serialization;


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "WcfMail", Namespace = "http://schemas.datacontract.org/2004/07/WCFService")]
    public partial class WcfMail : object, System.Runtime.Serialization.IExtensibleDataObject
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string[] AttachmentFilesField;

        private string[] BccField;

        private string BodyField;

        private System.Text.Encoding BodyencodingField;

        private string BodyformatField;

        private string[] CcField;

        private bool IsHtmlField;

        private string[] MailToField;

        private System.Net.Mail.MailPriority PriorityField;

        private string SubjectField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] AttachmentFiles
        {
            get
            {
                return this.AttachmentFilesField;
            }
            set
            {
                this.AttachmentFilesField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Bcc
        {
            get
            {
                return this.BccField;
            }
            set
            {
                this.BccField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Body
        {
            get
            {
                return this.BodyField;
            }
            set
            {
                this.BodyField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Text.Encoding Bodyencoding
        {
            get
            {
                return this.BodyencodingField;
            }
            set
            {
                this.BodyencodingField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Bodyformat
        {
            get
            {
                return this.BodyformatField;
            }
            set
            {
                this.BodyformatField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] Cc
        {
            get
            {
                return this.CcField;
            }
            set
            {
                this.CcField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsHtml
        {
            get
            {
                return this.IsHtmlField;
            }
            set
            {
                this.IsHtmlField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] MailTo
        {
            get
            {
                return this.MailToField;
            }
            set
            {
                this.MailToField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Net.Mail.MailPriority Priority
        {
            get
            {
                return this.PriorityField;
            }
            set
            {
                this.PriorityField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Subject
        {
            get
            {
                return this.SubjectField;
            }
            set
            {
                this.SubjectField = value;
            }
        }
    }
}
namespace System.Globalization
{
    using System.Runtime.Serialization;


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "CodePageDataItem", Namespace = "http://schemas.datacontract.org/2004/07/System.Globalization")]
    public partial class CodePageDataItem : object, System.Runtime.Serialization.IExtensibleDataObject
    {

        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;

        private string m_bodyNameField;

        private int m_codePageField;

        private int m_dataIndexField;

        private string m_descriptionField;

        private uint m_flagsField;

        private string m_headerNameField;

        private int m_uiFamilyCodePageField;

        private string m_webNameField;

        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string m_bodyName
        {
            get
            {
                return this.m_bodyNameField;
            }
            set
            {
                this.m_bodyNameField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int m_codePage
        {
            get
            {
                return this.m_codePageField;
            }
            set
            {
                this.m_codePageField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int m_dataIndex
        {
            get
            {
                return this.m_dataIndexField;
            }
            set
            {
                this.m_dataIndexField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string m_description
        {
            get
            {
                return this.m_descriptionField;
            }
            set
            {
                this.m_descriptionField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public uint m_flags
        {
            get
            {
                return this.m_flagsField;
            }
            set
            {
                this.m_flagsField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string m_headerName
        {
            get
            {
                return this.m_headerNameField;
            }
            set
            {
                this.m_headerNameField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public int m_uiFamilyCodePage
        {
            get
            {
                return this.m_uiFamilyCodePageField;
            }
            set
            {
                this.m_uiFamilyCodePageField = value;
            }
        }

        [System.Runtime.Serialization.DataMemberAttribute(IsRequired = true)]
        public string m_webName
        {
            get
            {
                return this.m_webNameField;
            }
            set
            {
                this.m_webNameField = value;
            }
        }
    }
}
namespace System.Net.Mail
{
    using System.Runtime.Serialization;


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name = "MailPriority", Namespace = "http://schemas.datacontract.org/2004/07/System.Net.Mail")]
    public enum MailPriority : int
    {

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Normal = 0,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        Low = 1,

        [System.Runtime.Serialization.EnumMemberAttribute()]
        High = 2,
    }
}


[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace = "http://www.muyingzhijia.com/", ConfigurationName = "IEmailService")]
public interface IEmailService
{

    [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://www.muyingzhijia.com/IEmailService/SendEmail")]
    void SendEmail(string to, string subject, string body);

    [System.ServiceModel.OperationContractAttribute(IsOneWay = true, Action = "http://www.muyingzhijia.com/IEmailService/SendCmail")]
    void SendCmail(WCFService.WcfMail CMail);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IEmailServiceChannel : IEmailService, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class EmailServiceClient : System.ServiceModel.ClientBase<IEmailService>, IEmailService
{

    public EmailServiceClient()
    {
    }

    public EmailServiceClient(string endpointConfigurationName) :
        base(endpointConfigurationName)
    {
    }

    public EmailServiceClient(string endpointConfigurationName, string remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public EmailServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
        base(endpointConfigurationName, remoteAddress)
    {
    }

    public EmailServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
        base(binding, remoteAddress)
    {
    }

    public void SendEmail(string to, string subject, string body)
    {
        base.Channel.SendEmail(to, subject, body);
    }

    public void SendCmail(WCFService.WcfMail CMail)
    {
        base.Channel.SendCmail(CMail);
    }
}
