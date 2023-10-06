using System;
using System.Collections.Generic;

namespace CI_Platform.Entities.DataModels;

public partial class ContactU
{
    public long ContactUsId { get; set; }

    public long UserId { get; set; }

    public string Subject { get; set; } = null!;

    public string Message { get; set; } = null!;
}
