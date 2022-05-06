using AutoMapper;
using OnlineBankingAPI.Models;

namespace OnlineBankingAPI.Profiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CreateNewCustomerModel, Customer>();
            CreateMap<UpdateCustomerModel, Customer>();
            CreateMap<CreateNewAccountModel, Account>();
            CreateMap<UpdateAccountModel, Account>();
            CreateMap<Account, GetAccountModel>();
        }
        
    }
}
