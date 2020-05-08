using AutoMapper;
using CRMService.Data;
using CRMService.Data.Entities;
using CRMService.Models;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace CRMService.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository repository, IMapper mapper)
        {
            _customerRepository = repository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(bool includeCustomerAudits = false)
        {
            try
            {
                var result = await _customerRepository.GetAllCustomersAsync(includeCustomerAudits);

                if (result == null)
                    return NotFound();
                // Mapping 
                var mappedResult = _mapper.Map<IEnumerable<CustomerModel>>(result);

                return Ok(mappedResult);
            }
            catch
            {
                return InternalServerError();
            }
        }


        [Route("{customerId}", Name = "GetCustomer")]
        public async Task<IHttpActionResult> Get(int customerId, bool includeCustomerAudits = false)
        {
            try
            {
                var result = await _customerRepository.GetCustomerAsync(customerId, includeCustomerAudits);

                if (result == null)
                    return NotFound();

                var mappedResult = _mapper.Map<CustomerModel>(result);

                return Ok(mappedResult);
            }
            catch
            {
                return InternalServerError();
            }
        }


        [Route()]
        public async Task<IHttpActionResult> Post(CustomerModel model)
        {
            try
            {
                if (await _customerRepository.GetCustomerByNameAsync(model.Name) != null)
                {
                    ModelState.AddModelError("Name", "Name in use");
                }

                if (ModelState.IsValid)
                {
                    var user = _mapper.Map<Customer>(model);

                    _customerRepository.AddCustomer(user);

                    if (await _customerRepository.SaveChangesAsync())
                    {
                        var newModel = _mapper.Map<CustomerModel>(user);

                        return CreatedAtRoute("GetCustomer", new { moniker = newModel.CustomerId }, newModel);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return BadRequest(ModelState);
        }

        [Route("{customerId}")]
        public async Task<IHttpActionResult> Put(int customerId, CustomerModel model)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerAsync(customerId);
                if (customer == null) return NotFound();

                //var userId = ((ClaimsIdentity)User.Identity).FindFirst("UserId");
                model.CustomerAudits = new CustomerAuditModel[]
                        {
                        new CustomerAuditModel
                        {
                            Date = DateTime.Now,
                            Operation = Models.Enums.CustomerAuditOperationType.Update,                           
                            Customer = model
                        }

                        };


                _mapper.Map(model, customer);

                if (await _customerRepository.SaveChangesAsync())
                {
                    return Ok(_mapper.Map<CustomerModel>(customer));
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("{customerId}")]
        public async Task<IHttpActionResult> Delete(int customerId)
        {
            try
            {
                var Customer = await _customerRepository.GetCustomerAsync(customerId);
                if (Customer == null) return NotFound();

                _customerRepository.DeleteCustomer(Customer);

                if (await _customerRepository.SaveChangesAsync())
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
