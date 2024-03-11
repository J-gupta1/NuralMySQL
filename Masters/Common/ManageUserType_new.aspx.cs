#region Copyright(c) 2010 Zed-Axis Technologies All rights are reserved
/*/* ================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ==================================================================================================
* Created By : Sumit maurya
* Role :       Software Engineer
* Module :     Admin
* Description :This page is used for Managing User 
* ===================================================================================================
* Reviewed By : 
=====================================================================================================
Modification On       Modified By          Modification    
---------------      -----------          -----------------------------------------------------------  

=====================================================================================================
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;
//using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.ApplicationBlocks.Data;
//using LuminousSMS.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Text;
//using ZedEBS;
//using ZedEBS.Admin;
using BussinessLogic;

    public partial class Masters_Common_ManageUserType_new : PageBase
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                }
            }
            catch (Exception ex)
            {
               
            }
        }


        protected void btnModuleSearch_Click(object sender, EventArgs e)
        {

        }
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }
}
