using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using WeiChat.Dao;
using WeiChat.Models;

namespace WeiChat.Service
{
    public class AutoService
    {
        private AutoDao _autoDao;
        public AutoService(string connection)
        {
            _autoDao = new AutoDao(connection);
        }

        /// <summary>
        /// 获取所有汽车信息
        /// </summary>
        /// <returns></returns>
        public List<CarInfoModel> GetCarList()
        {
            MySqlDataReader reader = _autoDao.GetCarList();

            var list = ChangeCarList(reader);

            return list;
        }

        /// <summary>
        /// 根据条件获取汽车信息
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="plate">车牌号</param>
        /// <returns></returns>
        public CarInfoModel GetCarInfo(int id, string plate)
        {
            MySqlDataReader reader = _autoDao.GetCarInfo(id, plate);
            CarInfoModel model = null;
            while (reader.Read())
            {
                model = new CarInfoModel
                {
                    Id = Convert.ToInt32(reader["id"]),
                    PlateNum = reader["plate_num"].ToString(),
                    EngineNum = reader["engine_num"].ToString(),
                    UserId = Convert.ToInt32(reader["user_id"]),
                    FrameNum = reader["frame_num"].ToString()
                };
            }
            return model;
        }

        /// <summary>
        /// 根据用户ID获取汽车信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<CarInfoModel> GetCarsWithUserId(int userId)
        {
            MySqlDataReader reader = _autoDao.GetCarListByUserId(userId);

            var list = ChangeCarList(reader);

            return list;
        }

        /// <summary>
        /// 保存汽车信息
        /// </summary>
        /// <param name="model">汽车信息模型</param>
        /// <returns>
        ///     0：成功
        ///     1：存在
        ///     -1：数据存储异常
        /// </returns>
        public int SaveCarInfo(CarInfoModel model)
        {
            var exist = _autoDao.CarPlateNumExistByUserId(model.PlateNum, (int)model.UserId);
            if (exist)
            {
                return 1;
            }
            var insertResult = _autoDao.insertCarInfo(model);
            if (insertResult > 0)
            {
                return 0;
            }
            return -1;
        }

        /// <summary>
        /// 更新汽车信息（用户id和车牌号）
        /// </summary>
        /// <param name="model">汽车信息</param>
        /// <returns>
        ///     true：成功
        ///     false：异常
        /// </returns>
        public bool UpgradeCarInfo(CarInfoModel model)
        {
            return _autoDao.UpdateCarInfoByUserId(model) > 0;
        }

        /// <summary>
        /// 根据READER转换LIST
        /// </summary>
        /// <param name="reader">sql数据</param>
        /// <returns></returns>
        private List<CarInfoModel> ChangeCarList(MySqlDataReader reader)
        {
            List<CarInfoModel> list = new List<CarInfoModel>();
            while (reader.Read())
            {
                CarInfoModel temp = new CarInfoModel
                {
                    Id = Convert.ToInt32(reader["id"]),
                    PlateNum = reader["plate_num"].ToString(),
                    EngineNum = reader["engine_num"].ToString(),
                    UserId = Convert.ToInt32(reader["user_id"]),
                    FrameNum = reader["frame_num"].ToString()
                };
                list.Add(temp);
            }
            reader.Close();
            reader.Dispose();
            return list;
        }
    }
}
