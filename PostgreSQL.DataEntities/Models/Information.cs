using System;
using System.Collections.Generic;

namespace PostgreSQL.DataEntities.Models;

public partial class Information
{
// data models
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
