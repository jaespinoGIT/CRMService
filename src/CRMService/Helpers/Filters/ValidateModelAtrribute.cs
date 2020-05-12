using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CRMService.Helpers.Filters
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ModelState.IsValid == false)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.BadRequest, actionContext.ModelState);
            }
        }
    }

    class CustomValidatorAttribute : ValidationAttribute
    {
        //custom message in ctor
        public CustomValidatorAttribute() : base("Validation model error") { }
        public CustomValidatorAttribute(string Message) : base(Message) { }
        public override bool IsValid(object value)
        {
            return !string.IsNullOrWhiteSpace(value.ToString());
        }
        //return a overriden ValidationResult
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            if (IsValid(value)) return ValidationResult.Success;
            var message = "Validation model error";
            return new ValidationResult(message);
        }
    }
}
