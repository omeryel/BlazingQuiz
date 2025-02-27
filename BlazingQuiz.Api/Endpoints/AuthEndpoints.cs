﻿using BlazingQuiz.Api.Services;
using BlazingQuiz.Shared.DTOs;

namespace BlazingQuiz.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapPost("/api/auth/login", async (LoginDto dto, AuthService authService) =>
            {
                return Results.Ok(await authService.LoginAsync(dto));
            });


            builder.MapPost("/api/auth/register", async (RegisterDto dto, AuthService authService) =>
            {
                return Results.Ok(await authService.RegisterAsync(dto));
            });

            return builder;
        }
    }
}
