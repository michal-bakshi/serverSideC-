﻿using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class User
{
    public int Idusers { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Role { get; set; }
}

