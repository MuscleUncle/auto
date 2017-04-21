using MySql.Data.MySqlClient;
using System;
using WeiChat.Models;

namespace WeiChat.Dao
{
    public class InsuranceDao
    {
        private string _connection;
        public InsuranceDao(string connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// 保存车险内容
        /// </summary>
        /// <param name="model">车险内容模型</param>
        /// <returns></returns>
        public int SaveInsuranceByCarId(InsuranceModel model)
        {
            string sql = @"INSERT INTO car_insurance(
                                            user_id,
                                            car_id,
                                            insurance_name,
                                            insurance_time,
                                            end_insurance_time,
                                            insurance_type
                                      ) VALUES(
                                            @user,
                                            @car,
                                            @name,
                                            @start,
                                            @end,
                                            @type
                                      )";
            MySqlParameter[] cmdParams =
           {
                new MySqlParameter("@user",model.UserId),
                new MySqlParameter("@car",model.CarId),
                new MySqlParameter("@name",model.Name),
                new MySqlParameter("@start",Convert.ToDateTime(model.Start)),
                new MySqlParameter("@end",Convert.ToDateTime(model.End)),
                new MySqlParameter("@type",model.InsType)
            };

            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.ExecuteSql(sql, cmdParams);
        }

        /// <summary>
        /// 获取车险信息
        /// </summary>
        /// <returns></returns>
        public MySqlDataReader GetInsuranceList()
        {
            string sql = @"SELECT
	                                        car_id,
	                                        user_id,
	                                        insurance_name,
	                                        insurance_time,
	                                        end_insurance_time,
	                                        insurance_type
                                        FROM
	                                        car_insurance";
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            return db.ExecuteReader(sql);
        }

        /// <summary>
        /// 根据汽车ID获取车险内容
        /// </summary>
        /// <param name="carId">汽车ID</param>
        /// <returns></returns>
        public MySqlDataReader GetInsuranceByCarId(int carId)
        {
            string sql = @"SELECT
	                                        car_id,
	                                        user_id,
	                                        insurance_name,
	                                        insurance_time,
	                                        end_insurance_time,
	                                        insurance_type
                                        FROM
	                                        car_insurance
                                        WHERE car_id = @car";
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            MySqlParameter[] cmdParams =
            {
                new MySqlParameter("@car",carId)
            };
            return db.ExecuteReader(sql, cmdParams);
        }

        /// <summary>
        /// 根据用户ID获取车险内容
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public MySqlDataReader GetInsuranceByUserId(int userId)
        {
            string sql = @"SELECT
	                                        car_id,
	                                        user_id,
	                                        insurance_name,
	                                        insurance_time,
	                                        end_insurance_time,
	                                        insurance_type
                                        FROM
	                                        car_insurance
                                        WHERE user_id = @user";
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            MySqlParameter[] cmdParams =
            {
                new MySqlParameter("@user",userId)
            };
            return db.ExecuteReader(sql, cmdParams);
        }

        /// <summary>
        /// 根据用户ID和汽车ID获取车险内容
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="carId">汽车ID</param>
        /// <returns></returns>
        public MySqlDataReader GetInsuranceByUserIdAndCarId(int userId, int carId)
        {
            string sql = @"SELECT
	                                        car_id,
	                                        user_id,
	                                        insurance_name,
	                                        insurance_time,
	                                        end_insurance_time,
	                                        insurance_type
                                        FROM
	                                        car_insurance
                                        WHERE user_id = @user
                                        AND car_id = @car";
            MySqlDbHelper db = new MySqlDbHelper(_connection);
            MySqlParameter[] cmdParams =
            {
                new MySqlParameter("@car",carId),
                new MySqlParameter("@user",userId)
            };
            return db.ExecuteReader(sql, cmdParams);
        }
    }
}
