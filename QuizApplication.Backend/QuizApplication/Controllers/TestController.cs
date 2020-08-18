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
    [Authorize]
    public class TestController : ControllerBase
    {
        private readonly TestService _testService;

        public TestController(TestService testService)
        {
            _testService = testService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<TestDto> GetTestById(int id)
        {
            var test = _testService.GetTestById(id);

            return Ok(test);
        }

        [HttpGet]
        public ActionResult<TestDto> GetAllTests()
        {
            var tests = _testService.GetAllTests();

            return Ok(tests);
        }

        [HttpPost]
        public async Task<ActionResult<TestDto>> CreateTest([FromBody]TestDto test)
        {
            if (test != null && ModelState.IsValid)
            {
                var newTest = await _testService.AddTestAsync(test);

                return CreatedAtAction("GetTest", new { id = newTest.Id }, newTest);
            }

            return BadRequest("Failed to create a new test");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TestDto>> UpdateTest(int id, [FromBody] TestDto test)
        {
            if (test != null && ModelState.IsValid)
            {
                var updatedTest = await _testService.UpdateTestAsync(id, test);

                return Ok(updatedTest);
            }

            return BadRequest("Failed to update test");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TestDto>> DeleteTest(int id)
        {
            await _testService.DeleteTestAsync(id);

            return NoContent();
        }

        [HttpPost("{testId}")]
        [AllowAnonymous]
        public async Task<ActionResult<TestResultDto>> GetTestResult(int testId, [FromBody] UserAnswersDto userAnswersDto)
        {
            if (userAnswersDto != null && ModelState.IsValid)
            {
                var testResult = await _testService.GetTestResult(testId, userAnswersDto);

                return Ok(testResult);
            }

            return BadRequest("Failed to update test");
        }
    }
}