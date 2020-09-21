using System.Collections.Generic;
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
        private readonly TestAccessConfigService _testAccessConfigService;

        public TestController(
            TestService testService,
            TestAccessConfigService testAccessConfigService)
        {
            _testService = testService;
            _testAccessConfigService = testAccessConfigService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<TestDto> GetTestById(int id)
        {
            var test = _testService.GetTestById(id);

            return Ok(test);
        }

        [HttpGet("{testId}/{link}")]
        [AllowAnonymous]
        public ActionResult<TestAccessConfigDto> GetTestByLink(int testId, string link)
        {
            var testAccessConfigDto = _testAccessConfigService.GetTestByLink(testId, link);

            if (testAccessConfigDto != null)
            {
                return Ok(testAccessConfigDto);
            }

            return BadRequest("No test with such link or id");
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

                return Ok(newTest);
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

        [Route("access-config/{testId}")]
        [HttpGet]
        public ActionResult<TestAccessConfigDto> GetTestAccessConfig(int testId)
        {
            var testAccessConfig = _testAccessConfigService.GetTestAccessConfigByTestId(testId);

            return Ok(testAccessConfig);
        }

        [Route("access-config/{testId}")]
        [HttpPost]
        public async Task<ActionResult<TestAccessConfigDto>> CreateTestAccessConfig(
            int testId,
            [FromBody] IEnumerable<TestAccessConfigDto> testAccessConfigs)
        {
            await _testAccessConfigService.CreateTestAccessConfigs(testId, testAccessConfigs);

            return Ok();
        }

        [Route("access-config/{testAccessConfigId}")]
        [HttpDelete]
        public async Task<ActionResult<TestAccessConfigDto>> DeleteTestAccessConfig(int testAccessConfigId)
        {
            await _testAccessConfigService.DeleteTestAccessConfigAsync(testAccessConfigId);

            return Ok();
        }
    }
}