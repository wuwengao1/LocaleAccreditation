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
        public ActionResult JsonConditionCombinationInfo()
        {
            return View();
        }

        public ActionResult JsonDataList()//业务主界面数据查询函数
        {
            //接收的基本查询参数有： id,limit,offset,search,sort,order            
            //__TIPS__*:根据表结构，修改以下函数的参数

            Condition cdtIds = new Condition();
            if (!Membership.CurrentUser.HaveAuthority("ZZJL.ZZJLCX.QUERY_ALL_ZZJL"))
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
                string comId2 = new String(charArray);
                comId2 += "%";
                cdtIds.AddSubCondition("AND", "ZZXXZZDW", "like", comId2);
            }
            return QueryDataFromEasyUIDataGrid("C_JZZ_TMP", "ZZXXZZRQ", "SLBH,ZZZXPH,GMSFHM,ZZSBID,ZZXXZZDW,ZZXXZZDWMC", cdtIds, "*");
        }
        public ActionResult GetSelect()
        {
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            Condition cdtId;
            if (RoleLevel != 0 && RoleLevel != 9)
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
    }
}