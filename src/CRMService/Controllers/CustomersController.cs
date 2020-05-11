using AutoMapper;
using CRMService.Core.Domain.Entities;
using CRMService.Core.Services.Interfaces;

using CRMService.Infrastructure.Data.EntityFramework.Repositories;
using CRMService.Models;
using Marvin.JsonPatch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Serilog;
using CRMService.Helpers;

namespace CRMService.Controllers
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {   
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(bool includeCustomerAudits = false)
        {
            try
            {
                var result = await _customerService.GetAllCustomersAsync(includeCustomerAudits);

                if (result == null)
                    return NotFound();
                // Mapping 
                var mappedResult = _mapper.Map<IEnumerable<CustomerModel>>(result);

                return Ok(mappedResult);
            }
            catch 
            {
                //Log.Error(response.ErrorMessage.Title);
                //Log.Error(response.ErrorMessage.Message);

                return InternalServerError();
            }
        }

        [Route("{customerId}", Name = "GetCustomer")]
        public async Task<IHttpActionResult> Get(int customerId, bool full = false)
        {
            try
            {
                var result = await _customerService.GetCustomerAsync(customerId, full);

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
                if (!ModelState.IsValid)
                {
                   return BadRequest(ModelState);
                }
                else
                {
                    var customer = _mapper.Map<Customer>(model);                    

                    if (await _customerService.AddCustomer(customer))
                    {
                        var newModel = _mapper.Map<CustomerModel>(customer);

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
                if (!ModelState.IsValid)
                    return BadRequest("Not a valid model");

                 var customer = _mapper.Map<Customer>(model);

                var customerUpdated = await _customerService.UpdateCustomer(customerId, customer);
                
                if (customerUpdated == null)
                    return BadRequest();
                else 
                    return Ok(_mapper.Map<CustomerModel>(customerUpdated));
               
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
                if (await _customerService.DeleteCustomer(customerId))
                    return Ok();
                else
                    return BadRequest();
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
            var customer = await _customerService.GetCustomerAsync(customerId);
            if (customer == null) return NotFound();         

            customer.Photo = payload;

            var customerUpdated = await _customerService.UpdateCustomer(customerId, customer);
            if (customerUpdated != null)
            {
                return Ok(_mapper.Map<CustomerModel>(customerUpdated));
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


        [Route("{customerId}/photo")]
        [HttpPost]        
        public async Task<IHttpActionResult> UploadCustomerImage(int customerId, [FromBody] UploadCustomerPhotoModel model)
        {
            //Depending on if you want the byte array or a memory stream, you can use the below. 
            //THIS IS NO LONGER NEEDED AS OUR MODEL NOW HAS A BYTE ARRAY
            //var imageDataByteArray = Convert.FromBase64String(model.ImageData);

            //When creating a stream, you need to reset the position, without it you will see that you always write files with a 0 byte length. 
            var imageDataStream = new System.IO.MemoryStream(model.ImageData);
            imageDataStream.Position = 0;

            //Go and do something with the actual data.
            //_customerImageService.Upload([...])

            //For the purpose of the demo, we return a file so we can ensure it was uploaded correctly. 
            //But otherwise you can just return a 204 etc.
            return Ok();
            //return File(model.ImageData, "image/png");
        }

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

        //        var customer = await _customerService.GetCustomerAsync(customerId, false);
        //        if (customer == null) return NotFound();

        //        var customerModelToPatch = _mapper.Map<CustomerModel>(customer);

        //        patchDoc.ApplyTo(customerModelToPatch);

        //        // Assign entity changes to original entity retrieved from database
        //        _mapper.Map(customerModelToPatch, customer);

        //        if (await _customerService.SaveChangesAsync())
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
