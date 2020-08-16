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
        private readonly IMapper _mapper;

        public TestService(IRepository<Test> testRepository, IMapper mapper)
        {
            _testRepository = testRepository;
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
    }
}
