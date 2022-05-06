using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingAPI.Services.Abstract;
using System;
using System.Text.RegularExpressions;

namespace OnlineBankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        IMapper _mapper;
        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("makeDeposit")]
        public IActionResult MakeDeposit(string accountNumber, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10 digit");

            var response = _transactionService.Makedeposit(accountNumber, amount, transactionPin);
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpPost]
        [Route("makeWithdrawal")]
        public IActionResult MakeWithdrawal(string accountNumber, decimal amount, string transactionPin)
        {
            if (!Regex.IsMatch(accountNumber, @"^[0][1-9]\d{9}$|^[1-9]\d{9}$")) return BadRequest("Account number must be 10 digit");

            var response = _transactionService.MakeWithdrawal(accountNumber, amount, transactionPin);
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpGet]
        [Route("findCustomerTransactionsByDate")]
        public IActionResult FindCustomerTransactionsByDate(string identityNumber, DateTime startDate, DateTime endDate)
        {
            var response = _transactionService.FindCustomerTransactionsByDate(identityNumber, startDate, endDate);

            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpGet]
        [Route("getAccountTransactions")]
        public IActionResult GetAccountTransactions(string accountNumber, string pin)
        {
            var response = _transactionService.GetAccountTransactions(accountNumber, pin);
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }
    }
}
