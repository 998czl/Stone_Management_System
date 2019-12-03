
using System;
using System.Collections.Generic;
using System.Text;
using WebCore_Model;

namespace WebCore_Entity.Application.Interface
{
	public interface IAdministratorService
	{
		Administrator GetAdmin(string userName, string password);

		bool AddAdmin(Administrator model); 
			
	}
}
