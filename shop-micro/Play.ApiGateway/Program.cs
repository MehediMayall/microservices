using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Authentication
builder.Services
    .AddAuthentication(BearerTokenDefaults.AuthenticationScheme)
    .AddBearerToken();

builder.Services.AddAuthorization(option =>{
    option.AddPolicy("catalog-api-access", 
    policy => policy.RequireAuthenticatedUser().RequireClaim("catalog-api-access", true.ToString()));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/login", async(bool catalogApiAccess=false) =>{
    
    return Results.SignIn(
        new ClaimsPrincipal(
        new ClaimsIdentity(
            [
                new Claim("sub",Guid.NewGuid().ToString()),
                new Claim("catalog-api-access", catalogApiAccess.ToString())
            ], BearerTokenDefaults.AuthenticationScheme
        )), 
        authenticationScheme: BearerTokenDefaults.AuthenticationScheme
    );
});

app.MapReverseProxy();
app.UseAuthentication();
app.UseAuthorization();


app.Run();

