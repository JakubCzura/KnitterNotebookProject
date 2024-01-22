using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace KnitterNotebook.Helpers.Extensions.DependencyRegistration;

public static class ValidatorsRegistration
{
    public static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
        services.AddScoped<IValidator<ChangeNicknameDto>, ChangeNicknameDtoValidator>();
        services.AddScoped<IValidator<ChangeEmailDto>, ChangeEmailDtoValidator>();
        services.AddScoped<IValidator<ChangePasswordDto>, ChangePasswordDtoValidator>();
        services.AddScoped<IValidator<ChangeThemeDto>, ChangeThemeDtoValidator>();
        services.AddScoped<IValidator<CreateMovieUrlDto>, CreateMovieUrlDtoValidator>();
        services.AddScoped<IValidator<CreateSampleDto>, CreateSampleDtoValidator>();
        services.AddScoped<IValidator<CreateYarnDto>, CreateYarnDtoValidator>();
        services.AddScoped<IValidator<CreateNeedleDto>, CreateNeedleDtoValidator>();
        services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
        services.AddScoped<IValidator<PlanProjectDto>, PlanProjectDtoValidator>();
        services.AddScoped<IValidator<LogInDto>, LogInDtoValidator>();
        services.AddScoped<IValidator<CreateProjectImageDto>, CreateProjectImageDtoValidator>();
        services.AddScoped<IValidator<ChangeProjectStatusDto>, ChangeProjectStatusDtoValidator>();
        services.AddScoped<IValidator<EditProjectDto>, EditProjectDtoValidator>();

        return services;
    }
}