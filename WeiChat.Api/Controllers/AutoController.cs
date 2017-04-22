using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using WeiChat.Models;
using WeiChat.Models.AppSettings;
using WeiChat.Service;

namespace WeiChat.Api.Controllers
{
    public class AutoController : Controller
    {
        private readonly AutoService _autoService;
        public AutoController(IOptions<ConnectionStrings> option)
        {
            var config = option.Value;
            _autoService = new AutoService(config.MySqlConnection);
        }

        /// <summary>
        /// 获取所有汽车信息
        /// </summary>
        /// <returns></returns>
        [Route("car/list")]
        [HttpGet]
        public string CarList()
        {
            var list = _autoService.GetCarList();
            string json = JsonConvert.SerializeObject(list);
            return json;
        }

        /// <summary>
        /// 根据ID或车牌号获取汽车模型
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="plate">车牌号</param>
        /// <returns></returns>
        [Route("car/param")]
        [HttpGet]
        public string CarByIdOrPlate(int id = -1, string plate = "")
        {
            var model = _autoService.GetCarInfo(id, plate);
            string json = JsonConvert.SerializeObject(model);
            return json;
        }

        /// <summary>
        /// 根据用户ID获取汽车列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [Route("car/user")]
        [HttpGet]
        public string CarByUserId(int userId)
        {
            var list = _autoService.GetCarsWithUserId(userId);
            string json = JsonConvert.SerializeObject(list);
            return json;
        }

        /// <summary>
        /// 保存汽车信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("car/save")]
        [HttpPost]
        public int SaveCarInfoByUser([FromForm]CarInfoModel model)
        {
            try
            {
                var result = _autoService.SaveCarInfo(model);
                return result;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// 更新汽车信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("car/update")]
        [HttpPost]
        public int UpgradeCarInfoByPlateAndUser([FromForm]CarInfoModel model)
        {
            try
            {
                var result = _autoService.UpgradeCarInfo(model);
                return result ? 0 : -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
