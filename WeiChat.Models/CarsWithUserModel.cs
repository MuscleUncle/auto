using System.Collections.Generic;

namespace WeiChat.Models
{
    /// <summary>
    /// 汽车用户模型
    /// </summary>
    public class CarsWithUserModel
    {
        public UserInfo User { get; set; }
        public List<CarInfoModel> Cars { get; set; }
    }
}
