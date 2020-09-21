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
    public class TestAccessConfigService
    {
        private readonly IRepository<TestAccessConfig> _testAccessConfigRepository;
        private readonly IMapper _mapper;

        public TestAccessConfigService(
            IRepository<TestAccessConfig> testAccessConfigRepository,
            IMapper mapper)
        {
            _testAccessConfigRepository = testAccessConfigRepository;
            _mapper = mapper;
        }

        public TestAccessConfigDto GetTestByLink(int testId, string link)
        {
            var test = _testAccessConfigRepository.GetWithInclude<TestAccessConfig>(
                tac => tac.TestId == testId && tac.UniqueLink == link,
                tac => tac.Include(t => t.Test)
                .ThenInclude(t => t.Questions)
                .ThenInclude(q => q.QuestionAnswers)).FirstOrDefault();

            return _mapper.Map<TestAccessConfigDto>(test);
        }

        public IEnumerable<TestAccessConfigDto> GetTestAccessConfigByTestId(int testId)
        {
            var testAccessConfigs = _testAccessConfigRepository.GetWithInclude<TestAccessConfig>(
                tac => tac.TestId == testId);

            return _mapper.Map<IEnumerable<TestAccessConfigDto>>(testAccessConfigs);
        }

        public async Task CreateTestAccessConfigs(int testId, IEnumerable<TestAccessConfigDto> testAccessConfigDtos)
        {
            var testAccessConfigs = _mapper.Map<IEnumerable<TestAccessConfig>>(testAccessConfigDtos);

            foreach (var item in testAccessConfigs)
            {
                item.UniqueLink = GenerateRandomString();
            }

            await _testAccessConfigRepository.AddRangeAsync(testAccessConfigs);
        }

        private string GenerateRandomString()
        {
            const int stringLength = 10;
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rand = new Random();
            return new string(Enumerable.Repeat(chars, stringLength)
              .Select(s => s[rand.Next(s.Length)]).ToArray());
        }

        public async Task DeleteTestAccessConfigAsync(int testAccessConfigId)
        {
            var testAccessConfig = await _testAccessConfigRepository.GetByIdAsync(testAccessConfigId);

            await _testAccessConfigRepository.DeleteAsync(testAccessConfig);
        }
    }
}
