using System;
using System.Collections.Generic;
using System.Text;
using WeiChat.Dao;
using WeiChat.Models;

namespace WeiChat.Service
{
    public class UserService
    {
        private UserInfoDao userInfoDao;
        public UserService(string connection)
        {
            userInfoDao = new UserInfoDao(connection);
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user">用户模型</param>
        /// <returns>
        ///         0：注册成功
        ///         1：用户名存在
        ///         -1：存储异常
        /// </returns>
        public int Register(UserInfo user)
        {
            bool existsResult = userInfoDao.LoginExists(user.LoginName);
            if (existsResult)
            {
                return 1;
            }
            int count = userInfoDao.InsertUser(user);

            if (count > 0)
            {
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// 账号密码进行登录
        /// </summary>
        /// <param name="lgName">用户名称</param>
        /// <param name="lgPwd">用户密码</param>
        /// <returns>
        ///     1：成功
        ///     0：失败
        /// </returns>
        public int Login(string lgName, string lgPwd)
        {
            bool existsResult = userInfoDao.LoginWithPwdExists(lgName, lgPwd);
            if (existsResult)
            {
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 通过OpenId登录
        /// </summary>
        /// <param name="openId">openId</param>
        /// <returns>
        ///     1：存在
        ///     0：不存在
        /// </returns>
        public int LoginByOpen(string openId)
        {
            bool existsResult = userInfoDao.LoginWithOpenId(openId);

            if (existsResult)
            {
                return 1;
            }
            return 0;
        }
    }
}
