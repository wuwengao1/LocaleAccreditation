using MisFrameWork.core;
using MisFrameWork.core.db;
using MisFrameWork3.Classes.Controller;
using MisFrameWork3.Classes.Membership;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace MisFrameWork3.Areas.ZZJLCX.Controllers
{
    public class DWZZJLController : FWBaseController
    {
        // GET: ZZJLCX/SBZZJL
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult JsonConditionCombinationInfo()
        {
            return View();
        }
        #region __TIPS__:单位制证数量统计
        public ActionResult AcceptStat3()
        {
            int RoleLevel = Membership.CurrentUser.RoleLevel;
            string comId2 = "";
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
                comId2 = new String(charArray);
                comId2 += "%";
            }
            string search = Request["search"];
            string query_sql;
            if (string.IsNullOrEmpty(comId2))
            {

                if (string.IsNullOrEmpty(search))
                {
                    query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP  group by ZZXXZZDW,ZZXXZZDWMC ";
                }
                else
                {
                    query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP where ZZXXZZDW like  '%" + search + "%' and ( ZZXXZZDWMC like  '%" + search + "%')  group by ZZXXZZDW,ZZXXZZDWMC ";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP where ZZXXZZDW like '" + comId2 + "' group by ZZXXZZDW,ZZXXZZDWMC ";
                }
                else
                {
                    query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP where ZZXXZZDW like '" + comId2 + "' and ( ZZXXZZDW like  '%" + search + "%' OR ZZXXZZDWMC like  '%" + search + "%')  group by ZZXXZZDW,ZZXXZZDWMC ";
                }
            }

            List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(query_sql, -1, -1);
            return JsonDateObject(record);
        }

        public ActionResult AcceptStat2()
        {

            string SSDW = Request["SSDW"];
            string ZZY = Request["ZZY"];
            string ACCEPT_DATE = Request["ACCEPT_DATE"];
            string ACCEPT_DATE2 = Request["ACCEPT_DATE2"];
            string zzys = "";
            //根据单位先获取单位下的制证员，然后根据制证员id查询与制证员关联的设备编号，最后根据设备编号查询制证记录
            if (!string.IsNullOrEmpty(SSDW))
            {
                char[] c = SSDW.ToCharArray();
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

                Condition cdtId3 = new Condition();
                cdtId3.AddSubCondition("AND", "COMPANY_ID", "like", comId2);
                cdtId3.AddSubCondition("AND", "ROLES_ID_V_D_FW_S_ROLES__MC", "=", "制证员");
                List<UnCaseSenseHashTable> re = DbUtilityManager.Instance.DefaultDbUtility.Query("FW_S_USERS", cdtId3, "USER_ID", null, null, -1, -1);
                if (re.Count != 0)
                {

                    string userId = "(";
                    for (int i = 0; i < re.Count; i++)
                    {
                        userId += "'" + re[i]["USER_ID"] + "',";
                    }
                    userId += "'0')";

                    Condition cdtId = new Condition();
                    cdtId.AddSubCondition("AND", "SBFZR", "in", "EXPR:" + userId);
                    List<UnCaseSenseHashTable> rec = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", cdtId, "MACHINENO", null, null, -1, -1);
                    if (rec.Count != 0)
                    {
                        zzys = "(";
                        for (int i = 0; i < re.Count; i++)
                        {
                            zzys += "'" + rec[i]["MACHINENO"] + "',";
                        }
                        zzys += "'0')";
                    }
                    else
                    {
                        zzys = "('0')";
                    }
                }
                else
                {
                    zzys = "('0')";
                }
            }
            string sbbh = "";
            if (!string.IsNullOrEmpty(ZZY))
            {
                Condition cdtId = new Condition();
                cdtId.AddSubCondition("AND", "SBFZR", "=", ZZY);
                List<UnCaseSenseHashTable> rec = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", cdtId, "MACHINENO", null, null, -1, -1);
                if (rec.Count != 0)
                {
                    sbbh = rec[0]["MACHINENO"].ToString();
                }
            }

            string query_sql;
            if (string.IsNullOrEmpty(zzys) && string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(sbbh))
            {
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID = '" + sbbh + " group by ZZSBID ";
            }
            else if (string.IsNullOrEmpty(zzys) && string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(sbbh))
            {
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " group by ZZSBID ";
            }

            else if (!string.IsNullOrEmpty(zzys) && string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(sbbh))
            {
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID in " + zzys + " and ZZSBID = '" + sbbh + "' group by ZZSBID ";
            }
            else if (!string.IsNullOrEmpty(zzys) && string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(sbbh))
            {
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID in " + zzys + " group by ZZSBID ";
            }

            else if (string.IsNullOrEmpty(zzys) && !string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID = '" + sbbh + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (string.IsNullOrEmpty(zzys) && !string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }


            else if (!string.IsNullOrEmpty(zzys) && !string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + "  and ZZSBID in " + zzys + " and ZZSBID = '" + sbbh + "'and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (!string.IsNullOrEmpty(zzys) && !string.IsNullOrEmpty(ACCEPT_DATE) && string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + "  and ZZSBID in " + zzys + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }

            else if (string.IsNullOrEmpty(zzys) && string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID = '" + sbbh + " and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (string.IsNullOrEmpty(zzys) && string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }


            else if (!string.IsNullOrEmpty(zzys) && string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID in " + zzys + " and ZZSBID = '" + sbbh + "' and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (!string.IsNullOrEmpty(zzys) && string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID in " + zzys + " and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }



            else if (string.IsNullOrEmpty(zzys) && !string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && !string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID = '" + sbbh + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else if (string.IsNullOrEmpty(zzys) && !string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }

            else if (!string.IsNullOrEmpty(zzys) && !string.IsNullOrEmpty(ACCEPT_DATE) && !string.IsNullOrEmpty(ACCEPT_DATE2) && string.IsNullOrEmpty(sbbh))
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID in " + zzys + " and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }
            else
            {
                ACCEPT_DATE = ACCEPT_DATE + " 00:00:00";
                ACCEPT_DATE2 = ACCEPT_DATE2 + " 23:59:59";
                query_sql = "select ZZSBID,count(*) as count from C_JZZ_TMP where ZZSBID in " + GetMachineNo() + " and ZZSBID in " + zzys + " and ZZSBID = '" + sbbh + "' and ZZXXZZRQ >= TO_DATE('" + ACCEPT_DATE + "', 'YYYY-MM-DD HH24:MI:SS') and ZZXXZZRQ <= TO_DATE('" + ACCEPT_DATE2 + "', 'YYYY-MM-DD HH24:MI:SS') group by ZZSBID ";
            }

            List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(query_sql, -1, -1);

            Hashtable hash = new Hashtable();
            List<UnCaseSenseHashTable> rows = new List<UnCaseSenseHashTable>();
            if (record.Count != 0)
            {
                foreach (UnCaseSenseHashTable r in record)
                {
                    Condition cdtId = new Condition();
                    cdtId.AddSubCondition("AND", "MACHINENO", "=", r["ZZSBID"]);
                    List<UnCaseSenseHashTable> rec = DbUtilityManager.Instance.DefaultDbUtility.Query("B_MACHINE", cdtId, "SBFZR", null, null, -1, -1);

                    UnCaseSenseHashTable info = new UnCaseSenseHashTable();

                    info["SBFZR"] = rec[0]["SBFZR"];
                    info["COUNT"] = r["count"];
                    if (rec[0]["SBFZR"] != null)
                    {
                        rows.Add(info);
                    }
                }
            }

            hash["rows"] = rows;
            return JsonDateObject(hash);
        }


        public ActionResult AcceptStat()
        {
            int RoleLevel = Membership.CurrentUser.RoleLevel;

            if (Membership.CurrentUser.HaveAuthority("ZZJL.ZZJLCX.QUERY_ALL_ZZJL"))
            {
                string sql = "";
                if (Request["cdt_combination"] != null)
                {
                    sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP ";
                    string jsoncdtCombination = System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(Request["cdt_combination"]));
                    Condition cdtCombination = Condition.LoadFromJson(jsoncdtCombination);
                    cdtCombination.Relate = "AND";
                    ReplaceCdtCombinationOpreate(cdtCombination);
                    int count = cdtCombination.SubConditions.Count;
                    if (count != 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Condition c = cdtCombination.SubConditions[i];
                            if (c.Src == "ZZXXZZRQ")
                            {
                                sql += " " + c.Relate + " " + c.Src + " " + c.Op + "  TO_DATE('" + c.Tag + "', 'YYYY-MM-DD HH24:MI:SS') ";
                            }
                            else
                            {
                                sql += " " + c.Relate + " " + c.Src + " " + c.Op + " '" + c.Tag + "'";
                            }
                        }
                    }
                    sql += "  group by ZZXXZZDW,ZZXXZZDWMC";
                }
                if (!string.IsNullOrEmpty(sql))
                {
                    List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(sql, -1, -1);
                    return JsonDateObject(record);
                }
                else
                {
                    string query_sql;
                    string search = Request["search"];

                    if (string.IsNullOrEmpty(search))
                    {
                        query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP ";
                    }
                    else
                    {
                        query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP where 1=1  and ( ZZXXZZDW like  '%" + search + "%' OR ZZXXZZDWMC like  '%" + search + "%') ";
                    }
                    if (Request["date_range_type"] != null && (Request["start_date"] != null || Request["end_date"] != null))
                    {
                        string fieldName = null;
                        int dataRangeTypeIndex = 0;
                        try
                        {
                            dataRangeTypeIndex = int.Parse(Request["date_range_type"]);
                        }
                        catch (Exception e)
                        {
                            dataRangeTypeIndex = 0;
                        }
                        string[] arrDataRangeFields = new string[] { "制证时间" };
                        if (dataRangeTypeIndex != 0)
                        {
                            fieldName = arrDataRangeFields[dataRangeTypeIndex - 1];
                            if (!String.IsNullOrEmpty(Request["start_date"]))
                            {
                                query_sql += " AND ZZXXZZRQ >=  TO_DATE('" + Request["start_date"].ToString() + "', 'YYYY-MM-DD HH24:MI:SS') ";
                            }
                            if (!String.IsNullOrEmpty(Request["end_date"]))
                            {
                                DateTime dtEndDate = DateTime.Parse(Request["end_date"]);
                                dtEndDate = dtEndDate.AddDays(1);//加多一天

                                query_sql += " AND ZZXXZZRQ <=  TO_DATE('" + dtEndDate.ToString() + "', 'YYYY-MM-DD HH24:MI:SS') ";
                            }
                        }
                    }
                    query_sql += " group by ZZXXZZDW,ZZXXZZDWMC";
                    List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(query_sql, -1, -1);
                    return JsonDateObject(record);
                }
            }
            else
            {
                string comId2 = "";
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
                comId2 = new String(charArray);
                comId2 += "%";

                string sql = "";
                if (Request["cdt_combination"] != null)
                {
                    sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP where ZZXXZZDW like '" + comId2;
                    string jsoncdtCombination = System.Text.ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(Request["cdt_combination"]));
                    Condition cdtCombination = Condition.LoadFromJson(jsoncdtCombination);
                    cdtCombination.Relate = "AND";
                    ReplaceCdtCombinationOpreate(cdtCombination);
                    int count = cdtCombination.SubConditions.Count;
                    if (count != 0)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            Condition cc = cdtCombination.SubConditions[i];
                            if (cc.Src == "ZZXXZZRQ")
                            {
                                sql += " " + cc.Relate + " " + cc.Src + " " + cc.Op + "  TO_DATE('" + cc.Tag + "', 'YYYY-MM-DD HH24:MI:SS') ";
                            }
                            else
                            {
                                sql += " " + cc.Relate + " " + cc.Src + " " + cc.Op + " '" + cc.Tag + "'";
                            }
                        }
                    }
                    sql += "  group by ZZXXZZDW,ZZXXZZDWMC";
                }
                if (!string.IsNullOrEmpty(sql))
                {
                    List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(sql, -1, -1);
                    return JsonDateObject(record);
                }
                else
                {
                    string query_sql;
                    string search = Request["search"];

                    if (string.IsNullOrEmpty(search))
                    {
                        query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP where ZZXXZZDW like '" + comId2;
                    }
                    else
                    {
                        query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP where ZZXXZZDW like '" + comId2 + " and (ZZXXZZDW like  '%" + search + "%' or ZZXXZZDWMC like  '%" + search + "%') ";
                    }
                    if (Request["date_range_type"] != null && (Request["start_date"] != null || Request["end_date"] != null))
                    {
                        string fieldName = null;
                        int dataRangeTypeIndex = 0;
                        try
                        {
                            dataRangeTypeIndex = int.Parse(Request["date_range_type"]);
                        }
                        catch (Exception e)
                        {
                            dataRangeTypeIndex = 0;
                        }
                        string[] arrDataRangeFields = new string[] { "制证时间" };
                        if (dataRangeTypeIndex != 0)
                        {
                            fieldName = arrDataRangeFields[dataRangeTypeIndex - 1];
                            if (!String.IsNullOrEmpty(Request["start_date"]))
                            {
                                query_sql += " AND ZZXXZZRQ >=  TO_DATE('" + Request["start_date"].ToString() + "', 'YYYY-MM-DD HH24:MI:SS') ";
                            }
                            if (!String.IsNullOrEmpty(Request["end_date"]))
                            {
                                DateTime dtEndDate = DateTime.Parse(Request["end_date"]);
                                dtEndDate = dtEndDate.AddDays(1);//加多一天

                                query_sql += " AND ZZXXZZRQ <=  TO_DATE('" + dtEndDate.ToString() + "', 'YYYY-MM-DD HH24:MI:SS') ";
                            }
                        }
                    }
                    query_sql += " group by ZZXXZZDW,ZZXXZZDWMC";
                    List<UnCaseSenseHashTable> record = DbUtilityManager.Instance.DefaultDbUtility.Query(query_sql, -1, -1);
                    return JsonDateObject(record);
                }
            }
        }
        #endregion


        #region 打印数据
        public FileResult ActionPrint(string name, string oject_id)
        {
            //获取数据
            Condition cdtIds = new Condition();
            string search = Request["Search"];
            string date_range_type = Request["date_range_type"];
            string start_date = Request["start_date"];
            string end_date = Request["end_date"];
            Condition cdtIds2 = new Condition();
            string query_sql;
            if (!string.IsNullOrEmpty(search))
            {
                query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP where 1=1 and (ZZXXZZDW like  '%" + search + "%' or ZZXXZZDWMC like  '%" + search + "%') ";
            }
            else
            {
                query_sql = "select ZZXXZZDW,ZZXXZZDWMC,count(*) as count from C_JZZ_TMP where 1=1 ";
            }
            
            if (!string.IsNullOrEmpty(date_range_type) && date_range_type != "0" && (!string.IsNullOrEmpty(start_date) || !string.IsNullOrEmpty(end_date)))
            {
                if (!String.IsNullOrEmpty(start_date))
                {
                    query_sql += " AND ZZXXZZRQ >=  TO_DATE('" + start_date + "', 'YYYY-MM-DD HH24:MI:SS') ";
                }
                if (!String.IsNullOrEmpty(end_date))
                {
                    DateTime dtEndDate = DateTime.Parse(end_date);
                    dtEndDate = dtEndDate.AddDays(1);//加多一天

                    query_sql += " AND ZZXXZZRQ <=  TO_DATE('" + dtEndDate.ToString() + "', 'YYYY-MM-DD HH24:MI:SS') ";
                }
            }

            if (!Membership.CurrentUser.HaveAuthority("SYS.USER.QUERY_ALL_USER"))
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
                query_sql += " ZZXXZZDW like '" + comId3;
             }
            query_sql += " group by ZZXXZZDW,ZZXXZZDWMC";
            List<UnCaseSenseHashTable> records = DbUtilityManager.Instance.DefaultDbUtility.Query(query_sql, -1, -1);

            //设置打印图纸大小
            Document document = new Document(PageSize.A4);
            //设置页边距
            document.SetMargins(36, 36, 36, 60);
            //中文字体
            string chinese = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "SIMSUN.TTC,1");
            BaseFont baseFont = BaseFont.CreateFont(chinese, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            //文字大小12，文字样式
            Font cn = new Font(baseFont, 14, Font.NORMAL);

            //PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(@"D:\temp.pdf", FileMode.Create));

            //这样写：是生成文件到内存中去
            var memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);//生成到内存中
            //writer.PageEvent = new PdfPageHelper();//页脚
            document.Open();//打开文件


            //Paragraph title = new Paragraph("国家工作人员登记备案表", new Font(baseFont, 23, Font.BOLD, BaseColor.BLACK));
            Paragraph title = new Paragraph("", new Font(baseFont, 23, Font.BOLD, BaseColor.BLACK));
            title.Alignment = Element.ALIGN_CENTER; //居中
            title.SpacingAfter = 20;
            document.Add(title);

            //数据表格
            PdfPTable table = new PdfPTable(4);
            table.SetWidths(new float[] { 2.5F, 8, 8 ,8});
            table.WidthPercentage = 100;
            AddBodyContentCell(table, "序号", cn);
            AddBodyContentCell(table, "制证单位编号", cn);
            AddBodyContentCell(table, "制证单位名称", cn);
            AddBodyContentCell(table, "数量", cn);

            for (int i = 0; i < records.Count; i++)
            {
                UnCaseSenseHashTable record = records[i];
                AddBodyContentCell(table, Convert.ToString(i + 1), cn);
                if (!string.IsNullOrEmpty((string)record["ZZXXZZDW"]))
                {
                    AddBodyContentCell(table, record["ZZXXZZDW"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }

                if (!string.IsNullOrEmpty((string)record["ZZXXZZDWMC"]))
                {
                    AddBodyContentCell(table, record["ZZXXZZDWMC"].ToString(), cn);
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
                if (record["COUNT"] != null)
                {
                    if (!string.IsNullOrEmpty(record["COUNT"].ToString()))
                    {
                        AddBodyContentCell(table, record["COUNT"].ToString(), cn);
                    }
                    else
                    {
                        AddBodyContentCell(table, "", cn);
                    }
                }
                else
                {
                    AddBodyContentCell(table, "", cn);
                }
            }
            document.Add(table);

            document.Close();

            var bytes = memoryStream.ToArray();
            //result = Convert.ToBase64String(bytes);

            return File(bytes, "application/pdf");
        }

        private void AddBodyContentCell(PdfPTable bodyTable, String text, iTextSharp.text.Font font, int rowspan = 2, bool needRightBorder = false)
        {
            PdfPCell cell = new PdfPCell();
            //float defaultBorder = 0.5f;
            //cell.BorderWidthLeft = defaultBorder;
            //cell.BorderWidthTop = 0;
            //cell.BorderWidthRight = needRightBorder ? defaultBorder : 0;
            //cell.BorderWidthBottom = defaultBorder;
            cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
            cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_BASELINE;
            //cell.Rowspan = rowspan;
            cell.PaddingBottom = 3;
            cell.Phrase = new Phrase(text, font);
            bodyTable.AddCell(cell);
        }
        #endregion
    }
}