<%@ Application Language="C#" %>
<%@ Import Namespace="DataAccess" %>
<%@ Import Namespace="BussinessLogic"%>
<%@ Import Namespace="System.Collections" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="ZedAxis.Common.PasswordExpiryService" %>
<%@ Import Namespace="Microsoft.ApplicationBlocks.Data" %>
<%@ Import Namespace="System.Security.Principal" %>
<%@ Import Namespace="System.Configuration" %>
<%@ Import Namespace="System.Web.Security" %>
<%@ Import Namespace="System.Web.Util" %>
<%@ Import Namespace="System.Net" %>
<%@ Import Namespace="System.IO" %>

<script runat="server">
   
    UserData ObjUser;
    Authenticates objAuthenticate;
    //public static int noOfExpiryDays;
    void Application_Start(object sender, EventArgs e)
    {// Code that runs on application startup
        //Mailer.sConString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
        //System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
        System.Configuration.Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~/web.config");
        System.Net.Configuration.MailSettingsSectionGroup settings = (System.Net.Configuration.MailSettingsSectionGroup)(config.GetSectionGroup("system.net/mailSettings"));
        Mailer.smtpHostServer = settings.Smtp.Network.Host;
        if (Convert.ToBoolean(settings.Smtp.Network.DefaultCredentials) == false)
        {
            Mailer.smtpAuthentication = 1;
        }
        else
        {
            Mailer.smtpAuthentication = 0;
        }
        Mailer.smtpUserName = settings.Smtp.Network.UserName;
        Mailer.smtpPassword = settings.Smtp.Network.Password;
        Mailer.smtpPort = settings.Smtp.Network.Port;
        Application["encKey"] = "!!%$ZedAxisSDMS$%!!";
           string _ConnectionString = ConfigurationManager.ConnectionStrings["AppConString"].ConnectionString;
       
        string pr = ConfigurationManager.ConnectionStrings["AppConString"].ProviderName.ToString();
      
        ZedAxis.Common.PasswordExpiryService.TaskScheduler objPwdExpiryTaskSchd = default(ZedAxis.Common.PasswordExpiryService.TaskScheduler);
        objPwdExpiryTaskSchd = new TaskScheduler();
        objPwdExpiryTaskSchd.Name = "PasswordExpirySrv";
        objPwdExpiryTaskSchd.FilePath = Context.Server.MapPath(objPwdExpiryTaskSchd.FilePath);
        objPwdExpiryTaskSchd.StartTask();

            DataTable dtConfig = null;
            Authenticates objAuthenticate = new Authenticates();
            objAuthenticate.ConfigKey = "PWDEXPY";
            dtConfig = objAuthenticate.GetApplicationConfiguration();
            if (dtConfig.Rows.Count > 0)
            {
                Application["ExpiryDays"] = dtConfig.Rows[0]["ConfigValue"].ToString();
            }
            //MastersData.RegisterCache();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown

    }
    //public void Application_AcquireRequestState(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrEmpty(Session["LoginName"].ToString()))
    //    {
    //        Response.Redirect("login.aspx",false);
    //    }
    //}

    //public void Application_BeginRequest(Object sender, EventArgs e)
    //{
    //    string dummyURL = "http://localhost/ZedSalesV2/Masters/Dummy.aspx";
    //    if (HttpContext.Current.Request.Url.ToString() == dummyURL)
    //    {
    //        MastersData.registermeAgain();
    //    }

    //}
    
    
    
    public void Application_AuthenticateRequest(object sender, EventArgs e)
    {
        // Fires upon attempting to authenticate the use

     


    }
 
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        try
        {
            PageBase.Errorhandling(Server.GetLastError());
        }
        catch (Exception)
        {
            
           
        }
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
        try
        {
            objAuthenticate = new Authenticates();
            if (!string.IsNullOrEmpty(Session["LoginName"].ToString()))
            {
                objAuthenticate.LoginAttemp(Session["LoginName"].ToString(), 3);
            }
        }
        catch (Exception ex)
        {
          //  BussinessLogic.PageBase.Errorhandling(ex);
        }
    }
       
</script>
