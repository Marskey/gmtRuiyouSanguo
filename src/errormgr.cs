using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gmt
{
    public enum EErrType
    {
        ERR_SUCCESS = 0,
        ERR_FAILD = 1,
        ERR_UPDATE_ROLLING_NOTICE_FAILD = 2,
        ERR_ACTIVITY_REWARD_ID_MAX = 3,
        ERR_ACTIVITY_ID_MAX = 4,
        ERR_ACTIVITY_ACHIEVE_ID_MAX = 5,
        ERR_ACTIVITY_ACHIEVE_ZERO = 6,
        ERR_ACTIVITY_REWARD_COUNT = 7,
        ERR_ACTIVITY_REWARD_ITEM_COUNT = 8,
        ERR_ACTIVITY_ACHIEVE_REQUEST_COUNT = 9,
        ERR_TABLE_DATA_SAVE_FAILED = 10,
        ERR_LOGIN_FAILED = 11,
        ERR_LOGIN_ACC_PWD_NOT_FOUND = 12,
    }


    public static class CErrMgr
    {
        private static string[] ErrMsgs = {
            "成功"
            , "失败"
            , "更新跑马灯失败"
            , "奖励id达到上限, 添加失败"
            , "活动id达到上限, 添加失败"
            , "成就id达到上限, 添加失败"
            , "提交的活动不能没有任务信息"
            , "提交的活动不能没有奖励信息"
            , "提交的活动不能奖励数量不符合规定"
            , "提交的活动中的奖励中道具数量不符合规定"
            , "提交的活动中的任务次数数量不符合规定"
            , "保存表格文件失败"
            , "登入失败"
            , "用户名或密码不正确"
            };

        private static string _strLastErrMsg = "";
        // ======== Public Access Method ======== //
        public static void SetLastErrMsg(string errMsg)
        {
            _strLastErrMsg = errMsg;
        }

        public static void SetLastErrMsg(EErrType errType)
        {
            _strLastErrMsg = GetErrMsg(errType);
        }

        public static string GetLastErrMsg()
        {
            return _strLastErrMsg;
        }

        public static string GetErrMsg(EErrType errType)
        {
            int errId = (int)errType;
            return string.Format(@"{{""error"":{0}, ""msg"":""{1}""}}", errId, ErrMsgs[errId]);
        }

        // ======== Private Access Method ======== //
    }
}