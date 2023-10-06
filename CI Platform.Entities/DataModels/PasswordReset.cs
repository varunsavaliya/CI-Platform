﻿using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.DataModels;

public partial class PasswordReset
{
    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int PasswordResetId { get; set; }
}
