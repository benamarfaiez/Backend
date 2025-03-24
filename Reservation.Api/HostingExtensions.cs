using Reservation.Api.Middlewares;
using Reservation.Dal.Repositories;
using Reservation.Domain.Interfaces.Repositories;
using Reservation.Domain.Interfaces.Services;
using Reservation.Domain.Services;
using Scalar.AspNetCore;

namespace Reservation.Api;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
        builder.Services.AddAuthentication()
           .AddJwtBearer(options =>
           {
               options.Authority = "https://localhost:5001";
               options.TokenValidationParameters.ValidateAudience = false;
           });

        //builder.Services.AddAuthorization();
        //builder.Services.AddAuthorizationBuilder()
        //    .AddPolicy("ReservationApiPolicy", policy =>
        //    {
        //        policy.RequireClaim("scope", "Reservation.Api");
        //    });
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        });

        builder.Services.AddScoped<IBookingService, BookingService>();
        builder.Services.AddScoped<IBookingRepository, BookingRepository>();
        builder.Services.AddScoped<IRoomRepository, RoomRepository>();
        builder.Services.AddScoped<IRoomService, RoomService>();
        builder.Services.AddScoped<IPersonRepository, PersonRepository>();
        builder.Services.AddScoped<IPersonService, PersonService>();
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseMiddleware<LoggingMiddleware>();
        app.UseMiddleware<ErrorLoggingMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }
}