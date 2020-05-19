using AutoMapper;
using CRMService.Core.Domain.Entities;
using CRMService.Core.Services.Interfaces;
using CRMService.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Web.Http;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace CRMService.Controllers
{
    /// <summary>
    /// Customer operations controller
    /// </summary>
    [ApiVersion("1.0")]
    [Authorize()]
    [RoutePrefix("api/customers")]
    public class CustomersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="customerService"></param>
        /// <param name="mapper"></param>
        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        /// GET: api/customers
        /// <summary>
        /// Gets all registered customers.
        /// </summary>
        /// <remarks>
        /// Gets all registered customers.
        /// </remarks>   
        /// <param name="includeCustomerAudits"></param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Response customers list.</response>  
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
        /// GET: api/customers/{customerId}
        /// <summary>
        /// Gets an customer by id.
        /// </summary>
        /// <remarks>
        /// Gets all data of an customer.
        /// </remarks>  
        /// <param name="customerId">Id (int) of the customer.</param>
        /// <param name="full">Boolean. Get all info if true.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Response customer.</response>  
        /// <response code="404">NotFound. User not found.</response>
        [Route("{customerId}", Name = "GetCustomer")]
        public async Task<IHttpActionResult> Get(int customerId, bool full = false)
        {

            var result = await _customerService.GetCustomerAsync(customerId, full);

            if (result == null)
                return NotFound();

            var mappedResult = _mapper.Map<CustomerModel>(result);

            return Ok(mappedResult);

        }

        /// POST: api/customers
        /// <summary>
        /// Add a new customer
        /// </summary>
        /// <remarks>
        /// Add a new customer to the db
        /// </remarks>
        /// <param name="model"> Customer to be created.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>                            
        /// <response code="201">Created. Customer created.</response>        
        /// <response code="400">BadRequest. Wrong object format.  .</response>
        /// <response code="409">Conflict. There is an customer already in the db.</response>
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

                customer = await _customerService.AddCustomer(customer, User.Identity.GetUserId());

                if (customer != null)
                {
                    var newModel = _mapper.Map<CustomerModel>(customer);

                    return CreatedAtRoute("GetCustomer", new { customerId = newModel.CustomerId }, newModel);
                }
            }


            return BadRequest();
        }
        /// PUT: api/customers/{customerId}
        /// <summary>
        /// Change the data of the customer.
        /// </summary>
        /// <remarks>
        /// Change the data of the customer.
        /// </remarks>     
        /// <param name="customerId">Id (int) of the customer.</param>
        /// <param name="model"> Updated data customer.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. customer data changed</response>         
        /// <response code="400">BadRequest. Wrong object format.</response>
        [Route("{customerId}")]
        public async Task<IHttpActionResult> Put(int customerId, CustomerModel model)
        {

            var customer = await _customerService.GetCustomerForUpdateAsync(customerId);
            if (customer == null)
                return NotFound();
            _mapper.Map(model, customer);         

            var customerUpdated = await _customerService.UpdateCustomer(customer, User.Identity.GetUserId());

            if (customerUpdated == null)
                return InternalServerError();
            else
                return Ok(_mapper.Map<CustomerModel>(customerUpdated));

        }
        /// DELETE: api/customers/{customerId}
        /// <summary>
        /// Delete customer by id
        /// </summary>
        /// <remarks>
        /// Delete customer by id
        /// </remarks> 
        /// <param name="customerId">Id (int) of the customer.</param>
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. User deleted.</response>  
        /// <response code="404">NotFound. User not found.</response>
        [Route("{customerId}")]
        public async Task<IHttpActionResult> Delete(int customerId)
        {
            if (await _customerService.DeleteCustomer(customerId))
                return Ok();
            else
                return InternalServerError();
        }

        /// PUT: api/customers/{customerId}/photo
        /// <summary>
        /// Uploads the photo of the customer.
        /// </summary>
        /// <remarks>
        /// Uploads the photo of the customer.
        /// </remarks>     
        /// <param name="customerId">Id (int) of the customer.</param>      
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. customer photo uploaded</response>         
        /// <response code="400">BadRequest. Wrong object format.</response>
        [HttpPut]
        [Route("{customerId}/photo")]
        public async Task<IHttpActionResult> UploadPhotoFile(int customerId)
        {
            if (!Request.Content.IsMimeMultipartContent())
                return this.StatusCode(HttpStatusCode.UnsupportedMediaType);

            var filesProvider = await Request.Content.ReadAsMultipartAsync();
            var fileContents = filesProvider.Contents.FirstOrDefault();
            if (fileContents == null)
                return this.BadRequest("Missing file");

            byte[] payload = await fileContents.ReadAsByteArrayAsync();
            var customer = await _customerService.GetCustomerForUpdateAsync(customerId);
            if (customer == null)
                return NotFound();
            customer.Photo = payload;
         
            var customerUpdated = await _customerService.UpdateCustomer(customer, User.Identity.GetUserId());
            if (customerUpdated == null)
                return BadRequest();
            else
                return Ok(_mapper.Map<CustomerModel>(customerUpdated));
        }
        /// GET: api/customers/{customerId}/photo
        /// <summary>
        /// Gets the photo of the customer by id.
        /// </summary>
        /// <remarks>
        /// Gets the photo of the customer by id.
        /// </remarks>  
        /// <param name="customerId">Id (int) of the customer.</param>      
        /// <response code="401">Unauthorized. Incorrect or inexistent jwt token or not enough permissions.</response>              
        /// <response code="200">OK. Photo response message.</response>  
        /// <response code="404">NotFound. User or photo not found.</response> 
        [Route("{customerId}/photo")]
        public async Task<HttpResponseMessage> Get(int customerId)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var customer = await _customerService.GetCustomerAsync(customerId, true);
            if (customer == null || customer.Photo == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            result.Content = new ByteArrayContent(customer.Photo);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return result;
        }
    }
}
