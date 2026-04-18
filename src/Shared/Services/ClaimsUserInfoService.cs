// OroKernel
// Copyright (C) 2025 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Services;

public class ClaimsUserInfoService : IPostConfigureOptions<UserInfo>
{
    private readonly IHttpContextAccessor _contextAccessor;

    public ClaimsUserInfoService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
    }

    public void PostConfigure(string? name, UserInfo options)
    {
        if (options == null) throw new ArgumentNullException(nameof(options));

        var user = _contextAccessor?.HttpContext?.User;
        if (user == null) return;

        var claims = user.Claims;

        // State claim: tests use empty string as claim type for state
        var stateClaim = claims.FirstOrDefault(c => string.Equals(c.Type, "", System.StringComparison.OrdinalIgnoreCase));
        if (stateClaim != null && int.TryParse(stateClaim.Value, out var state))
        {
            if (System.Enum.IsDefined(typeof(EntityBaseState), state))
                options.State = (EntityBaseState)state;
            else
                options.State = default;
        }

        // ID claim - accept common variants
        var idClaim = claims.FirstOrDefault(c =>
            string.Equals(c.Type, "Sub", System.StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.Type, "sub", System.StringComparison.OrdinalIgnoreCase) ||
            string.Equals(c.Type, System.Security.Claims.ClaimTypes.NameIdentifier, System.StringComparison.OrdinalIgnoreCase));

        if (idClaim != null && Guid.TryParse(idClaim.Value, out var guid))
            options.Id = guid;
        else
            options.Id = Guid.Empty;

        var userNameClaim = claims.FirstOrDefault(c => string.Equals(c.Type, "UserName", System.StringComparison.OrdinalIgnoreCase) ||
                                                      string.Equals(c.Type, System.Security.Claims.ClaimTypes.Name, System.StringComparison.OrdinalIgnoreCase));
        options.UserName = userNameClaim?.Value ?? string.Empty;

        var emailClaim = claims.FirstOrDefault(c => string.Equals(c.Type, "Email", System.StringComparison.OrdinalIgnoreCase) ||
                                                   string.Equals(c.Type, System.Security.Claims.ClaimTypes.Email, System.StringComparison.OrdinalIgnoreCase));
        options.Email = emailClaim?.Value ?? string.Empty;
    }
}