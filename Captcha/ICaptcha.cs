using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XjjXmm.Infrastructure.Captcha;

public interface ICaptcha
{
    /// <summary>
    /// 获得验证数据
    /// </summary>
    /// <returns></returns>
    Task<CaptchaOutput> Get();

    /// <summary>
    /// 检查验证数据
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> Check(CaptchaInput input);
}
