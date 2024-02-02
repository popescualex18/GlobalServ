using GlobalServ.Api.Attributes;
using GlobalServ.BusinessLogic.Interfaces;
using GlobalServ.Core.Helpers;
using GlobalServ.Core.Interfaces;
using GlobalServ.DataModels.Models;
using GlobalServ.DomainModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace GlobalServ.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IServiceBusinessService _serviceBusinessService;
        private readonly IOptions<ApiOptions> _apiOptions;
        private readonly ITokenHelper _tokenHelper;
        public ServiceController(IServiceBusinessService serviceBusinessService, ITokenHelper tokenHelper, IOptions<ApiOptions> apiOptions)
        {
            _serviceBusinessService = serviceBusinessService;
            _tokenHelper = tokenHelper; 
            _apiOptions = apiOptions;   
        }

        [HttpPost("add-service")]
        [Allow(Common.Enum.RolesEnum.User)]
        public IActionResult Register([FromBody] ServiceDto model)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var values);
            var principal = _tokenHelper.GetPrincipalFromToken(values.First()!, _apiOptions.Value.TokenOptions.Secret, true);
            var entityToAdd = new ServiceModel
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(principal!.Identity!.Name),
                Description = model.Description,
                Name = model.Name,
                Price = model.Price,

            };
            _serviceBusinessService.Add(entityToAdd);
            return Ok(entityToAdd);
        }
        [HttpPut("update-service/{serviceId}")]
        [Allow(Common.Enum.RolesEnum.User)]
        public IActionResult UpdateService(Guid serviceId,[FromBody] ServiceDto model)
        {
            HttpContext.Request.Headers.TryGetValue("Authorization", out var values);
            var principal = _tokenHelper.GetPrincipalFromToken(values.First()!, _apiOptions.Value.TokenOptions.Secret, true);
            var entityToAdd = new ServiceModel
            {
                Id = serviceId,
                UserId = Guid.Parse(principal!.Identity!.Name),
                Description = model.Description,
                Name = model.Name,
                Price = model.Price,

            };
            _serviceBusinessService.Update(entityToAdd);
            return Ok();
        }
        [HttpDelete("delete-service/{serviceId}")]
        [Allow(Common.Enum.RolesEnum.User)]
        public IActionResult DeleteService(Guid serviceId)
        {
            
            var entityToDelete = _serviceBusinessService.Get(x => x.Id == serviceId).FirstOrDefault();
            if(entityToDelete != null)
            {
                _serviceBusinessService.Delete(entityToDelete);
            }
            
            return Ok();
        }
    }
}
