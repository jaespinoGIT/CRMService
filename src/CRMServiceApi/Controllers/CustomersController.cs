using AutoMapper;
using CRMService.Core.Domain.Entities;
using CRMService.Core.Interfaces.Gateways;
using CRMService.Core.Interfaces.UseCases.GetCustomerDetails;
using CRMService.Data;

using CRMService.Models;
using CRMService.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace CRMServiceApi.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
       
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IGetCustomerDetailsUseCase _getCustomerDetailsUseCase;

        public CustomersController(IGetCustomerDetailsUseCase getCustomerDetailsUseCase, IMapper mapper)
        {
            _getCustomerDetailsUseCase = getCustomerDetailsUseCase;
            _mapper = mapper;
        }

        //public CustomersController(ICustomerRepository repository, IMapper mapper)
        //{
        //    _customerRepository = repository;
        //    _mapper = mapper;
        //}


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
                var result = await _getCustomerDetailsUseCase.Execute(customerId);

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

        //[Route("{customerId}", Name = "GetCustomer")]
        //public async Task<IHttpActionResult> Get(int customerId, bool includeCustomerAudits = false)
        //{
        //    try
        //    {
        //        var result = await _customerRepository.GetCustomerAsync(customerId, includeCustomerAudits);

        //        if (result == null)
        //            return NotFound();

        //        var mappedResult = _mapper.Map<CustomerModel>(result);

        //        return Ok(mappedResult);
        //    }
        //    catch
        //    {
        //        return InternalServerError();
        //    }
        //}


        [Route()]
        public async Task<IHttpActionResult> Post(CustomerModel model)
        {
            try
            {     
                if (ModelState.IsValid)
                {
                    if (await _customerRepository.GetCustomerByNameAsync(model.Name) != null)
                    {
                        ModelState.AddModelError("Name", "Name in use");
                    }

                    var user = _mapper.Map<Customer>(model);

                    _customerRepository.AddCustomer(user);

                    if (await _customerRepository.SaveChangesAsync())
                    {
                        var newModel = _mapper.Map<CustomerModel>(user);

                        return CreatedAtRoute("GetCustomer", new { customerId = newModel.CustomerId }, newModel);
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
                            Operation = CustomerAuditOperationType.Update,                           
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


        [HttpPatch]
        [Route("{customerId}/upload")]
        public async Task<IHttpActionResult> Upload(int customerId)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return this.StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            var filesProvider = await Request.Content.ReadAsMultipartAsync();
            var fileContents = filesProvider.Contents.FirstOrDefault();
            if (fileContents == null)
            {
                return this.BadRequest("Missing file");
            }

            byte[] payload = await fileContents.ReadAsByteArrayAsync();        
            var customer = await _customerRepository.GetCustomerAsync(customerId, false);
            if (customer == null) return NotFound();         

            customer.Photo = payload;          

            if (await _customerRepository.SaveChangesAsync())
            {
                return Ok(_mapper.Map<CustomerModel>(customer));
            }
            else
            {
                return InternalServerError();
            }

            //return this.Ok(new
            //{
            //    Result = "file uploaded successfully",
            //});
        }


        //[Route("{customerId}/photo")]
        //[HttpPost]        
        //public async Task<IHttpActionResult> UploadCustomerImage(int customerId, [FromBody] UploadCustomerPhotoModel model)
        //{
        //    //Depending on if you want the byte array or a memory stream, you can use the below. 
        //    //THIS IS NO LONGER NEEDED AS OUR MODEL NOW HAS A BYTE ARRAY
        //    //var imageDataByteArray = Convert.FromBase64String(model.ImageData);

        //    //When creating a stream, you need to reset the position, without it you will see that you always write files with a 0 byte length. 
        //    var imageDataStream = new System.IO.MemoryStream(model.ImageData);
        //    imageDataStream.Position = 0;

        //    //Go and do something with the actual data.
        //    //_customerImageService.Upload([...])

        //    //For the purpose of the demo, we return a file so we can ensure it was uploaded correctly. 
        //    //But otherwise you can just return a 204 etc.
        //    return Ok();
        //    //return File(model.ImageData, "image/png");
        //}

        //[Route("{customerId}/photo")]
        //[Consumes]
        //[HttpPatch]
        //public async Task<IHttpActionResult> UploadCustomerPhoto(int customerId, [FromBody] JsonPatchDocument<CustomerModel> patchDoc)
        //{
        //    try
        //    {
        //        // If the received data is null
        //        if (patchDoc == null)
        //            return BadRequest();

        //        var customer = await _customerRepository.GetCustomerAsync(customerId, false);
        //        if (customer == null) return NotFound();

        //        var customerModelToPatch = _mapper.Map<CustomerModel>(customer);

        //        patchDoc.ApplyTo(customerModelToPatch);

        //        // Assign entity changes to original entity retrieved from database
        //        _mapper.Map(customerModelToPatch, customer);

        //        if (await _customerRepository.SaveChangesAsync())
        //        {
        //            return Ok(_mapper.Map<CustomerModel>(customer));
        //        }
        //        else
        //        {
        //            return InternalServerError();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }

        //}
    }
}
