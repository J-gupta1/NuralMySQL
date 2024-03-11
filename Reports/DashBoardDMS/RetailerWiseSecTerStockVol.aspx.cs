#region Copyright(c) 2016 Zed-Axis Technologies All rights are reserved
/*/
 * ================================================================================================
 * ================================================================================================
 * COPYRIGHT (c) 2016 Zed Axis Technologies (P) Ltd.
 * ALL RIGHTS ARE RESERVED. REPRODUCTION OR TRANSMISSION IN WHOLE OR IN PART,
 * ANY FORM OR BY ANY MEANS, ELECTRONIC, MECHANICAL OR OTHERWISE, WITHOUT THE PRIOR PERMISSION OF THE COPYRIGHT OWNER.
 * ================================================================================================
 * Created By : Rakesh Raj
 * Created On : 19 Sept 2018
 * Description : Download Retailer Wise Secondary & Tertiary Stock Volume/Quantity from Excel Sheet.
 * ================================================================================================
 * Change Log: 
 * ------------- 
 * DD-MMM-YYYY, Name, #CCXX, Description
 * ====================================================================================================
 * 01/10/2018, Rakesh Raj, #CC01, Validate and Remove Table 2
 * 18/10/2018, Rakesh Raj, #CC03, Adding Total Sum Row in Volume Wise and Value Wise Report
 */
#endregion


using System;
using System.Data;
using DataAccess;
using BussinessLogic;


