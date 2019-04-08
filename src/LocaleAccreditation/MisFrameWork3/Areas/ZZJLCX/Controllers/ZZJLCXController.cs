using MisFrameWork.core;
using MisFrameWork.core.db;
using MisFrameWork.core.db.Support;
using MisFrameWork3.Classes.Controller;
using MisFrameWork3.Classes.Membership;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MisFrameWork3.Areas.ZZJLCX.Controllers
{
    public class ZZJLCXController : FWBaseController
    {
        // GET: ZZJLCX/ZZJLCX
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewShowDetail()
        {
            return View();
        }

        public ActionResult JsonDataList()//业务主界面数据查询函数
        {
            //接收的基本查询参数有： id,limit,offset,search,sort,order            
            //__TIPS__*:根据表结构，修改以下函数的参数

            Condition cdtIds = new Condition();

            string MACHINENO = Request["MACHINENO"];
            string ZZDW = Request["ZZDW"];
            string KH = Request["KH"];
            string ACCEPT_DATE = Request["ACCEPT_DATE"];
            string ACCEPT_DATE2 = Request["ACCEPT_DATE2"];
            if (!String.IsNullOrEmpty(MACHINENO))
            {
                cdtIds.AddSubCondition("AND", "ZZSBID", "like", "%" + MACHINENO + "%");
            }
            if (!String.IsNullOrEmpty(KH))
            {
                cdtIds.AddSubCondition("AND", "GMSFHM", "like", "%" + KH + "%");
            }
            //if (!String.IsNullOrEmpty(ZZDW))
            //{
            //    Session session = DbUtilityManager.Instance.DefaultDbUtility.CreateAndOpenSession();
            //    Condition cdtId3 = new Condition();
            //    cdtId3.AddSubCondition("AND", "SBFZR", "=", ZZY);
            //    List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(session, "B_MACHINE", cdtId3, "*", null, null, -1, -1);
            //    if (record.Count == 0)
            //    {
            //        cdtIds.AddSubCondition("AND", "ZZSBID", "like", "%" + MACHINENO + "%");
            //    }
            //}
            if (!String.IsNullOrEmpty(ZZDW))
            {
                cdtIds.AddSubCondition("AND", "ZZXXZZDW", "=", ZZDW);
            }
            if (!String.IsNullOrEmpty(ACCEPT_DATE))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                cdtIds.AddSubCondition("AND", "ZZXXZZRQ", ">=", Convert.ToDateTime(ACCEPT_DATE));
            }
            if (!String.IsNullOrEmpty(ACCEPT_DATE2))
            {
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                cdtIds.AddSubCondition("AND", "ZZXXZZRQ", "<=", Convert.ToDateTime(ACCEPT_DATE2));
            }
            cdtIds.AddSubCondition("AND", "ZZSBID", "in", "EXPR:" + GetMachineNo());
            return QueryDataFromEasyUIDataGrid("C_JZZ_TMP", "", "", cdtIds, "*");
        }
        public ActionResult GetSelect()
        {
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            Condition cdtId;
            if (RoleLevel != 0)
            {
                string COMPANY_ID = Membership.CurrentUser.CompanyId.ToString();
                char[] c = COMPANY_ID.ToCharArray();
                string comId = "";
                bool temp = false;
                for (int i = c.Length - 1; i >= 0; i--)
                {
                    string cc = c[i].ToString();
                    if (cc != "0" && !temp)
                    {
                        temp = true;
                    }
                    if (temp)
                    {
                        comId += c[i];
                    }
                }
                char[] charArray = comId.ToCharArray();
                Array.Reverse(charArray);
                string comId3 = new String(charArray);
                comId3 += "%";
                cdtId = new Condition("AND", "DM", "like", comId3);

                Condition cdtId2 = new Condition();

                cdtId2.AddSubCondition("AND", "SSDW", "like", comId3);
                cdtId2.AddSubCondition("AND", "DELETED_MARK", "=", "0");
                List<UnCaseSenseHashTable> sb = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", cdtId2, "MACHINENO as DM", null, null, -1, -1);
                List<UnCaseSenseHashTable> dw = DbUtilityManager.Instance.DefaultDbUtility.Query("V_D_FW_COMP", cdtId, "DM,MC", null, null, -1, -1);
                return JsonDateObject(new { sb = sb, dw = dw });
            }
            else
            {
                Condition cdtId2 = new Condition();
                cdtId2.AddSubCondition("AND", "DELETED_MARK", "=", "0");
                List<UnCaseSenseHashTable> sb = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", null, "MACHINENO as DM", null, null, -1, -1);
                List<UnCaseSenseHashTable> dw = DbUtilityManager.Instance.DefaultDbUtility.Query("V_D_FW_COMP", null, "DM,MC", null, null, -1, -1);
                return JsonDateObject(new { sb = sb, dw = dw });
            }
        }



        #region __TIPS__:框架通用函数( 增 删 改)

        //public ActionResult GetDetail()
        //{
        //    string SLXLH = Request["SLXLH"];
        //    Hashtable hash = new Hashtable();
        //    if (SLXLH != null && SLXLH != "")
        //    {

        //        Condition cdtId = new Condition("AND", "SLXLH", "=", SLXLH);
        //        List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query("B_SFZ_ZZSL", cdtId, "*", null, null, -1, -1);

        //        string url = Server.MapPath("/Content/images/");
        //        string savepath = url + SLXLH + "_IMAGE.jpg";
        //        string savepath2 = url + SLXLH + "_XCZP.jpg";
        //        string savepath3 = url + SLXLH + "_ZWY_TXSJ.jpg";
        //        string savepath4 = url + SLXLH + "_ZWE_TXSJ.jpg";
        //        string imageUrl = "";
        //        string xczpUrl = "";
        //        string zwytpUrl = "";
        //        string zwetpUrl = "";
        //        if (System.IO.File.Exists(savepath))
        //        {
        //            imageUrl = "/Content/images/" + SLXLH + "_IMAGE.jpg";
        //            record[0]["IMAGE"] = imageUrl;
        //        }
        //        if (System.IO.File.Exists(savepath2))
        //        {
        //            xczpUrl = "/Content/images/" + SLXLH + "_XCZP.jpg";
        //            record[0]["XCZP"] = xczpUrl;
        //        }
        //        if (System.IO.File.Exists(savepath3))
        //        {
        //            zwytpUrl = "/Content/images/" + SLXLH + "_ZWY_TXSJ.jpg";
        //            record[0]["ZWY_TXSJ"] = zwytpUrl;
        //        }
        //        if (System.IO.File.Exists(savepath4))
        //        {
        //            zwetpUrl = "/Content/images/" + SLXLH + "_ZWE_TXSJ.png";
        //            record[0]["ZWE_ZCJG"] = zwetpUrl;
        //        }

        //        if (imageUrl == null || imageUrl == "")
        //        {
        //            byte[] b = (byte[])record[0]["IMAGE"];
        //            FileStream fs = new FileStream(savepath, FileMode.CreateNew);
        //            BinaryWriter bw = new BinaryWriter(fs);
        //            bw.Write(b, 0, b.Length);
        //            bw.Close();
        //            fs.Close();
        //            record[0]["IMAGE"] = "/Content/images/" + SLXLH + "_IMAGE.jpg";
        //        }

        //        if (xczpUrl == null || xczpUrl == "")
        //        {
        //            byte[] b = (byte[])record[0]["XCZP"];
        //            FileStream fs = new FileStream(savepath2, FileMode.CreateNew);
        //            BinaryWriter bw = new BinaryWriter(fs);
        //            bw.Write(b, 0, b.Length);
        //            bw.Close();
        //            fs.Close();
        //            record[0]["XCZP"] = "/Content/images/" + SLXLH + "_XCZP.jpg";
        //        }

        //        if (zwytpUrl == null || zwytpUrl == "")
        //        {
        //            byte[] b = (byte[])record[0]["ZWY_TXSJ"];
        //            FileStream fs = new FileStream(savepath3, FileMode.CreateNew);
        //            BinaryWriter bw = new BinaryWriter(fs);
        //            bw.Write(b, 0, b.Length);
        //            bw.Close();
        //            fs.Close();
        //            record[0]["ZWY_TXSJ"] = "/Content/images/" + SLXLH + "_ZWY_TXSJ.jpg";
        //        }

        //        if (zwetpUrl == null || zwetpUrl == "")
        //        {
        //            byte[] b = (byte[])record[0]["ZWE_TXSJ"];
        //            FileStream fs = new FileStream(savepath4, FileMode.CreateNew);
        //            BinaryWriter bw = new BinaryWriter(fs);
        //            bw.Write(b, 0, b.Length);
        //            bw.Close();
        //            fs.Close();
        //            record[0]["ZWE_TXSJ"] = "/Content/images/" + SLXLH + "_ZWE_TXSJ.jpg";
        //        }
        //        hash["rows"] = record;
        //        hash["temp"] = true;
        //        return JsonDateObject(hash);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


        #endregion

    }
}