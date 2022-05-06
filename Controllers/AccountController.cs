using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingAPI.Models;
using OnlineBankingAPI.Services.Abstract;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OnlineBankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        IMapper _mapper;
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("getAllAccounts")]    
        public IActionResult GetAllAccounts()
        {
            var response = _accountService.GetAllAccounts();
            var cleanedAccounts = _mapper.Map<IList<GetAccountModel>>(response.Data);
            return Ok(cleanedAccounts);
        }

        [HttpPost]
        [Route("createNewAccount")]
        public IActionResult CreateNewAccount([FromBody] CreateNewAccountModel newAccount)
        {
            Response response = new Response();
            if (!ModelState.IsValid)
            {
                return BadRequest(newAccount);
            }

            var account = _mapper.Map<Account>(newAccount);
            response = _accountService.Create(newAccount.IdentityNumber, account, newAccount.Pin, newAccount.ConfirmPin);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response.ResponseMessage);
        }

        [HttpPost]
        [Route("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            Response response = new Response();
            if (!ModelState.IsValid)
            {                
                return BadRequest(model);
            }

            response = _accountService.Authenticate(model.AccountNumber, model.Pin);
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response.ResponseMessage);
        }

        [HttpGet]
        [Route("getAccountByNumber")]
        public IActionResult GetAccountByNumber(string accountNumber)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10 digit");

            var response = _accountService.GetAccountByAccountNumber(accountNumber);
            var cleanedAccount = _mapper.Map<GetAccountModel>(response.Data);
            return Ok(cleanedAccount);
        }

        [HttpGet]
        [Route("getByAccountId")]
        public IActionResult GetByAccountId(int Id)
        {
            var response = _accountService.GetAccountById(Id);
            var cleanedAccount = _mapper.Map<GetAccountModel>(response.Data);
            return Ok(cleanedAccount);
        }

        [HttpPost]
        [Route("updateAccount")]
        public IActionResult UpdateAccount([FromBody] UpdateAccountModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var account = _mapper.Map<Account>(model);
            var response = _accountService.Update(account, model.Pin);
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response.ResponseMessage);
        }

        [HttpPost]
        [Route("deleteAccount")]
        public IActionResult DeleteAccount([FromBody] AuthenticateModel model)
        {
            var response = _accountService.Delete(model);
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response.ResponseMessage);
        }

        [HttpGet]
        [Route("getCustomerAccounts")]
        public IActionResult GetCustomerAccounts(string identityNumber)
        {
            var response = _accountService.GetCustomerAccounts(identityNumber);
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
