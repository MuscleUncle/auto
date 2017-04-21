using MySql.Data.MySqlClient;
using WeiChat.Models;

namespace WeiChat.Dao
{
    public class UserInfoDao
    {
        private string _connection;
        public UserInfoDao(string connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int InsertUser(UserInfo user)
        {
            string sql = @"INSERT INTO user_info(
                                            login_name,
                                            login_pwd,
                                            nick_name,
                                            create_time,
                                            status,
                                            open_id
                                        ) VALUES(
                                            @lgName,
                                            @lgPwd,
                                            @nickName,
                                             now(),
                                             0,
                                             @openId
                                        )";
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            MySqlParameter[] cmdParms =
            {
                new MySqlParameter("@lgName",user.LoginName),
                new MySqlParameter("@lgPwd",user.LoginPwd),
                new MySqlParameter("@nickName",user.NickName),
                new MySqlParameter("@openId",user.OpenId)
            };
            return db.ExecuteSql(sql, cmdParms);
        }

        /// <summary>
        /// 查询账号是否存在
        /// </summary>
        /// <param name="lgName">账号信息</param>
        /// <returns>
        ///     true：账号名存在
        ///     false：账号名不存在
        /// </returns>
        public bool LoginExists(string lgName)
        {
            string sql = @"SELECT COUNT(1) FROM user_info WHERE login_name  = @lgName";

            MySqlParameter[] cmdParams = { new MySqlParameter("@lgName", lgName) };

            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.Exists(sql, cmdParams);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="lgName">用户登录名</param>
        /// <param name="lgPwd">用户登录密码</param>
        /// <returns>
        ///     true：登录信息正确
        ///     false：登录信息错误
        /// </returns>
        public bool LoginWithPwdExists(string lgName, string lgPwd)
        {
            string sql = @"SELECT COUNT(1) FROM user_info WHERE login_name  = @lgName AND login_pwd = @lgPwd";

            MySqlParameter[] cmdParams =
            {
                new MySqlParameter("@lgName", lgName),
                new MySqlParameter("@lgPwd",lgPwd)
            };

            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.Exists(sql, cmdParams);
        }

        /// <summary>
        /// 通过openid登录
        /// </summary>
        /// <param name="openId">微信openId</param>
        /// <returns></returns>
        public bool LoginWithOpenId(string openId)
        {
            string sql = @"SELECT COUNT(1) FROM user_info WHERE open_id = @open";

            MySqlParameter[] cmdParams =
            {
                new MySqlParameter("@open",openId)
            };

            MySqlDbHelper db = new MySqlDbHelper(_connection);

            return db.Exists(sql, cmdParams);
        }
    }
}
