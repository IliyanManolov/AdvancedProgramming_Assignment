﻿using IMDB.Application.Abstractions.Services;
using IMDB.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IMDB.Application;

public static class Configuration
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordService, PasswordService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWatchListService, WatchListService>();
        services.AddScoped<IMediaTransformer, MediaTransformer>();

        services.AddScoped<IActorService, ActorService>();
        services.AddScoped<IDirectorService, DirectorService>();

        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IMediaService, MediaService>();

        services.AddScoped<IShowEpisodeService, ShowEpisodeService>();

        return services;
    }
}
