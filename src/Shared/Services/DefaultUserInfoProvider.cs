// OroKernel
// Copyright (C) 2026 Oscar Rojas
// Licensed under the GNU AGPL v3.0 or later.
// See the LICENSE file in the project root for details.
namespace OroKernel.Shared.Services;

public class DefaultUserInfoProvider : OroKernel.Shared.Interfaces.IUserInfoProvider
{
    private readonly IOptions<UserInfo> _options;

    public DefaultUserInfoProvider(IOptions<UserInfo> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public UserInfo? GetUserInfo()
    {
        return _options.Value;
    }
}