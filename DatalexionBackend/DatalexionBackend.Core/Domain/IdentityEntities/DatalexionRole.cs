﻿using Microsoft.AspNetCore.Identity;

namespace DatalexionBackend.Core.Domain.IdentityEntities;

public class DatalexionRole : IdentityRole
{
    public DateTime Creation { get; set; } = DateTime.Now;

    public DateTime Update { get; set; } = DateTime.Now;

}
