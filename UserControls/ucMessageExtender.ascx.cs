/*Change Log:
 * 31-May-2011, Rakesh Goel - #CC01, Added new property
 * 28-Sep-2011, Rakesh Goel - #CC02, Set this.visible to false on first page load
 * 04-Oct-2011, Rakesh Goel - #CC03, Set view Error link to false by default if success message is being shown
 * 28-Aug-2012, Prashant Chitransh, #CC04: Change in logic (#CC02), Set this.visible to false on first OnInit event because on page load
 *                                         it will hide the error or any message we want to show on page load event of the page.
 * 29-Dec-2015, Priya Bhatia - #CC05, Anonymous
 * 29-Dec-2015, Priya Bhatia - #CC06, Set visibility of link to Generate Ticket in case of Serial No Mismatch from Update Feedback Interface
 * 01-Jun-2017, Kalpana, #CC07: hardcoded style removed and applied responsive css
 * 16-June-2017,Pankaj Mittal,#cc08: if invalid character message has throug by sql helper class then show a user friendly message to end user.
 * 25-Jun-2017, Sumit Maurya, #CC09, New check added to show "No Record Found." message.
 * 04-Sep-2017, Kalpana, #CC10: class changed
*/
using System;
using System.Xml;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace LuminousSMS.Controls
{
    public partial class ucMessageExtender : System.Web.UI.UserControl
    {
        private string _ErrorXml;
        #region Enum
        public enum EnumMessageType
        {
            ErrorUC = 1,
            Info = 2,
            Success = 3,
            Warning = 4
        }
        #endregion

        #region Wrapper methods
        public void ShowError(string message)
        {
            Show(EnumMessageType.ErrorUC, message);
        }
        public void ShowInfo(string message)
        {
            Show(EnumMessageType.Info, message);
        }
        public void ShowSuccess(string message)
        {
            Show(EnumMessageType.Success, message);
        }
        public void ShowWarning(string message)
        {
            Show(EnumMessageType.Warning, message);
        }
        public Boolean hideMessage
        {
            get { return this.Visible; }
            set
            {
                this.Visible = value;

                //  billu.Visible = false;  //#CC01 commented
                billu.Visible = this.Visible;  //#CC01 added


            }
        }

        public Boolean hideMessagelink //#CC01 new property created to replace hideMessage
        {
            get { return this.Visible; }
            set
            {
                this.Visible = !(value);
                billu.Visible = this.Visible;
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

        //void SetDivDetail()
        //{
        //    try
        //    {
        //        if (_ErrorXml != string.Empty)
        //        {
        //            StringReader myStreamReader = new StringReader(_ErrorXml);
        //            XmlDataDocument myXmlDataDocument = new XmlDataDocument();
        //            myXmlDataDocument.DataSet.ReadXml(myStreamReader);
        //            GridView GrdError = new GridView();
        //            GrdError.DataSource = myXmlDataDocument.DataSet.Tables[0];
        //            GrdError.DataBind();
        //            StringBuilder sb = new StringBuilder();
        //            StringWriter sw = new StringWriter(sb);
        //            HtmlTextWriter htw = new HtmlTextWriter(sw);
        //            GrdError.RenderControl(htw);
        //            Atag.Attributes.Clear();
        //            Atag.Attributes.Add("onclick", "PopupMessage('" + Server.UrlEncode(sb.ToString().Replace(" ","~")) + "');");
        //           // hidn.Value = sb.ToString();
        //            Atag.Visible = true;
        //            Show(EnumMessageType.Info, "Click to link for checking detail error!");
        //            sb = null;
        //            sw = null;
        //            htw = null;
        //            GrdError = null;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        ShowError(ex.Message);
        //    }
        //}

        // #CC04: OnInit event created.
        protected override void OnInit(EventArgs e)
        {
            if (!IsPostBack)
                this.Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)  //#CC02 added
        {
            /* #CC04: commented.
            if (!IsPostBack)
            {
                this.Visible = false;
            }*/
        }

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
                    GrdError.RowStyle.CssClass = "gridrow";//#CC10: class changed gridrowpop to gridrow
                    GrdError.AlternatingRowStyle.CssClass = "Altrow";//#CC10: class changed Altrowpop to Altrow
                    GrdError.HeaderStyle.CssClass = "gridheader";
                    GrdError.CellPadding = 4;
                    GrdError.CellSpacing = 1;
                    GrdError.Width = 400;
                    //GrdError.Height = 300;
                    GrdError.DataSource = myXmlDataDocument.DataSet.Tables[0];
                    GrdError.DataBind();
                    UCpnlMsggrid.Controls.Add(GrdError);

                    //StringBuilder sb = new StringBuilder();
                    //StringWriter sw = new StringWriter(sb);
                    //HtmlTextWriter htw = new HtmlTextWriter(sw);
                    //GrdError.RenderControl(htw);
                    // Atag.Attributes.Clear();
                    //  Atag.Attributes.Add("onclick", "PopupMessage('" + Server.UrlEncode(sb.ToString().Replace(" ", "~")) + "');");
                    // hidn.Value = sb.ToString();
                    billu.Visible = true;

                    Show(EnumMessageType.Info, "Click on View Error link for error details");

                    GrdError = null;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
        #endregion
        public void ShowAppError(Exception ex)
        {
            exMessage = ex;
            Show(EnumMessageType.ErrorUC, ex.Message);
        }
        private Exception exMessage
        {
            get;
            set;
        }
        #region Show control
        public void Show(EnumMessageType messageType, string message)
        {
            // CloseButton.Visible = ShowCloseButton;
            //if (Atag.Attributes.Count == 0)
            //{
            //    Atag.Visible = false;
            //}
            /*#cc08 added start*/
            if (message.Contains("Special character not allowed"))
            {
                message = "Invalid/Special character not allowed";
            }
            /*#cc08 added end*/
            /* #CC09 Add Start */
            if (message.ToLower().Contains("no data exists") || message.ToLower().Contains("error occur on your page"))
            {
                message = "No Record Found.";
            }
            /* #CC09 Add Start */
            string sMessageType = messageType.ToString().ToLower();  //#CC03 added
            if (sMessageType == "success")                           //#CC03 added   
            {
                billu.Visible = false;
            }
            litMessage.Text = message;
            //pnlUcMessageBox.CssClass = messageType.ToString().ToLower();      //#CC03 commented   
            pnlUcMessageBox.CssClass = sMessageType;                           //#CC03 added  
            this.Visible = true;
            if (sMessageType == "ErrorUC")                         //Pankaj Dhingra
                ExceptionPolicy.HandleException(exMessage, "ExceptionPolicy");
            txtfocus.Focus();/*#cc05 added*/
        }
        #endregion
    }
}
