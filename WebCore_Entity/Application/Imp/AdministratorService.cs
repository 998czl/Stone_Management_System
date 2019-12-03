using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using WebCore_Entity.Application.Interface;
using WebCore_Model;

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
	}
}
