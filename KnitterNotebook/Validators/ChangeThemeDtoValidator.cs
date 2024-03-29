﻿using FluentValidation;
using KnitterNotebook.Models.Dtos;
using KnitterNotebook.Properties;
using KnitterNotebook.Services.Interfaces;

namespace KnitterNotebook.Validators;

public class ChangeThemeDtoValidator : AbstractValidator<ChangeThemeDto>
{
    private readonly IUserService _userService;
    private readonly IThemeService _themeService;

    public ChangeThemeDtoValidator(IUserService userService,
                                   IThemeService themeService)
    {
        _userService = userService;
        _themeService = themeService;

        RuleFor(dto => dto.UserId)
           .MustAsync(async (id, cancellationToken) => await _userService.UserExistsAsync(id))
           .WithMessage(Translations.UserNotFound);

        RuleFor(dto => dto.ThemeName)
           .MustAsync(async (themeName, cancellationToken) => await _themeService.ThemeExistsAsync(themeName))
           .WithMessage(Translations.ThemeNotFound);
    }
}