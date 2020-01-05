using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Hackathon.Models;
using Hackathon.Services;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json;

namespace Hackathon.Controllers
{
    [Route("[controller]")] // -> Invoke method using its name

    [ApiController]
    [EnableCors("corspolicy")]
    public class StudentController : ControllerBase
    {
        private readonly IService<StudentDetails, int> service;
        private readonly IPreferenceService<UserPreferences,int> prefService;

        public StudentController(IService<StudentDetails, int> service,IPreferenceService<UserPreferences,int> prefService)
        {
            this.service = service;
            this.prefService = prefService;
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPaginatedData()
        {
            List<StudentDetails> responseClone;
            var studentId = Convert.ToInt32(Request.Query["StudentId"]);
            var filter = Request.Query["filter"];
            var pageIndex = Convert.ToInt32(Request.Query["pageIndex"]);
            var pageSize = Convert.ToInt32(Request.Query["pageSize"]);
            var sort = Request.Query["sort"];
            try
            {
                var response = await service.GetAsync();
                if (response == null) throw new Exception("Record not found");
                else
                {
                    responseClone = response.ToList();
                    var initialPos = pageIndex * pageSize;

                    if (responseClone.Count > 0)
                    {
                        responseClone = responseClone.GetRange(initialPos, pageSize);
                    }
                    if (sort == "desc")
                    {
                        responseClone.Reverse();
                    }

                    if (!string.IsNullOrEmpty(filter))
                    {
                        filter = FirstCharToUpper(filter);
                        var propertyInfo = typeof(StudentDetails).GetProperty(filter);
                        if (sort == "desc")
                        {
                            responseClone = responseClone.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        else
                        {
                            responseClone = responseClone.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                        }

                    }
                }
                return Ok(responseClone);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        public string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetHeadersData()
        {

            try
            {
                var response = await prefService.GetHeadersAsync();
                if (response == null) throw new Exception("Record not found");
                return Ok(response.FirstOrDefault().PreferanceValue);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PostHideData()
        {
            try
            {
                var columnHeader = Request.Query["headerName"];
                columnHeader = '"' + columnHeader + '"';
                var response = await prefService.GetHeadersAsync();
                var currentPreferences = response.FirstOrDefault().PreferanceValue.ToString();
                var newPreferences = currentPreferences.Replace(columnHeader + ":true", columnHeader + ":false");
                var updateResponse = prefService.Update(1, newPreferences);
                return Ok(updateResponse);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> PostShowData()
        {
            try
            {
                var columnHeader = Request.Query["headerName"];
                columnHeader = '"'+ columnHeader + '"';
                var res = await prefService.GetHeadersAsync();
                var currentPreferences = res.FirstOrDefault().PreferanceValue.ToString();
                var newPreferences = currentPreferences.Replace(columnHeader + ":false" , columnHeader + ":true");
                var response = prefService.Update(1, newPreferences);
                return Ok(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(StudentDetails student)
        {
            if (ModelState.IsValid)
            {
                var res = await service.CreateAsync(student);
                return Ok(res);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, StudentDetails student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await service.UpdateAsync(id, student);
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