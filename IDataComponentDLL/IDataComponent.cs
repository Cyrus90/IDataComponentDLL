using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace IDataComponentDLL
{
    #region 提交成绩结构

    /// <summary>
    /// 步骤信息数组
    /// </summary>
    class SStepInformation
    {
        /// <summary>
        /// 步骤ID
        /// </summary>
        private string stepID;
        public string StepID
        {
            get { return stepID; }
            set { stepID = value; }
        }

        /// <summary>
        /// 步骤描述
        /// </summary>
        private string stepDesc;
        public string StepDesc
        {
            get { return stepDesc; }
            set { stepDesc = value; }
        }

        /// <summary>
        /// 步骤开始时间
        /// </summary>
        private string stepStartTime;
        public string StepStartTime
        {
            get { return stepStartTime; }
            set { stepStartTime = value; }
        }

        /// <summary>
        /// 步骤结束时间
        /// </summary>
        private string stepEndTime;
        public string StepEndTime
        {
            get { return stepEndTime; }
            set { stepEndTime = value; }
        }
    }

    class SItemInfomation
    {
        /// <summary>
        /// 评分时的时间
        /// </summary>
        private string itemTime;
        public string ItemTime
        {
            get { return itemTime; }
            set { itemTime = value; }
        }

        /// <summary>
        /// 评分结束的时间
        /// </summary>
        private string itemEndTime;
        public string ItemEndTime
        {
            get { return itemEndTime; }
            set { itemEndTime = value; }
        }

        /// <summary>
        /// 评分项编码
        /// </summary>
        private string itemNo;
        public string ItemNo
        {
            get { return itemNo; }
            set { itemNo = value; }
        }

        /// <summary>
        /// 此项评分项分值
        /// </summary>
        private string score;
        public string Score
        {
            get { return score; }
            set { score = value; }
        }
    }

    class SSubmitScore
    {
        /// <summary>
        /// 协议接口名
        /// </summary>
        private string command = "grade/submitscore";
        public string Command
        {
            get { return command; }
            set { command = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string operatorAccount;
        public string OperatorAccount
        {
            get { return operatorAccount; }
            set { operatorAccount = value; }
        }

        /// <summary>
        /// 用户ID
        /// </summary>
        private string operatorSessionid;
        public string OperatorSessionid
        {
            get { return operatorSessionid; }
            set { operatorSessionid = value; }
        }

        /// <summary>
        /// 评分者
        /// </summary>
        private string graderAccount;
        public string GraderAccount
        {
            get { return graderAccount; }
            set { graderAccount = value; }
        }

        /// <summary>
        /// 评分者Session
        /// </summary>
        private string graderddSessionid;
        public string GraderddSessionid
        {
            get { return graderddSessionid; }
            set { graderddSessionid = value; }
        }

        /// <summary>
        /// Task Log ID
        /// </summary>
        private string taskLogid;
        public string TaskLogid 
        {
            get { return taskLogid; }
            set { taskLogid = value; }
        }

        /// <summary>
        /// 操作开始时间
        /// </summary>
        private string startTime;
        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        /// <summary>
        /// 操作结束时间
        /// </summary>
        private string endTime;
        public string EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        /// <summary>
        /// 预约id
        /// </summary>
        private string applyid;
        public string ApplyId
        {
            get { return applyid; }
            set { applyid = value; }
        }

        /// <summary>
        /// 设备id
        /// </summary>
        private string deviceid;
        public string DeviceId
        {
            get { return deviceid; }
            set { deviceid = value; }
        }

        /// <summary>
        /// 评分表编码
        /// </summary>
        private string scoreSheetCode;
        public string ScoreSheetCode
        {
            get { return scoreSheetCode; }
            set { scoreSheetCode = value; }
        }

        /// <summary>
        /// 音频文件ID
        /// </summary>
        private string audioFileid;
        public string AudioFileid
        {
            get { return audioFileid; }
            set { audioFileid = value; }
        }

        /// <summary>
        /// 视频文件
        /// </summary>
        private string videoFileid;
        public string VideoFileid
        {
            get { return videoFileid; }
            set { videoFileid = value; }
        }

        /// <summary>
        /// 步骤信息数组
        /// </summary>
        private List<SStepInformation> stepInfoArray;
        public List<SStepInformation> StepInfoArray
        {
            get { return stepInfoArray; }
            set { stepInfoArray = value; }
        }

        /// <summary>
        /// 评分项信息数值
        /// </summary>
        private List<SItemInfomation> scoreArray;
        public List<SItemInfomation> ScoreArray
        {
            get { return scoreArray; }
            set { scoreArray = value; }
        }

        public SSubmitScore(string pOperatorAccount,string pTaskId)
        {
            Command = "grade/submitscore";
            OperatorAccount = pOperatorAccount;
            TaskLogid = pTaskId;
        }

    }
    #endregion

    /// <summary>
    /// 数据组件
    /// </summary>
    public class IDataComponent : MonoBehaviour
    {
        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        static extern long WritePrivateProfileString(string section, string key,
            string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);


        #endregion

        #region 与录制视频、音频、截图的交互数据
        const string configPath = "C:\\MISData\\Data\\channel.ini";
        const string timePath = "C:\\MISData\\Data\\time.ini";
        #region 读Ini文件

        /// <summary>
        /// 读取Ini文件
        /// </summary>
        /// <param name="Section">区域</param>
        /// <param name="Key">键</param>
        /// <param name="NoText"></param>
        /// <param name="iniFilePath"></param>
        /// <returns></returns>
        string ReadIniData(string Section, string Key, string NoText, string iniFilePath = configPath)
        {
            if (File.Exists(iniFilePath))
            {
                StringBuilder temp = new StringBuilder(1024);
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);
                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }

        #endregion

        #region 写Ini文件
        /// <summary>
        /// 写Ini文件值
        /// </summary>
        /// <param name="Section"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="iniFilePath"></param>
        /// <returns></returns>
        bool WriteIniData(string Section, string Key, string Value, string iniFilePath = configPath)
        {
            if (File.Exists(iniFilePath))
            {
                long OpStation = WritePrivateProfileString(Section, Key, Value, iniFilePath);
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        #endregion
        #endregion

        /// <summary>
        /// 标准评分码，读取本地数据存储
        /// </summary>
        static SSubmitScore standardScoreSheet;

        /// <summary>
        /// 存储一次操作的评分数据
        /// </summary>
        static SSubmitScore submitScore;

        /// <summary>
        /// 服务器地址
        /// </summary>
        static string toURL="";

        /// <summary>
        /// 提交成绩的返回码
        /// </summary>
        static string submitScoreErrCode = "";
        static string submitScoreErrDesc = "";

        /// <summary>
        /// 登录返回码
        /// </summary>
        static string loginErrCode = "";
        static string loginErrdesc = "";

        /// <summary>
        /// pc智能产品获取的token
        /// </summary>
        static string pcLoginToken = "";

        /// <summary>
        /// 登录时的用户ID
        /// </summary>
        static string userId = "";

        /// <summary>
        /// 账户中文名-web用
        /// </summary>
        static string userName = "";

        /// <summary>
        /// pc版中文名
        /// </summary>
        static string pcUserName = "";

        /// <summary>
        /// 用户登录（pc、web）时的账户
        /// </summary>
        string loginAccount = "";

        /// <summary>
        /// web的TaskID
        /// </summary>
        string taskid = "";

        bool isWebApplication = true;//是否是web

        WWW www;

        #region 创建单例

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            m_instance = this;
            submitScore = new SSubmitScore(loginAccount,taskid);
            submitScore.StepInfoArray = new List<SStepInformation>();
            submitScore.ScoreArray = new List<SItemInfomation>();
        }

        void Start()
        {
            gameObject.name = "IDataComponent";
            userName = "";
        }

        void Update()
        {
            if (isWebApplication==true)
            {
                //调用html里面的函数
                if (submitScore.OperatorAccount != null || submitScore.OperatorAccount == "")
                {
                    Application.ExternalCall("getAccountName");
                }

                if (userName == "")
                {
                    Application.ExternalCall("getAccountChinsesName");
                }

                if (submitScore.TaskLogid != null || submitScore.TaskLogid == "")
                {
                    Application.ExternalCall("getTaskLogID");
                }
            }
        }

        //控制本模块的变量
        private volatile static IDataComponent m_instance;

        //线程安全
        private static readonly object lockHelper = new object();

        private IDataComponent()
        {

        }


        /// <summary>
        /// 创建单例
        /// </summary>
        /// <returns></returns>
        public static IDataComponent GetInstance()
        {
            return m_instance;
        }
        #endregion

        /// <summary>
        /// 组提交成绩包
        /// </summary>
        /// <returns></returns>
        string getSubmitScorePacket()
        {
            JsonData data = new JsonData();
            data["1"] = new JsonData();
            data["1"]["command"] = submitScore.Command;
            data["1"]["operatoraccount"] = submitScore.OperatorAccount;
            data["1"]["operatorsessionid"] = submitScore.OperatorSessionid;
            data["1"]["graderaccount"] = submitScore.GraderAccount;
            data["1"]["graderddsessionid"] = submitScore.GraderddSessionid;
            data["1"]["starttime"] = submitScore.StartTime;
            UnityEngine.Debug.Log("TaskId = " + submitScore.TaskLogid);
            data["1"]["taskid"] = submitScore.TaskLogid;
            data["1"]["scoresheetcode"] = submitScore.ScoreSheetCode;

            submitScore.EndTime = DateTime.Now.ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
            
            data["1"]["endtime"] = submitScore.EndTime;

            data["1"]["applyid"] = submitScore.ApplyId;
            data["1"]["deviceid"] = submitScore.DeviceId;

            data["1"]["stepinfo"] = new JsonData();
            if (submitScore.StepInfoArray.Count == 0)
            {
                JsonData content = new JsonData();
                data["1"]["stepinfo"].Add(content);
            }
            else
            {
                foreach (SStepInformation si in submitScore.StepInfoArray)
                {
                    JsonData content = new JsonData();
                    content["stepid"] = si.StepID;
                    content["starttime"] = si.StepStartTime;
                    content["endtime"] = si.StepEndTime;
                    content["stepdesc"] = si.StepDesc;
                    data["1"]["stepinfo"].Add(content);
                }
            }
            

            data["1"]["score"] = new JsonData();
            if (submitScore.ScoreArray.Count==0)
            {
                JsonData content = new JsonData();
                data["1"]["score"].Add(content);
            }
            else
            {
                foreach (SItemInfomation si in submitScore.ScoreArray)
                {
                    JsonData content = new JsonData();

                    content["itemno"] = si.ItemNo;
                    content["score"] = si.Score;
                    content["itemtime"] = si.ItemTime;
                    data["1"]["score"].Add(content);
                }
            }
            return data.ToJson();
        }


        /// <summary>
        /// 组登录包
        /// </summary>
        /// <returns></returns>
        string getLoginPacket(string pAccountName,string pPassword)
        {
            JsonData data = new JsonData();
            data["1"] = new JsonData();
            data["1"]["command"] = "login";
            data["1"]["accountname"] = pAccountName;
            data["1"]["password"] = pPassword;
            return data.ToJson();
        }

        #region 发送数据包


        /// <summary>
        /// 发送成绩数据
        /// </summary>
        /// <param name="pOpenURL">默认为"",表示web版；否则为智能产品直接打开连接：例http:127.0.0.1:80</param>
        /// <returns></returns>
        IEnumerator sendScorePacket(string pOpenURL)
        {
            #region 发送、接受数据包
            //组JSon包
            string post_query = getSubmitScorePacket();
            UnityEngine.Debug.Log("发送的数据包=" + post_query);
            WWWForm form = new WWWForm();
            Dictionary<string, string> headers = form.headers;
            byte[] rawData = Encoding.UTF8.GetBytes(post_query);
            headers["Content-Type"] = "application/json";
            headers["Accept"] = "application/json";
            UnityEngine.Debug.Log("ToURL="+toURL);
            www = new WWW(toURL, rawData, headers);
            yield return www;

            if (www.error != null)
            {
                submitScoreErrDesc = www.error;
                UnityEngine.Debug.Log("error:" + www.error);
            }
            else
            {
                UnityEngine.Debug.Log("receive data is succeed: " + www.text);
                //解析数据
                string scoreid = "";

                JsonData data = JsonMapper.ToObject(www.text);
                string funName = data["1"]["command"].ToString();
                submitScoreErrCode = data["1"]["errcode"].ToString();
                submitScoreErrDesc = data["1"]["errdesc"].ToString();
                UnityEngine.Debug.Log("submitScoreErrDesc=" + submitScoreErrDesc);
                if (((IDictionary)data["1"]).Contains("scoreid"))
                    scoreid = data["1"]["scoreid"].ToString();
                UnityEngine.Debug.Log("submitScoreErrCode=" + submitScoreErrCode);
                if (submitScoreErrCode == "0")
                {
                    UnityEngine.Debug.Log("发送成功！");
                    if (pOpenURL!="")//智能产品
                    {
                        Application.OpenURL(pOpenURL + "/pc/train-record-detail.html?detail=" + scoreid+"&u="+userId+"&t="+pcLoginToken);
                        WriteIniData("Data", "Scoreid", scoreid);
                    }
                    else
                    {
                        notifyHTMLSendDataEnd(scoreid);
                    }
                       
                }
                else
                {
                    UnityEngine.Debug.Log("发送失败,Code=:" + submitScoreErrCode + ",Desc=" + submitScoreErrDesc);
                    if (pOpenURL != "")//智能产品
                    {
                        WriteIniData("Data", "Scoreid", "-1");
                    }
                    else
                    {
                        notifyHTMLSendDataFailed(submitScoreErrDesc);
                    }
                }
            }

            yield return 0;
            #endregion
        }

        IEnumerator sendLoginPacket(string pAccountName, string pPassword)
        {
            #region 发送、接受数据包
            //组JSon包
            string post_query = getLoginPacket(pAccountName,pPassword);
            UnityEngine.Debug.Log("发送的数据包=" + post_query);
            WWWForm form = new WWWForm();
            Dictionary<string, string> headers = form.headers;
            byte[] rawData = Encoding.UTF8.GetBytes(post_query);
            headers["Content-Type"] = "application/json";
            headers["Accept"] = "application/json";
            UnityEngine.Debug.Log("ToURL=" + toURL);

            www = new WWW(toURL, rawData, headers);
            yield return www;
            if (www.error != null)
            {
                loginErrdesc = www.error;
                UnityEngine.Debug.Log("error:" + www.error);
            }
            else
            {
                UnityEngine.Debug.Log("receive data is succeed: " + www.text);
                //解析数据
                JsonData data = JsonMapper.ToObject(www.text);
                string funName = data["1"]["command"].ToString();
                loginErrCode = data["1"]["errcode"].ToString();
                loginErrdesc = data["1"]["errdesc"].ToString();
                if (((IDictionary)data["1"]).Contains("sessionid"))
                {
                    pcLoginToken = data["1"]["sessionid"].ToString();
                }
                if (((IDictionary)data["1"]).Contains("user_id"))
                {
                    userId = data["1"]["user_id"].ToString();
                }
                if (((IDictionary)data["1"]).Contains("name"))
                {
                    pcUserName = data["1"]["name"].ToString();
                    UnityEngine.Debug.Log("pcUserName="+pcUserName);
                }

                if (loginErrCode == "0")
                {
                    UnityEngine.Debug.Log("发送成功！");
                    UnityEngine.Debug.Log("Token="+pcLoginToken);
                    submitScore.OperatorAccount = pAccountName;
                }
                else
                {
                    UnityEngine.Debug.Log("发送失败,Code=:" + loginErrCode + ",Desc=" + loginErrdesc);
                }
            }

            yield return 0;
            #endregion
        }
        #endregion

        /// <summary>
        /// 通知网页已发送数据
        /// </summary>
        /// <returns></returns>
        void notifyHTMLSendDataEnd(string id)
        {
            Application.ExternalCall("operatorEnd", "已提交成绩，操作结束！");
            Application.ExternalCall("goScoreSheet", id);
        }

        /// <summary>
        /// 通知WEB发送数据失败
        /// </summary>
        /// <param name="desc"></param>
        void notifyHTMLSendDataFailed(string desc)
        {
            Application.ExternalCall("sendDataFailed", "数据发送失败，描述为:"+desc);
        }

        /// <summary>
        /// 从WEb获取账户名
        /// </summary>
        /// <param name="pName"></param>
        void getAccountNameFromHTML(string pName)
        {
            UnityEngine.Debug.Log("get account name ="+pName);
            submitScore.OperatorAccount = pName;
        }

        /// <summary>
        /// 从Web获取账户名对应的中文名
        /// </summary>
        /// <param name="pName"></param>
        void getAccountChineseNameFromHTML(string pName)
        {
            UnityEngine.Debug.Log("get account chinses name =" + pName);
            userName = pName;
        }

        /// <summary>
        /// 从WEB获取TaskID-不可调用
        /// </summary>
        /// <param name="pId"></param>
        void getTaskLogIDFromHTML(string pId)
        {
            UnityEngine.Debug.Log("get task log id =" + pId);
            submitScore.TaskLogid = pId;
        }

        ////////////////////////////////////////////////现使用 通信接口///////////////////////////////////////////////////

        #region 组数据包

        /// <summary>
        /// 增加用户账户--从web获取，不需要使用了
        /// </summary>
        /// <param name="pOperatorAccount"></param>
        void addOperatorAccount(string pOperatorAccount)
        {
            submitScore.OperatorAccount = pOperatorAccount;
        }

        /// <summary>
        /// 增加操作者sessionID--暂不用
        /// </summary>
        /// <param name="pOperatorSessionId"></param>
        void addOperatorSessionid(string pOperatorSessionId)
        {
            submitScore.OperatorSessionid = pOperatorSessionId;
        }

        /// <summary>
        /// 增加评分者账户--暂不用
        /// </summary>
        /// <param name="pGraderAccount"></param>
        void addGraderAccount(string pGraderAccount)
        {
            submitScore.GraderAccount = pGraderAccount;
        }

        /// <summary>
        /// 增加评分账户session ID--暂不用
        /// </summary>
        /// <param name="pGraderSessionId"></param>
        void addGraderSessionid(string pGraderSessionId)
        {
            submitScore.GraderddSessionid = pGraderSessionId;
        }

        /// <summary>
        /// 增加预约ID
        /// </summary>
        /// <param name="pApplyID"></param>
        void addApplyId(string pApplyID)
        {
            submitScore.ApplyId = pApplyID;
        }

        /// <summary>
        /// 增加设备ID
        /// </summary>
        /// <param name="pDeviceID"></param>
        void addDeviceID(string pDeviceID)
        {
            submitScore.DeviceId = pDeviceID;
        }

        /// <summary>
        /// 增加开始操作时间
        /// </summary>
        public void addStartTime()
        {
            submitScore.StartTime = DateTime.Now.ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
        }

        /// <summary>
        /// 增加结束操作时间--提交时自动添加
        /// </summary>
        void addEndTime()
        {
            submitScore.EndTime = DateTime.Now.ToString("%yyyy-%MM-%dd %HH:%mm:%ss"); ;
        }

        /// <summary>
        /// 增加评分表编码--表示用的是哪套评分表
        /// </summary>
        /// <param name="pScoreSheetCode"></param>
        public void addScoreSheetCode(string pScoreSheetCode)
        {
            submitScore.ScoreSheetCode = pScoreSheetCode;
        }

        /// <summary>
        /// 增加评分数据,注意，目前要求在程序一运行就调用开始录屏，结束时调用停止
        /// </summary>
        /// <param name="pCode"></param>
        /// <param name="pScore"></param>
        /// <param name="endDetailTime">评分开始后延迟几秒作为评分结束时间</param>
        /// <param name="isWeb">此参数严格区分是否是在线系统还是智能系统</param>
        public void addScoreItem(string pCode, float pScore,float endDetailTime = 5.0f,bool isWeb = true)
        {
            SItemInfomation sf = new SItemInfomation();
            sf.ItemNo = pCode;
            sf.Score = pScore.ToString();
            sf.ItemTime = DateTime.Now.ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
            sf.ItemEndTime = DateTime.Now.AddSeconds(endDetailTime).ToString("%yyyy-%MM-%dd %HH:%mm:%ss");

            if(isWeb==false)
                sf.ItemTime = ReadIniData("Data","Time","",timePath);
            foreach (SItemInfomation si in submitScore.ScoreArray)
            {
                if (si.ItemNo == pCode)
                {
                    return;
                }
            }
            submitScore.ScoreArray.Add(sf);
        }

        /// <summary>
        /// 创建步骤信息--v1.0版本，建议使用createStepAndTime替换
        /// </summary>
        /// <param name="pStepDesc">当前步骤的描述</param>
        /// <returns>返回创建的步骤信息ID</returns>
        public int createStepInfo(string pStepDesc="")
        {
            SStepInformation si = new SStepInformation();
            si.StepStartTime = si.StepEndTime = "";
            si.StepID = submitScore.StepInfoArray.Count.ToString();
            si.StepDesc = pStepDesc;
            submitScore.StepInfoArray.Add(si);
            return submitScore.StepInfoArray.Count-1;
        }

        /// <summary>
        /// 创建步骤信息,自动添加步骤开始时间--v1.1建议使用
        /// </summary>
        /// <param name="pStepDesc">当前步骤的描述</param>
        /// <returns>返回创建的步骤信息ID</returns>
        public int createStepAndTime(string pStepDesc = "")
        {
            SStepInformation si = new SStepInformation();
            si.StepID = submitScore.StepInfoArray.Count.ToString();
            si.StepDesc = pStepDesc;
            si.StepStartTime = DateTime.Now.ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
            si.StepEndTime = "";
            submitScore.StepInfoArray.Add(si);
            return submitScore.StepInfoArray.Count - 1;
        }

        /// <summary>
        /// 为创建的步骤添加开始时间
        /// </summary>
        /// <param name="pStepId">设置为您创建的步骤ID</param>
        public void setStepStartTime(int pStepId)
        {
            if(pStepId<0 ||pStepId >= submitScore.StepInfoArray.Count)
            {
                UnityEngine.Debug.LogError("参数pStepId不能得到正确的索引。");
                return;
            }
            submitScore.StepInfoArray[pStepId].StepStartTime = DateTime.Now.ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
        }

        /// <summary>
        /// 为创建的步骤添加结束时间
        /// </summary>
        /// <param name="pStepId">设置为您创建的步骤ID</param>
        public void setStepEndTime(int pStepId)
        {
            if (pStepId < 0 || pStepId >= submitScore.StepInfoArray.Count)
            {
                UnityEngine.Debug.LogError("参数pStepId不能得到正确的索引。");
                return;
            }
            submitScore.StepInfoArray[pStepId].StepEndTime = DateTime.Now.ToString("%yyyy-%MM-%dd %HH:%mm:%ss");
        }

        #endregion

        /// <summary>
        /// 从本地或者服务器加载评分表简化版
        /// </summary>
        void getScoreSheet()
        {
            standardScoreSheet = new SSubmitScore(loginAccount,taskid);
            standardScoreSheet.ScoreArray = new List<SItemInfomation>();
            var fileAddress = System.IO.Path.Combine(Application.streamingAssetsPath, "scoreSheet.txt");

            FileInfo fInfo0 = new FileInfo(fileAddress);
            string s = "";
            if (fInfo0.Exists)
            {
                StreamReader r = new StreamReader(fileAddress);
                s = r.ReadToEnd();
                JsonData data = JsonMapper.ToObject(s);
                standardScoreSheet.ScoreSheetCode = data["scoresheetcode"].ToString();

                JsonData item = data["scoreItem"];
                for (int i = 0; i < item.Count; i++)
                {
                    SItemInfomation sf = new SItemInfomation();
                    sf.ItemNo = item[i]["itemcode"].ToString();
                    sf.Score = item[i]["itemScore"].ToString();
                    standardScoreSheet.ScoreArray.Add(sf);
                }
            }
        }

        /// <summary>
        /// 当开始新一轮计分时，复原成绩数据。
        /// </summary>
        public void resetScoreData()
        {
            submitScore.ScoreArray = null;
            //submitScore = null;
            //submitScore = new SSubmitScore();
            submitScore.ScoreArray = new List<SItemInfomation>();
            submitScore.StepInfoArray = null;
            submitScore.StepInfoArray = new List<SStepInformation>();
            submitScoreErrCode = "";
            submitScoreErrDesc = "";
          
            //loginErrCode = "";
            //loginErrdesc = "";
        }

        /// <summary>
        /// 从已经记录的评分数据里移除某项操作
        /// </summary>
        /// <param name="pCode"></param>
        public void removeScoreItem(string pCode)
        {
            for(int i = 0;i<submitScore.ScoreArray.Count;i++)
            {
                if(submitScore.ScoreArray[i].ItemNo == pCode)
                {
                    submitScore.ScoreArray.Remove(submitScore.ScoreArray[i]);
                }
            }
        }

        /// <summary>
        /// 设置推送数据的服务器地址
        /// </summary>
        /// <param name="pURL">例如：http://192.168.1.165:8086</param>
        public void setURL(string pURL)
        {
            toURL = pURL;
        }

        /// <summary>
        /// 检测编码是否已经记录
        /// </summary>
        /// <param name="pCode"></param>
        /// <returns></returns>
        public bool isExistCode(string pCode)
        {
            if (submitScore==null)
            {
                return false;
            }
            foreach (SItemInfomation si in submitScore.ScoreArray)
            {
                if (si.ItemNo == pCode)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 推送成绩数据--web和智能化产品
        /// </summary>
        /// <param name="pOpenURL">例：http://192.168.1.195；智能设备：如果此参数不为空，发送成功后将打开连接；WEb：参数为空</param>
        public void sendScoreData(string pOpenURL="")
        {
           StartCoroutine(sendScorePacket(pOpenURL));
        }
        
        /// <summary>
        /// 发送登录数据--智能化产品
        /// </summary>
        /// <param name="pAccoutName"></param>
        /// <param name="pPassword"></param>
        public void sendLoginData(string pAccoutName,string pPassword)
        {
           StartCoroutine(sendLoginPacket(pAccoutName, pPassword));
        }

        /// <summary>
        /// 推送成绩后，可在Update里调用此接口检测是否发送成功。
        /// </summary>
        /// <returns>成功为true,失败为false</returns>
        public string checkSendScoreDataIsSucceed()
        {
            return submitScoreErrDesc;
        }

        /// <summary>
        /// 发送登录请求后，可在update里检测是否登录成功。
        /// </summary>
        /// <returns></returns>
        public string checkLoginIsSucceed()
        {
            if (loginErrCode == "0")
            {
                return "succeed";
            }
            else
                return loginErrdesc;
        }
        
        /// <summary>
        /// 从web端获取训练者中文名
        /// </summary>
        /// <returns></returns>
        public string getWebAccountChineseName()
        {
            return userName;
        }

        /// <summary>
        /// 获取登录后的用户名-智能设备
        /// </summary>
        /// <returns></returns>
        public string getPcAccountChineseName()
        {
            return pcUserName;
        }
        
        /// <summary>
        /// 通知组件当前运行平台是web还是pc端智能产品（web端时会从web获取账户数据，pc端则不会）
        /// </summary>
        /// <param name="isWebPlatform">true,当前是web端;false,pc端智能产品</param>
        public void notifyComponentPlatform(bool isWebPlatform = true)
        {
            isWebApplication = isWebPlatform;
        }

        /////////////////////////////////////////////视频、音频、图片等交互接口//////////////////////////////
        /// <summary>
        /// 开始录制视频--智能化产品
        /// </summary>
        public bool startRecordVideo()
        {
            WriteIniData("Data", "ProcessName", Process.GetCurrentProcess().ProcessName);
            return WriteIniData("Data", "State", "1");
        }

        /// <summary>
        /// 结束录制视频--智能化产品
        /// </summary>
        public bool finishRecordVideo()
        {
            return WriteIniData("Data", "State", "0");
        }

        /// <summary>
        /// 截图-全屏--智能化产品,只能在录制视频的时候截图
        /// </summary>
        /// <param name="pCode">发生错误时截图，传入错误编码</param>
        /// <returns>成功返回true</returns>
        public bool printScreen(string pCode)
        {
            if (ReadIniData("Data", "State", "") == "1")
                return WriteIniData("Data", "PrtScn", pCode);
            else
                return false;
        }

        ////////////////////////////////////////////////与HTML进行交互///////////////////////////////////////
        /// <summary>
        /// 通知网页操作已结束,将提示训练已结束。
        /// </summary>
        /// <returns></returns>
        void notifyHTMLFinished()
        {
            Application.ExternalCall("operatorEnd", "训练已结束！");
        }
    }
}
