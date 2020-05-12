﻿using AutoMapper;
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
using System.ComponentModel;
using CRMService.Helpers.Filters;
using CRMService.Core.Exceptions.Services;

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

            var result = await _customerService.GetAllCustomersAsync(includeCustomerAudits);

            if (result == null)
                return NotFound();
            // Mapping 
            var mappedResult = _mapper.Map<IEnumerable<CustomerModel>>(result);

            return Ok(mappedResult);

        }

        [Route("{customerId}", Name = "GetCustomer")]
        public async Task<IHttpActionResult> Get(int customerId, bool full = false)
        {

            var result = await _customerService.GetCustomerAsync(customerId, full);

            if (result == null)
                return NotFound();

            var mappedResult = _mapper.Map<CustomerModel>(result);

            return Ok(mappedResult);

        }


        [Route()]
        public async Task<IHttpActionResult> Post(CustomerModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var customer = _mapper.Map<Customer>(model);

                customer = await _customerService.AddCustomer(customer);

                if (customer != null)
                {
                    var newModel = _mapper.Map<CustomerModel>(customer);

                    return CreatedAtRoute("GetCustomer", new { customerId = newModel.CustomerId }, newModel);
                }
            }


            return BadRequest();
        }

        [Route("{customerId}")]
        public async Task<IHttpActionResult> Put(int customerId, CustomerModel model)
        {

            var customer = await _customerService.GetCustomerAsync(customerId);
            if (customer == null)
                return NotFound();
            _mapper.Map(model, customer);

            var customerUpdated = await _customerService.UpdateCustomer(customer);

            if (customerUpdated == null)
                return InternalServerError();
            else
                return Ok(_mapper.Map<CustomerModel>(customerUpdated));

        }

        [Route("{customerId}")]
        public async Task<IHttpActionResult> Delete(int customerId)
        {
            if (await _customerService.DeleteCustomer(customerId))
                return Ok();
            else
                return InternalServerError();
        }

        [Route("{customerId}/photo")]
        [HttpPut]
        public async Task<IHttpActionResult> UploadCustomerImage(int customerId, [FromBody] UploadCustomerPhotoModel model)
        {
            if (model == null || model.Photo == null)
                return BadRequest();

            //When creating a stream, you need to reset the position, without it you will see that you always write files with a 0 byte length. 
            //var imageDataStream = new System.IO.MemoryStream(model.Photo);
            //imageDataStream.Position = 0;

            var customer = await _customerService.GetCustomerAsync(customerId, false);
            if (customer == null)
                return NotFound();
            customer.Photo = model.Photo;
            var customerUpdated = await _customerService.UpdateCustomer(customer);
            if (customerUpdated != null)
                return Ok(_mapper.Map<CustomerModel>(customerUpdated));
            else
                return InternalServerError();
        }

        [Route("{customerId}/photo")]
        [HttpPatch]
        public async Task<IHttpActionResult> UploadCustomerPhoto(int customerId, [FromBody] JsonPatchDocument<CustomerModel> patchDoc)
        {
            // If the received data is null
            if (patchDoc == null)
                return BadRequest();

            var customer = await _customerService.GetCustomerAsync(customerId, false);
            if (customer == null)
                return NotFound();
            var customerModelToPatch = _mapper.Map<CustomerModel>(customer);

            patchDoc.ApplyTo(customerModelToPatch);

            // Assign entity changes to original entity retrieved from database   
            var customerUpdated = await _customerService.UpdateCustomer(_mapper.Map(customerModelToPatch, customer));
            if (customerUpdated == null)
                return BadRequest();
            else
                return Ok(_mapper.Map<CustomerModel>(customerUpdated));
        }

        [HttpPatch]
        [Route("{customerId}/upload")]
        public async Task<IHttpActionResult> Upload(int customerId)
        {
            if (!Request.Content.IsMimeMultipartContent())
                return this.StatusCode(HttpStatusCode.UnsupportedMediaType);

            var filesProvider = await Request.Content.ReadAsMultipartAsync();
            var fileContents = filesProvider.Contents.FirstOrDefault();
            if (fileContents == null)
                return this.BadRequest("Missing file");

            byte[] payload = await fileContents.ReadAsByteArrayAsync();
            var customer = await _customerService.GetCustomerAsync(customerId);
            if (customer == null)
                return NotFound();
            customer.Photo = payload;

            var customerUpdated = await _customerService.UpdateCustomer(customer);
            if (customerUpdated == null)
                return BadRequest();
            else
                return Ok(_mapper.Map<CustomerModel>(customerUpdated));
        }
    }
}
