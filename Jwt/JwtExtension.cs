using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using XjjXmm.Infrastructure.Configuration;

namespace XjjXmm.Infrastructure.Jwt
{
	public static class JwtExtension
	{
		public static WebApplicationBuilder  RegistJwt(this WebApplicationBuilder builder)
		{
			var jwtConfig = ConfigHelper.GetSection<JwtTokenSetting>("jwt");
			
			builder.Services.AddAuthentication(x =>
				{
					//看这个单词熟悉么？没错，就是上边错误里的那个。
					x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				})// 也可以直接写字符串，AddAuthentication("Bearer")
				.AddJwtBearer(o =>
				{
					//var keyByteArray = System.Text.Encoding.ASCII.GetBytes(jwtConfig.Secret);
					var signingKey = jwtConfig.IssuerSigningKey;

					//var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,//是否验证IssuerSigningKey 
						IssuerSigningKey = signingKey,//参数配置在下边

						ValidateIssuer = true,   //是否验证Issuer
						ValidIssuer = jwtConfig.Issuer,//发行人


						ValidateAudience = true,//是否验证Audience 
						ValidAudience = jwtConfig.Audience,//订阅人

						ValidateLifetime = true,//是否验证超时  当设置exp和nbf时有效 同时启用ClockSkew 
						//ClockSkew = TimeSpan.Zero,//这个是缓冲过期时间，也就是说，即使我们配置了过期时间，这里也要考虑进去，过期时间+缓冲，默认好像是7分钟，你可以直接设置为0
						ClockSkew = jwtConfig.GetClickSkew(),

						RequireExpirationTime = true,
					};

				});
			
			return builder;
		}
	}
}
