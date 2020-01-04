

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hackathon.Models;
using Hackathon.Services;

namespace Hackathon.Controllers
{
    //[Route("api/[controller]/[action]")] // -> Invoke method using its name

    [Route("[controller]")] //-- > by default map with HttpMethod attributes
                                // HttpGet/ HttpPost/ HttpPut/ HttpDelete
    [ApiController] // --> Class that is used for Model Mapping and Binding
    public class StudentController : ControllerBase
    {
        private readonly IService<StudentDetails, int> service;

        public StudentController(IService<StudentDetails, int> service)
        {
            this.service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var res = await service.GetAsync();
            return Ok(res);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            try
            {
                var res = await service.GetAsync(id);
                if (res == null) throw new Exception("Record not found");
                return Ok(res);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> PostAsync(StudentDetails university)
        {
            if (ModelState.IsValid)
            {
                var res = await service.CreateAsync(university);
                return Ok(res);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, StudentDetails university)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await service.UpdateAsync(id, university);
                    return Ok(res);
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var res = await service.DeleteAsync(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}