public partial class Reports_DashBoardDMS_RetailerWiseSecTerStockVol : PageBase //System.Web.UI.Page
{
    DataTable dt;
    DataSet Ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 900;
        ucMsg.Visible = false;
        if (!IsPostBack)
        {
           
            ucDateTo.Date = PageBase.ToDate;
            ucDateFrom.Date = PageBase.Fromdate;
            

        }
    }

    
    protected void lnkExportToExcel_Click(object sender, EventArgs e)
    {
        try
        {
            dt = new DataTable();
            String[] str = { "Retailer wise Sec Ter Stock Vol", "Retailer wise Sec Ter Stock Val" };
            using (ReportData obj = new ReportData())
            {

                //obj.SalesChannelId = PageBase.SalesChanelID;
                obj.UserId = PageBase.UserId;
                obj.ToDate = ucDateTo.Date;
                obj.FromDate = ucDateFrom.Date;
                Ds = obj.getRetailerWiseSecTerStockVolReport();
                if ((obj.error == "") || (obj.error==null))
                {
                    if (Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0) /*CC01*/
                    {
                        if (Ds.Tables.Count > 2 && Ds.Tables[2].Rows.Count == 0)/*CC01*/
                        {
                             ucMsg.ShowInfo("Please define models in Key30Model table and try again!");
                             return;
                        }

                     

                        //Remove Column RETAILERIDMTER, RETAILERIDMSEC, RETAILERIDMSTK
                        
                        Ds.Tables[0].Columns.Remove("RETAILERIDMTER");
                        Ds.Tables[0].Columns.Remove("RETAILERIDMSEC");
                        Ds.Tables[0].Columns.Remove("RETAILERIDMSTK");


                        Ds.Tables[0].Columns["Region"].SetOrdinal(0); 
                        Ds.Tables[0].Columns["NSM"].SetOrdinal(1);
                        Ds.Tables[0].Columns["RSM"].SetOrdinal(2);
                        Ds.Tables[0].Columns["ASM"].SetOrdinal(3);
                        Ds.Tables[0].Columns["T1 Partner Code"].SetOrdinal(4);
                        Ds.Tables[0].Columns["T1 Partner Name"].SetOrdinal(5);
                        Ds.Tables[0].Columns["T1 Partner State"].SetOrdinal(6);
                        Ds.Tables[0].Columns["T1 Partner City"].SetOrdinal(7);
                        Ds.Tables[0].Columns["RDs Code"].SetOrdinal(8);
                        Ds.Tables[0].Columns["RDs Name"].SetOrdinal(9);
                        Ds.Tables[0].Columns["RDs State"].SetOrdinal(10);
                        Ds.Tables[0].Columns["RDs City"].SetOrdinal(11);
                        Ds.Tables[0].Columns["Retailer Code"].SetOrdinal(12);
                        Ds.Tables[0].Columns["Retailer Name"].SetOrdinal(13);
                        Ds.Tables[0].Columns["Retailer State"].SetOrdinal(14);
                        Ds.Tables[0].Columns["Retailer City"].SetOrdinal(15);
                        Ds.Tables[0].Columns["RDs FOS Name"].SetOrdinal(16);
                        Ds.Tables[0].Columns["RDs FOS Mobile No."].SetOrdinal(17);
                        Ds.Tables[0].Columns["ISD"].SetOrdinal(18);
                        Ds.Tables[0].Columns["ISD Store Code"].SetOrdinal(19);
                        Ds.Tables[0].Columns["Retailer Classification"].SetOrdinal(20);
                        Ds.Tables[0].Columns["Status"].SetOrdinal(21);
                        Ds.Tables[0].Columns["LLM TER"].SetOrdinal(22);
                        Ds.Tables[0].Columns["LM TER"].SetOrdinal(23);
                        Ds.Tables[0].Columns["LMTD TER"].SetOrdinal(24);
                        Ds.Tables[0].Columns["MTD TER"].SetOrdinal(25);
                        Ds.Tables[0].Columns["LD TER"].SetOrdinal(26);
                        Ds.Tables[0].Columns["LLM SEC"].SetOrdinal(27);
                        Ds.Tables[0].Columns["LM SEC"].SetOrdinal(28);
                        Ds.Tables[0].Columns["LMTD SEC"].SetOrdinal(29);
                        Ds.Tables[0].Columns["MTD SEC"].SetOrdinal(30);
                        Ds.Tables[0].Columns["LD SEC"].SetOrdinal(31);
                        Ds.Tables[0].Columns["Total Retailer Stock"].SetOrdinal(32);
                        Ds.Tables[0].Columns["TER OVER SEC %"].SetOrdinal(33);

                        //DISPLYAING SUM COLUMN AT THE END OF DATA TABLE Volume Wise REPORT

                        /*CC03*/
                        DataRow row = Ds.Tables[0].NewRow();
                        row["Region"] = "Total";
                        row["LLM TER"] = Ds.Tables[0].Compute("Sum([LLM TER])", string.Empty);
                        row["LM TER"] = Ds.Tables[0].Compute("Sum([LM TER])", string.Empty);
                        row["LMTD TER"] = Ds.Tables[0].Compute("Sum([LMTD TER])", string.Empty);
                        row["MTD TER"] = Ds.Tables[0].Compute("Sum([MTD TER])", string.Empty);
                        row["LD TER"] = Ds.Tables[0].Compute("Sum([LD TER])", string.Empty);
                        row["LLM SEC"] = Ds.Tables[0].Compute("Sum([LLM SEC])", string.Empty);
                        row["LM SEC"] = Ds.Tables[0].Compute("Sum([LM SEC])", string.Empty);
                        row["LMTD SEC"] = Ds.Tables[0].Compute("Sum([LMTD SEC])", string.Empty);
                        row["MTD SEC"] = Ds.Tables[0].Compute("Sum([MTD SEC])", string.Empty);
                        row["LD SEC"] = Ds.Tables[0].Compute("Sum([LD SEC])", string.Empty);
                        row["Total Retailer Stock"] = Ds.Tables[0].Compute("Sum([Total Retailer Stock])", string.Empty);
                        //Taken Addition Column for this
                        row["TER OVER SEC %"] = Ds.Tables[0].Compute("Sum([TEROVERSEC])", string.Empty) + "%";


                        // Model Order Needs to be set based on model table from database 
                           int ColumnCtr = 33;
                          string strColumnNameSec="";
                           string strColumnNameTER="";
                           string strColumnNameStock= "";


                        if (Ds.Tables[2].Rows.Count > 0)
                        {  

                            for (int i = 0; i < Ds.Tables[2].Rows.Count; i++)
                            {
                                //strColumnName = Ds.Tables[1].Rows[i][0].ToString();
                             
                                strColumnNameSec = Ds.Tables[2].Rows[i][0].ToString() + " SEC";
                                strColumnNameTER = Ds.Tables[2].Rows[i][0].ToString() + " TER";
                                //#CC03
                                strColumnNameStock = Ds.Tables[2].Rows[i][0].ToString() + " Stock";
                              

                                if (Ds.Tables[0].Columns.Contains(strColumnNameSec) && Ds.Tables[0].Columns.Contains(strColumnNameTER))
                                {    
                                    //#CC03
                                    row[strColumnNameStock] = Ds.Tables[0].Compute("Sum([" + strColumnNameStock + "])", string.Empty);

                                    row[strColumnNameSec] = Ds.Tables[0].Compute("Sum([" + strColumnNameSec + "])", string.Empty);
                                    row[strColumnNameTER] = Ds.Tables[0].Compute("Sum([" + strColumnNameTER + "])", string.Empty);
                                
                                    ColumnCtr = ColumnCtr + 1;
                                    Ds.Tables[0].Columns[strColumnNameSec].SetOrdinal(ColumnCtr);
                                    Ds.Tables[0].Columns[strColumnNameTER].SetOrdinal(ColumnCtr + 1);
                                    ColumnCtr = ColumnCtr + 1;
                                }
                            }

                            //foreach (DataRow row0 in Ds.Tables[0].Rows)
                            //{
                            //    if (row0[strColumnNameSec] is System.DBNull || row0[strColumnNameSec]=="")
                            //    {
                            //        row[strColumnNameSec] = "0";
                                    
                            //    }
                            //}
                     
                        }


                        /*CC03*/
                        row["ZZALL OTHER SEC"] = Ds.Tables[0].Compute("Sum([ZZALL OTHER SEC])", string.Empty);
                        row["ZZALL OTHER TER"] = Ds.Tables[0].Compute("Sum([ZZALL OTHER TER])", string.Empty);

                        row["ZZALL OTHERS"] = Ds.Tables[0].Compute("Sum([ZZALL OTHERS])", string.Empty);
                        
                        //Last Columns 
                        row["Ter Target"] = Ds.Tables[0].Compute("Sum([Ter Target])", string.Empty);
                        row["Ter Ach"] = Ds.Tables[0].Compute("Sum([Ter Ach])", string.Empty);
                        row["Ter Ach%"] = Ds.Tables[0].Compute("Sum([TerAchp])", string.Empty);
                        row["Sec Target"] = Ds.Tables[0].Compute("Sum([Sec Target])", string.Empty);
                        row["Sec Ach"] = Ds.Tables[0].Compute("Sum([Sec Ach])", string.Empty);
                        row["Sec Ach%"] = Ds.Tables[0].Compute("Sum([SecAchp])", string.Empty);
                        row["SEC GOLM%"] = Ds.Tables[0].Compute("Sum([SECGOLM])", string.Empty);
                        row["TER GOLM%"] = Ds.Tables[0].Compute("Sum([TERGOLM])", string.Empty);
                        
                        Ds.Tables[0].Rows.Add(row);

                        //Rename Column ZZALL OTHERS, ZZALL OTHER SEC, ZZALL OTHER TER
                        Ds.Tables[0].Columns["ZZALL OTHERS"].Caption = "Others";
                        Ds.Tables[0].Columns["ZZALL OTHER SEC"].Caption = "Other SEC";
                        Ds.Tables[0].Columns["ZZALL OTHER TER"].Caption = "Other TER";
                        /*CC03*/
                        /*CC03 Remove Columns*/
                        Ds.Tables[0].Columns.Remove("TerAchp");
                        Ds.Tables[0].Columns.Remove("SecAchp");
                        Ds.Tables[0].Columns.Remove("SECGOLM");
                        Ds.Tables[0].Columns.Remove("TERGOLM");
                        Ds.Tables[0].Columns.Remove("TEROVERSEC");


                        /* #CC02 Vale Wise Report*/

                         //Remove Column RETAILERIDMTER, RETAILERIDMSEC, RETAILERIDMSTK

                         Ds.Tables[1].Columns.Remove("RETAILERIDMTER");
                         Ds.Tables[1].Columns.Remove("RETAILERIDMSEC");
                         Ds.Tables[1].Columns.Remove("RETAILERIDMSTK");


                         Ds.Tables[1].Columns["Region"].SetOrdinal(0);
                         Ds.Tables[1].Columns["NSM"].SetOrdinal(1);
                         Ds.Tables[1].Columns["RSM"].SetOrdinal(2);
                         Ds.Tables[1].Columns["ASM"].SetOrdinal(3);
                         Ds.Tables[1].Columns["T1 Partner Code"].SetOrdinal(4);
                         Ds.Tables[1].Columns["T1 Partner Name"].SetOrdinal(5);
                         Ds.Tables[1].Columns["T1 Partner State"].SetOrdinal(6);
                         Ds.Tables[1].Columns["T1 Partner City"].SetOrdinal(7);
                         Ds.Tables[1].Columns["RDs Code"].SetOrdinal(8);
                         Ds.Tables[1].Columns["RDs Name"].SetOrdinal(9);
                         Ds.Tables[1].Columns["RDs State"].SetOrdinal(10);
                         Ds.Tables[1].Columns["RDs City"].SetOrdinal(11);
                         Ds.Tables[1].Columns["Retailer Code"].SetOrdinal(12);
                         Ds.Tables[1].Columns["Retailer Name"].SetOrdinal(13);
                         Ds.Tables[1].Columns["Retailer State"].SetOrdinal(14);
                         Ds.Tables[1].Columns["Retailer City"].SetOrdinal(15);
                         Ds.Tables[1].Columns["RDs FOS Name"].SetOrdinal(16);
                         Ds.Tables[1].Columns["RDs FOS Mobile No."].SetOrdinal(17);
                         Ds.Tables[1].Columns["ISD"].SetOrdinal(18);
                         Ds.Tables[1].Columns["ISD Store Code"].SetOrdinal(19);
                         Ds.Tables[1].Columns["Retailer Classification"].SetOrdinal(20);
                         Ds.Tables[1].Columns["Status"].SetOrdinal(21);
                         Ds.Tables[1].Columns["LLM TER"].SetOrdinal(22);
                         Ds.Tables[1].Columns["LM TER"].SetOrdinal(23);
                         Ds.Tables[1].Columns["LMTD TER"].SetOrdinal(24);
                         Ds.Tables[1].Columns["MTD TER"].SetOrdinal(25);
                         Ds.Tables[1].Columns["LD TER"].SetOrdinal(26);
                         Ds.Tables[1].Columns["LLM SEC"].SetOrdinal(27);
                         Ds.Tables[1].Columns["LM SEC"].SetOrdinal(28);
                         Ds.Tables[1].Columns["LMTD SEC"].SetOrdinal(29);
                         Ds.Tables[1].Columns["MTD SEC"].SetOrdinal(30);
                         Ds.Tables[1].Columns["LD SEC"].SetOrdinal(31);
                         Ds.Tables[1].Columns["Total Retailer Stock"].SetOrdinal(32);
                         Ds.Tables[1].Columns["TER OVER SEC %"].SetOrdinal(33);

                         //DISPLYAING SUM COLUMN AT THE END OF DATA TABLE VALUE WISE REPORT

                         /*CC03*/
                         DataRow rowval = Ds.Tables[1].NewRow();
                         rowval["Region"] = "Total";
                         rowval["LLM TER"] = Ds.Tables[1].Compute("Sum([LLM TER])", string.Empty);
                         rowval["LM TER"] = Ds.Tables[1].Compute("Sum([LM TER])", string.Empty);
                         rowval["LMTD TER"] = Ds.Tables[1].Compute("Sum([LMTD TER])", string.Empty);
                         rowval["MTD TER"] = Ds.Tables[1].Compute("Sum([MTD TER])", string.Empty);
                         rowval["LD TER"] = Ds.Tables[1].Compute("Sum([LD TER])", string.Empty);
                         rowval["LLM SEC"] = Ds.Tables[1].Compute("Sum([LLM SEC])", string.Empty);
                         rowval["LM SEC"] = Ds.Tables[1].Compute("Sum([LM SEC])", string.Empty);
                         rowval["LMTD SEC"] = Ds.Tables[1].Compute("Sum([LMTD SEC])", string.Empty);
                         rowval["MTD SEC"] = Ds.Tables[1].Compute("Sum([MTD SEC])", string.Empty);
                         rowval["LD SEC"] = Ds.Tables[1].Compute("Sum([LD SEC])", string.Empty);
                         rowval["Total Retailer Stock"] = Ds.Tables[1].Compute("Sum([Total Retailer Stock])", string.Empty);
                         //Taken Addition Column for this
                         rowval["TER OVER SEC %"] = Ds.Tables[1].Compute("Sum([TEROVERSEC])", string.Empty);



                         // Model Order Needs to be set based on model table from database 
                         ColumnCtr = 33;
                          strColumnNameSec = "";
                          strColumnNameTER = "";

                         if (Ds.Tables[2].Rows.Count > 0)
                         {
                             for (int i = 0; i < Ds.Tables[2].Rows.Count; i++)
                             {
                                 //strColumnName = Ds.Tables[1].Rows[i][0].ToString();

                                 strColumnNameSec = Ds.Tables[2].Rows[i][0].ToString() + " SEC";
                                 strColumnNameTER = Ds.Tables[2].Rows[i][0].ToString() + " TER";
                                 //#CC03
                                 strColumnNameStock = Ds.Tables[2].Rows[i][0].ToString() + " Stock";
                               

                                 if (Ds.Tables[1].Columns.Contains(strColumnNameSec) && Ds.Tables[1].Columns.Contains(strColumnNameTER))
                                 {
                                     //#CC03
                                     rowval[strColumnNameStock] = Ds.Tables[1].Compute("Sum([" + strColumnNameStock + "])", string.Empty);
                                     rowval[strColumnNameSec] = Ds.Tables[1].Compute("Sum([" + strColumnNameSec + "])", string.Empty);
                                     rowval[strColumnNameTER] = Ds.Tables[1].Compute("Sum([" + strColumnNameTER + "])", string.Empty);

                                     ColumnCtr = ColumnCtr + 1;
                                     Ds.Tables[1].Columns[strColumnNameSec].SetOrdinal(ColumnCtr);
                                     Ds.Tables[1].Columns[strColumnNameTER].SetOrdinal(ColumnCtr + 1);
                                     ColumnCtr = ColumnCtr + 1;
                                 }
                             }

                         }

                         /*CC03*/
                         rowval["ZZALL OTHER SEC"] = Ds.Tables[1].Compute("Sum([ZZALL OTHER SEC])", string.Empty);
                         rowval["ZZALL OTHER TER"] = Ds.Tables[1].Compute("Sum([ZZALL OTHER TER])", string.Empty);
                         rowval["ZZALL OTHERS"] = Ds.Tables[1].Compute("Sum([ZZALL OTHERS])", string.Empty);

                         //Last Columns 
                         rowval["Ter Target"] = Ds.Tables[1].Compute("Sum([Ter Target])", string.Empty);
                         rowval["Ter Ach"] = Ds.Tables[1].Compute("Sum([Ter Ach])", string.Empty);
                         rowval["Ter Ach%"] = Ds.Tables[1].Compute("Sum([TerAchp])", string.Empty);
                         rowval["Sec Target"] = Ds.Tables[1].Compute("Sum([Sec Target])", string.Empty);
                         rowval["Sec Ach"] = Ds.Tables[1].Compute("Sum([Sec Ach])", string.Empty);
                         rowval["Sec Ach%"] = Ds.Tables[1].Compute("Sum([SecAchp])", string.Empty);
                         rowval["SEC GOLM%"] = Ds.Tables[1].Compute("Sum([SECGOLM])", string.Empty);
                         rowval["TER GOLM%"] = Ds.Tables[1].Compute("Sum([TERGOLM])", string.Empty);

                         Ds.Tables[1].Rows.Add(rowval);

                         //Rename Column ZZALL OTHERS, ZZALL OTHER SEC, ZZALL OTHER TER
                         Ds.Tables[1].Columns["ZZALL OTHERS"].Caption = "Others";
                         Ds.Tables[1].Columns["ZZALL OTHER SEC"].Caption = "Other SEC";
                         Ds.Tables[1].Columns["ZZALL OTHER TER"].Caption = "Other TER";
                         /*CC03 Remove Columns*/
                         Ds.Tables[1].Columns.Remove("TerAchp");
                         Ds.Tables[1].Columns.Remove("SecAchp");
                         Ds.Tables[1].Columns.Remove("SECGOLM");
                         Ds.Tables[1].Columns.Remove("TERGOLM");
                         Ds.Tables[1].Columns.Remove("TEROVERSEC");
                        
                         
                        Ds.AcceptChanges();

                     //Removing Key30Model Data that is Table 2
                         if (Ds.Tables.Count > 2)/*CC01*/
                         {
                             Ds.Tables.Remove(Ds.Tables[2]); //Removing Table[1]
                         }


                        String FilePath = Server.MapPath("../../");
                        string FilenameToexport = "RetailerWiseSecTerStock";
                        PageBase.RootFilePath = FilePath;
                        //PageBase.ExportToExecl(dtcopy, FilenameToexport);
                    
                        ZedService.Utility.ZedServiceUtil.ExportToExecl(Ds, FilenameToexport,2,str);
                        
                      
                    }
                    else
                    {
                        ucMsg.ShowError(Resources.Messages.NoRecord);

                    }
                }
                else
                {
                    ucMsg.ShowError(obj.error);
                }
            }
        }

        catch (Exception ex)
        {
            ucMsg.ShowError(ex.Message.ToString());
        }
    }
}