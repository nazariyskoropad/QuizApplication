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
        private readonly IRepository<Question> _questionRepository;
        private readonly IMapper _mapper;

        public TestService(
            IRepository<Test> testRepository,
            IRepository<TestResult> testResultRepository,
            IRepository<Question> questionRepository,
            IRepository<TestAccessConfig> testAccessConfigRepository, IMapper mapper)
        {
            _testRepository = testRepository;
            _testResultRepository = testResultRepository;
            _questionRepository = questionRepository;
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
                    .Include(t => t.TestResults)
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

            var testToUpdate = _testRepository.GetWithInclude<Test>(
                x => x.Id == id,
                r => r
                .Include(c => c.Questions)).FirstOrDefault();
            await _questionRepository.DeleteRangeAsync(testToUpdate.Questions);
            await _questionRepository.AddRangeAsync(test.Questions);

            await _testRepository.UpdateAsync(test);

            var updatedTest = await _testRepository.GetByIdAsync(id);

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
            testResult.Points = Math.Round(testResult.Points);

            var addedTestResult = await _testResultRepository.AddAsync(testResult);

            return _mapper.Map<TestResultDto>(addedTestResult);
        }

        private TestResult ComputeResults(int testId, UserAnswersDto userAnswersDto)
        {
            var testResult = new TestResult();

            var test = GetTestById(testId);
            var testQuestions = test.Questions.ToList();

            var userQApairs = userAnswersDto.QApairs.ToList();

            foreach (var question in testQuestions)
            {
                var userAnswersToQuestion = userQApairs.Where(ua => ua.QuestionId == question.Id).ToList();
                if (userAnswersToQuestion.Count == 0)
                {
                    testResult.SkippedCount++;
                }
                else
                {
                    var questionAnswersCount = question.QuestionAnswers.Count;
                    var questionAnswersCorrectCount = 0;
                    var questionAnswersIncorrectCount = 0;

                    foreach (var questionAnswer in question.QuestionAnswers)
                    {
                        if (questionAnswer.IsCorrect)
                        {
                            if (userAnswersToQuestion.FirstOrDefault(cqa => cqa.AnswerId == questionAnswer.Id) != null)
                            {
                                questionAnswersCorrectCount++;
                            }
                            else
                            {
                                questionAnswersIncorrectCount++;
                            }
                        }
                        else
                        {
                            if (userAnswersToQuestion.FirstOrDefault(cqa => cqa.AnswerId == questionAnswer.Id) == null)
                            {
                                questionAnswersCorrectCount++;
                            }
                            else
                            {
                                questionAnswersIncorrectCount++;
                            }
                        }
                    }

                    var totalPointsForQuestion = question.Points * questionAnswersCorrectCount / questionAnswersCount;
                    if (totalPointsForQuestion > 0)
                    {
                        testResult.CorrectCount++;
                    }
                    else
                    {
                        testResult.IncorrectCount++;
                    }

                    testResult.Points += totalPointsForQuestion;
                }
            }

            return testResult;
        }
    }
}
