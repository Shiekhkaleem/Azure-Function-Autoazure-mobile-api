using AutoAzureMob.DAL.DAL;
using AutoAzureMob.Models.Models.Company;
using AutoAzureMob.Models.Models.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoAzureMob.BLL.BLL
{
    public class CompanyHandler : BaseHandler
    {
        private readonly IConfiguration _config;
        private readonly CompanyDAO companyDAO;
        public CompanyHandler(ExecuteContext executeContext,IConfiguration config): base(executeContext, config)
        {
            _config = config;
            companyDAO = new CompanyDAO(executeContext, _config);
        }
        public ResponseModel<List<CompanyRole>> GetAllCompanyRoles()
        {
            ResponseModel<List<CompanyRole>> response = new ResponseModel<List<CompanyRole>>();
            response.Content = companyDAO.GetAllCompanyRoles();
            response.Success = true;
            response.Description = "Data not found.";
            if (response.Content != null && response.Content.Count > 0)
            {
                response.Description = "Company role list.";
            }
            return response;
        }
    }
}
