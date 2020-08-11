using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApplication.BusinessLogic.Services;
using QuizApplication.Contracts.DTOs;

namespace QuizApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly TestService _testService;

        public TestController(TestService testService)
        {
            _testService = testService;
        }

        [HttpGet("{id}")]
        public ActionResult<TestDto> GetTestById(int id)
        {
            try
            {
                var test = _testService.GetTestById(id);

                return Ok(test);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return BadRequest("Failed to get a test");
        }

        [HttpGet]
        public ActionResult<TestDto> GetAllTests()
        {
            try
            {
                var tests = _testService.GetAllTests();

                return Ok(tests);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return BadRequest("Failed to get all tests");
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TestDto>> CreateTest([FromBody]TestDto test)
        {
            if (test != null && ModelState.IsValid)
            {
                try
                {
                    var newTest = await _testService.AddTestAsync(test);

                    return CreatedAtAction("GetTest", new { id = newTest.Id }, newTest);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return BadRequest("Failed to create a new test");
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<TestDto>> UpdateTest(int id, [FromBody] TestDto test)
        {
            if (test != null && ModelState.IsValid)
            {
                try
                {
                    var updatedTest = await _testService.UpdateTestAsync(id, test);

                    return Ok(updatedTest);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return BadRequest("Failed to create a new test");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<TestDto>> DeleteTest(int id)
        {
            try
            {
                await _testService.DeleteTestAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return BadRequest("Failed to delete a test");
        }
    }
}