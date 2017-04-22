using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WeiChat.Models;
using WeiChat.Models.AppSettings;
using WeiChat.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WeiChat.Api.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(IOptions<ConnectionStrings> option)
        {
            var config = option.Value;
            _userService = new UserService(config.MySqlConnection);
        }

        [Route("user/list")]
        public string UserList()
        {
            return "dsdsdsdsdsdsd";
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("user/register")]
        [HttpPost]
        public int UserRegister([FromForm]UserInfo user)
        {
            return _userService.Register(user);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("user/login")]
        [HttpPost]
        public int UserLogin([FromForm]UserInfo user)
        {
            return _userService.Login(user.LoginName, user.LoginPwd);
        }

        /// <summary>
        /// 通过openId登录
        /// </summary>
        /// <param name="open">微信openId</param>
        /// <returns></returns>
        [Route("user/open")]
        [HttpPost]
        public int UserLoginByOpen([FromForm] string open)
        {
            return _userService.LoginByOpen(open);
        }
    }
}
