using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BussinessLogic;

public partial class Controls_ucMessage : System.Web.UI.UserControl
{
    private string _ErrorXml ;

#region Enum
    public enum MessageType
    {
        ErrorUC = 1,
        Info = 2,
        Success = 3,
        Warning = 4
    }
    #endregion

#region Properties
    public bool ShowCloseButton { get; set; }
#endregion

#region Wrapper methods
            public void ShowError(string message)
            {
             
               
                    Show(MessageType.ErrorUC, message);
               
               

            }
            public void ShowError(string message,bool ShowGlobalMsg )
            {
                if (ShowGlobalMsg == true)
                {
                    Show(MessageType.ErrorUC, Resources.Messages.ErrorMsgTryAfterSometime);
                }
                else
                {
                    Show(MessageType.ErrorUC, message);
                }


            }
            public void ShowInfo(string message)
            {
            Show(MessageType.Info, message);
            }
            public void ShowSuccess(string message)
            {
            Show(MessageType.Success, message);
            Atag.Visible = false;
            }
            public void ShowWarning(string message)
            {
            Show(MessageType.Warning, message);
            }
            public Boolean ShowControl
            {
                get { return this.Visible; }
                set { this.Visible = value;
                Atag.Visible = false;
        
                }
            }
            public string XmlErrorSource
            {
                get { return _ErrorXml; }
                set
                {
                    _ErrorXml = value;
                    SetDivDetail();
                }
            }

    void SetGridData(){
        try
        {
            if (_ErrorXml != string.Empty)
            {
                StringReader myStreamReader = new StringReader(_ErrorXml);
                XmlDataDocument myXmlDataDocument = new XmlDataDocument();
                myXmlDataDocument.DataSet.ReadXml(myStreamReader);
                GridView GrdError = new GridView();
                GrdError.DataSource = myXmlDataDocument.DataSet.Tables[0];
                GrdError.DataBind();
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                HtmlTextWriter htw = new HtmlTextWriter(sw);
                GrdError.RenderControl(htw);
                Show(MessageType.Info, sb.ToString());
                sb = null;
                sw = null;
                htw = null;
                GrdError = null;
            }
        }
        catch (Exception ex)
        {

            ShowError(ex.Message);
        }
       

    
    }
	
    //Used to open in pop-up page
    void SetDivDetail()
    {
        try
        {
            if (_ErrorXml != string.Empty)
            {
                StringReader myStreamReader = new StringReader(_ErrorXml);
                XmlDataDocument myXmlDataDocument = new XmlDataDocument();
                myXmlDataDocument.DataSet.ReadXml(myStreamReader);
                GridView GrdError = new GridView();
                GrdError.RowStyle.CssClass = "gridrow";
                GrdError.AlternatingRowStyle.CssClass = "altrow";
                GrdError.HeaderStyle.CssClass = "gridheader";
                GrdError.CellPadding = 4;
                GrdError.CellSpacing = 1;
                GrdError.Width = 400;
				       //GrdError.Height = 300;
                GrdError.DataSource = myXmlDataDocument.DataSet.Tables[0];
                GrdError.DataBind();
                //pnlUcMessagegrid.Controls.Add(GrdError);
				
				test.Controls.Add(GrdError);

                //StringBuilder sb = new StringBuilder();
                //StringWriter sw = new StringWriter(sb);
                //HtmlTextWriter htw = new HtmlTextWriter(sw);
                //GrdError.RenderControl(htw);
               // Atag.Attributes.Clear();
              //  Atag.Attributes.Add("onclick", "PopupMessage('" + Server.UrlEncode(sb.ToString().Replace(" ", "~")) + "');");
                // hidn.Value = sb.ToString();
                Atag.Visible = true;

                Show(MessageType.Info, "Click on 'View Error' link to check error details.");
            
                GrdError = null;
            }
        }
        catch (Exception ex)
        {

            ShowError(ex.Message);
        }



    }

    

        #endregion

#region Show control
        public void Show(MessageType messageType, string message)
        {
               // CloseButton.Visible = ShowCloseButton;
                litMessage.Text =HttpUtility.HtmlDecode(message);
                pnlUcMessageBox.CssClass = messageType.ToString().ToLower();
                this.Visible = true;
               
        } 
#endregion






}
