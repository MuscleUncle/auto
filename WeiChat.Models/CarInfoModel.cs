namespace WeiChat.Models
{
    /// <summary>
    /// 汽车信息模型
    /// </summary>
    public class CarInfoModel
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public string PlateNum { get; set; }
        public string FrameNum { get; set; }
        public string EngineNum { get; set; }
    }
}
