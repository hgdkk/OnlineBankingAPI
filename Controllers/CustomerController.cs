using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingAPI.Models;
using OnlineBankingAPI.Services.Abstract;

namespace OnlineBankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        IMapper _mapper;
        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }


        [HttpPost]
        [Route("createNewCustomer")]
        public IActionResult CreateNewCustomer([FromBody] CreateNewCustomerModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);

            var customer = _mapper.Map<Customer>(model);
            var response = _customerService.Create(customer);
            if (response.IsSuccess)
                return Ok(response);
            else
                return BadRequest(response);
        }

        [HttpGet]
        [Route("getAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            var response = _customerService.GetAllCustomers();
            return Ok(response);
        }

        [HttpPost]
        [Route("updateCustomer")]
        public IActionResult UpdateCustomer([FromBody] UpdateCustomerModel model)
        {
            if (!ModelState.IsValid) return BadRequest(model);
            var customer = _mapper.Map<Customer>(model);
            var response = _customerService.Update(customer);
            if (response.IsSuccess)
                return Ok();
            else
                return BadRequest(response.ResponseMessage);
        }
    }
}
