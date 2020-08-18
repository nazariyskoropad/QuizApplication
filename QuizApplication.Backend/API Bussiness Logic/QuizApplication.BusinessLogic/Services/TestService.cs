using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuizApplication.Contracts.DTOs;
using QuizApplication.Contracts.Entities;
using QuizApplication.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizApplication.BusinessLogic.Services
{
    public class TestService
    {
        private readonly IRepository<Test> _testRepository;
        private readonly IRepository<TestResult> _testResultRepository;
        private readonly IMapper _mapper;

        public TestService(
            IRepository<Test> testRepository,
            IRepository<TestResult> testResultRepository, IMapper mapper)
        {
            _testRepository = testRepository;
            _testResultRepository = testResultRepository;
            _mapper = mapper;
        }

        public async Task<TestDto> AddTestAsync(TestDto testDto)
        {
            var test = _mapper.Map<Test>(testDto);
            test.CreatedAt = DateTime.Now;
            var addedTest = await _testRepository.AddAsync(test);

            return _mapper.Map<TestDto>(addedTest);
        }

        public TestDto GetTestById(int id)
        {
            var test = _testRepository.GetWithInclude<Test>(
                x => x.Id == id,
                r => r
                    .Include(c => c.Questions)
                    .ThenInclude(g => g.QuestionAnswers))
                    .FirstOrDefault();

            return _mapper.Map<TestDto>(test);
        }

        public async Task DeleteTestAsync(int id)
        {
            var test = await _testRepository.GetByIdAsync(id);
            await _testRepository.DeleteAsync(test);
        }

        public async Task<TestDto> UpdateTestAsync(int id, TestDto testDto)
        {
            var test = _mapper.Map<Test>(testDto);
            test.UpdatedAt = DateTime.Now;

            await _testRepository.UpdateAsync(test);

            var updatedTest = GetTestById(id);

            return _mapper.Map<TestDto>(updatedTest);
        }

        public IEnumerable<TestDto> GetAllTests()
        {
            var tests = _testRepository.GetWithInclude<Test>(
                x => true,
                r => r
                    .Include(c => c.Questions)
                    .ThenInclude(g => g.QuestionAnswers));

            return _mapper.Map<IEnumerable<TestDto>>(tests);
        }

        public async Task<TestResultDto> GetTestResult(int testId, UserAnswersDto userAnswersDto)
        {
            var testResult = ComputeResults(testId, userAnswersDto);

            testResult.TestId = testId;
            testResult.StartedAt = userAnswersDto.StartedAt;
            testResult.EndedAt = userAnswersDto.EndedAt;
            testResult.UserName = userAnswersDto.UserName;

            await _testResultRepository.AddAsync(testResult);

            return _mapper.Map<TestResultDto>(testResult);
        }

        private TestResult ComputeResults(int testId, UserAnswersDto userAnswersDto)
        {
            var testResult = new TestResult();

            var test = GetTestById(testId);
            var testQuestions = test.Questions.ToList();

            var userQApairs = userAnswersDto.QApairs.ToList();

            foreach (var qapair in userQApairs)
            {
                if (qapair.AnswerId == 0)
                {
                    testResult.SkippedCount++;
                }
                else
                {
                    var testQuestion = testQuestions.FirstOrDefault(q => q.Id == qapair.QuestionId);
                    if (testQuestion == null)
                    {
                        throw new ArgumentException("No question with this id");
                    }

                    var answer = testQuestion.QuestionAnswers.FirstOrDefault(qa => qa.Id == qapair.AnswerId);
                    if (answer != null && answer.IsCorrect)
                    {
                        testResult.CorrectCount++;
                        testResult.Points += testQuestion.Points;
                    }
                    else
                    {
                        testResult.IncorrectCount++;
                    }
                }
            }

            return testResult;
        }
    }
}
