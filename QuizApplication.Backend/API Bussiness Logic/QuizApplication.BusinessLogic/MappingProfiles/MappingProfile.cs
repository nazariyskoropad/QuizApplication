using AutoMapper;
using QuizApplication.Contracts.DTOs;
using QuizApplication.Contracts.Entities;

namespace QuizApplication.BusinessLogic.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            ConfigureMapping();
        }

        private void ConfigureMapping()
        {
            CreateMap<Test, TestDto>()
                .ReverseMap();

            CreateMap<Question, QuestionDto>()
                .ReverseMap();

            CreateMap<QuestionAnswer, QuestionAnswerDto>()
                .ReverseMap();
        }
    }
}
