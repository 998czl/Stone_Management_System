using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore_Entity.Application.Interface;
using WebCore_Entity.RedisManager;
using WebCore_Model;
using WebCore_Model.External;
using WebCore_Utils.ApiUtils;

namespace WebCore_Entity.Application.Imp
{
	public class AdministratorService : IAdministratorService
	{
		/// <summary>
		/// _Db
		/// </summary>
		private SqlSugarClient _Db;

		/// <summary>
		/// 数据库
		/// </summary>
		protected SqlSugarClient Db
		{
			get
			{
				if (_Db == null)
				{
					_Db = SqlSugarHelper.CreateDb();
				}
				return _Db;
			}
		}

		public bool AddAdmin(Administrator model)
		{
			try
			{
				Db.Ado.BeginTran(System.Data.IsolationLevel.ReadCommitted);

				Db.Insertable(model).ExecuteCommandIdentityIntoEntity();

				Db.Ado.CommitTran();

				return true;
			}
			catch
			{
				return false;
			}
		}

		public Administrator GetAdmin(string userName, string password)
		{
			try
			{
				var admin = Db.Queryable<Administrator>().First(s => s.UserName == userName && s.UserPassword == password);
				if (admin == null)
				{
					throw new Exception("用户名或密码不对!");
				}
				return admin;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


		//api登陆
		public Token<LoginInfo> Login(string username, string password)
		{
			if (string.IsNullOrEmpty(username))
			{
				throw new MTSException("用户名不能为空");
			}
			if (string.IsNullOrEmpty(password))
			{
				throw new MTSException("密码不能为空");
			}

			var result = Db.Queryable<Administrator>().First(s => s.UserName == username && s.UserPassword == password);
			if (result == null)
			{
				throw new Exception("用户名或密码不对!");
			}
			var loginInfo = new LoginInfo(LoginCategory.User)
			{
				Id = result.UserId,
				Username = result.UserName,				
				Nickname = result.RealName,								
			};
			var token =  new Token<LoginInfo>()
			{
				Id = loginInfo.Id,
				Signature = "123456",
				Expiry = DateTime.UtcNow.Add(new TimeSpan(0, 30, 0)),
				Data = loginInfo
			};
			//TokenHelper.Set(loginInfo, "User");
			return token;
		}
	}
}
