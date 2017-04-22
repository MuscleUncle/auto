using MySql.Data.MySqlClient;
using WeiChat.Models;

namespace WeiChat.Dao
{
    public class AutoDao
    {
        private readonly string _connection;
        public AutoDao(string connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// 获取所有汽车信息
        /// </summary>
        /// <returns></returns>
        public MySqlDataReader GetCarList()
        {
            string sql = @"SELECT
	                                        id,
	                                        user_id,
	                                        plate_num,
	                                        frame_num,
	                                        engine_num
                                       FROM
	                                        car_info";
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.ExecuteReader(sql);
        }

        /// <summary>
        /// 根据id或车牌查询汽车信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="plate">车牌号</param>
        /// <returns></returns>
        public MySqlDataReader GetCarInfo(int id, string plate)
        {
            string sql = @"SELECT
	                                        id,
	                                        user_id,
	                                        plate_num,
	                                        frame_num,
	                                        engine_num
                                       FROM
	                                        car_info
                                       WHERE 
                                             1 = 1";

            if (id > 0)
            {
                sql += @" AND id = @id";
            }
            if (!string.IsNullOrEmpty(plate))
            {
                sql += @" AND plate_num = @plate";
            }

            MySqlParameter[] cmdParams =
            {
                new MySqlParameter("@id",id),
                new MySqlParameter("@plate",plate)
            };

            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.ExecuteReader(sql, cmdParams);
        }

        public MySqlDataReader GetCarListByUserId(int userId)
        {
            string sql = @"SELECT
	                                        id,
	                                        user_id,
	                                        plate_num,
	                                        frame_num,
	                                        engine_num
                                       FROM
	                                        car_info
                                       WHERE 
                                            user_id = @user ";
            MySqlParameter[] cmdParams =
            {
                new MySqlParameter("@user",userId)
            };
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.ExecuteReader(sql, cmdParams);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MySqlDataReader GetCarWithUserList()
        {
            string sql = @"SELECT
	                                        C.id,
	                                        C.user_id,
	                                        C.plate_num,
	                                        C.frame_num,
	                                        C.engine_num,
	                                        U.nick_name,
	                                        U.open_id,
	                                        U.status
                                        FROM
	                                        car_info AS C,
	                                        user_info AS U";
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.ExecuteReader(sql);
        }

        /// <summary>
        /// 保存汽车信息
        /// </summary>
        /// <param name="model">汽车信息模型</param>
        /// <returns></returns>
        public int insertCarInfo(CarInfoModel model)
        {
            string sql = $@"INSERT INTO car_info(
                                            user_id,
                                            plate_num,
                                            frame_num,
                                            engine_num
                                       ) VALUES(
                                            @user,
                                            @plate,
                                            @frame,
                                            @engine
                                        )";
            MySqlParameter[] cmdParams =
           {
                new MySqlParameter("@user",model.UserId),
                new MySqlParameter("@plate",model.PlateNum),
                new MySqlParameter("@frame",model.FrameNum),
                new MySqlParameter("@engine",model.EngineNum)
            };
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.ExecuteSql(sql, cmdParams);
        }

        /// <summary>
        /// 查询用户的车牌号是否存在
        /// </summary>
        /// <param name="plate">车牌号</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public bool CarPlateNumExistByUserId(string plate, int userId)
        {
            string sql = @"SELECT COUNT(1) 
                                    FROM car_info 
                                    WHERE user_id = @user 
                                    AND plate_num = @plate";
            MySqlParameter[] cmdParams =
            {
                new MySqlParameter("@user",userId),
                new MySqlParameter("@plate",plate)
            };

            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.Exists(sql, cmdParams);
        }

        /// <summary>
        /// 通过车牌号用户ID更新汽车信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int UpdateCarInfoByUserId(CarInfoModel model)
        {
            string sql = $@"UPDATE car_info
                                         SET frame_num = @frame,
                                                engine_num =  @engine
                                         WHERE plate_num = @plate,
                                          AND user_id = @user 
                                        ";
            MySqlParameter[] cmdParams =
           {
                new MySqlParameter("@user",model.UserId),
                new MySqlParameter("@plate",model.PlateNum),
                new MySqlParameter("@frame",model.FrameNum),
                new MySqlParameter("@engine",model.EngineNum)
            };
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.ExecuteSql(sql, cmdParams);
        }
    }
}
