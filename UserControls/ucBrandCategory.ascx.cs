#region Copyright(c) 2019 Zed-Axis Technologies All rights are reserved
/*/
* ====================================================================================================
* <copyright company="Zed Axis Technologies">
* COPYRIGHT (c) 2010 Zed Axis Technologies (P) Ltd. 
* ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART, 
* ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, 
* WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
* </copyright>
* ====================================================================================================
* Created By    : Balram Jha
* Module        : 
* Description   : Brand-category 
* Table Name    : 
* ====================================================================================================
* Reviewed By :
 ====================================================================================================
* Change Log :
 
 ====================================================================================================
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DataAccess;
using BussinessLogic;


public partial class UserControls_ucBrandCategory : System.Web.UI.UserControl
    {
       
        #region Public Properties
    public Int16 EntityTypeId { get; set; }
    public int EntityId { get; set; }
        
        public string SelectedBrandCategoryIds
        {
            set
            {
                string[] selectedBrCat = value.Split(',');
                for (int counter = 0; counter < chkListBrandCategory.Items.Count; counter++)
                {
                        chkListBrandCategory.Items[counter].Selected = false;
                }
                for (int counterSelceted = 0; counterSelceted < selectedBrCat.Length; counterSelceted++)
                {
                    for (int counter = 0; counter < chkListBrandCategory.Items.Count; counter++)
                    {
                        if (chkListBrandCategory.Items[counter].Value == selectedBrCat[counterSelceted].Trim())
                            chkListBrandCategory.Items[counter].Selected = true;
                    }
                }
            }
            get
            {
                string BrandCategoryIds="";
                for(int counter =0;counter< chkListBrandCategory.Items.Count;counter++)
                {
                    if (chkListBrandCategory.Items[counter].Selected)
                        BrandCategoryIds = BrandCategoryIds + chkListBrandCategory.Items[counter].Value.ToString() + ",";
                }
                if (BrandCategoryIds.Length > 0)
                {
                    return BrandCategoryIds = BrandCategoryIds.Substring(0, BrandCategoryIds.Length - 1);
                }
                else
                    return "";
            }
        }

        #endregion

        

        #region Public Methods
   
        public void BindBrandCategoryList()
        {
            try
            {
                using (ClsSalesChannelBrandMapping BrandCategory = new ClsSalesChannelBrandMapping())
                {
                    BrandCategory.SalesChannelID = EntityId;
                    BrandCategory.SalesChannelTypeID = EntityTypeId;
                    DataSet dsBrandCategory = BrandCategory.GetAllBrandandCategory();
                    if(dsBrandCategory !=null)
                    {
                        chkListBrandCategory.DataSource = dsBrandCategory.Tables[0];
                        chkListBrandCategory.DataTextField = "BrandCategory";
                        chkListBrandCategory.DataValueField = "BrandCategoryMasterID";
                        chkListBrandCategory.DataBind();
                        for(int counter=0;counter< chkListBrandCategory.Items.Count;counter++ )
                        {
                            chkListBrandCategory.Items[counter].Selected = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
        }
        

        #endregion

    }
